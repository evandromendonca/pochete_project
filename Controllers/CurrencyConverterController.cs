using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Pochete.Data;
using Pochete.Helpers;
using Pochete.ViewModels;

namespace Pochete.Controllers
{
    public class CurrencyConverterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CurrencyConverterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult Converter([Bind("Date, First_currency, Second_currency, Amount")]CurrencyConverterVM ccvm)
        {   
            string referenceCurrency = "USD";

            // call the converter helper to convert the value
            HelperCurrencyConverter helper = new HelperCurrencyConverter();
            decimal final_value = helper.Convert(ccvm.Date, ccvm.First_currency, ccvm.Second_currency, ccvm.Amount, _context,
                referenceCurrency, true, true);

            return Json(new { final_value = final_value});
        }

        [HttpGet]
        public IActionResult Converter()
        {
            // select the currencies available in the database
            IEnumerable<string> currencyCodesList = _context.Currencies.Select(o => o.Code).OrderBy(o => o);

            // create a view model to display in the view
            CurrencyConverterVM ccvm = new CurrencyConverterVM();
            ccvm.Date = DateTime.Today;
            ccvm.Currencies = new List<string>(currencyCodesList);

            return View(ccvm);
        }
    }
}