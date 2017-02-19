using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pochete.Data;
using Pochete.Models;

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
            {
                return NotFound();
            }

            var currencyRates = _context.CurrenciesRates.Where(m => m.CurrencyId == id);

            if (currencyRates == null)
            {
                return NotFound();
            }

            return View(currencyRates);
        }
    }
}