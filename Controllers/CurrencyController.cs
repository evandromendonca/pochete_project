using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pochete.Data;
using Pochete.Models;
using Pochete.ViewModels;

namespace Pochete.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CurrencyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult CurrencyList()
        {
            var currencies = _context.Currencies.ToList()
                .OrderBy(o => string.IsNullOrWhiteSpace(o.Name))
                .ThenBy(o => o.Name)
                .ThenBy(o => o.Code);

            return View(currencies);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name", "Code")] Currency currency)
        {
            if (ModelState.IsValid)
            {
                _context.Currencies.Add(currency);
                await _context.SaveChangesAsync();
                return RedirectToAction("CurrencyList");
            }

            return View(currency);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currency = await _context.Currencies.SingleOrDefaultAsync(m => m.Id == id);
            if (currency == null)
            {
                return NotFound();
            }
            return View(currency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Code")] Currency currency)
        {
            if (id != currency.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Currencies.Attach(currency);
                _context.Entry(currency).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("CurrencyList");
            }
            
            return View(currency);
        }

        [HttpGet]
        public ActionResult CurrencyRates(int? id)
        {
            if (id == null)
                return NotFound();

            string referenceCurrency = "USD";

            var currencyRates = _context.CurrenciesRates
                .Where(m => m.CurrencyId == id && m.ReferenceCurrency.Code == referenceCurrency)
                .Include(o => o.Currency);
            var referenceCurrencies = _context.Currencies
                .Select(m => m.Code).OrderBy(o => o);

            if (currencyRates == null)
                return NotFound();

            CurrencyRatesVM ratesVM = new CurrencyRatesVM();
            ratesVM.CurrencyId = id ?? 0;
            ratesVM.Rates = currencyRates.Select(o => new DateRate(){Date = o.Date, Rate = o.Rate}).OrderByDescending(o => o.Date).ToList();
            ratesVM.CurrenciesCodes = referenceCurrencies.ToList();
            ratesVM.ReferenceCurrency = referenceCurrency;
            ratesVM.Currency = currencyRates.First().Currency.Code;
        
            return View(ratesVM);
        }

        [HttpPost]
        [ActionName("CurrencyRates")]
        public ActionResult CurrencyRatesPost(CurrencyRatesVM ratesVM)
        {
            if (string.IsNullOrWhiteSpace(ratesVM.ReferenceCurrency))
                ratesVM.ReferenceCurrency = "USD";

            var currencyRates = _context.CurrenciesRates
                .Where(m => m.Currency.Code == ratesVM.Currency && m.ReferenceCurrency.Code == ratesVM.ReferenceCurrency)
                .Include(o => o.Currency);

            ratesVM.Rates = currencyRates.Select(o => new DateRate(){Date = o.Date, Rate = o.Rate}).OrderByDescending(o => o.Date).ToList();

            return Json(new { 
                rates = ratesVM.Rates, 
                currency = ratesVM.Currency, 
                referencecurrency = ratesVM.ReferenceCurrency });
        }
    }
}