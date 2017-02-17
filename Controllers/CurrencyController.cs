using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public ActionResult CurrencyList()
        {
            List<Currency> currencies = _context.Currencies.ToList();

            return View(currencies);
        }
    }
}