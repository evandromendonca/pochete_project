using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pochete.ViewModels
{
    public class CurrencyRatesVM
    {
        public List<DateRate> Rates { get; set; }

        public List<string> CurrenciesCodes { get; set; }

        public string ReferenceCurrency { get; set; }

        public string Currency { get; set; }

        public int CurrencyId { get; set; }

        public CurrencyRatesVM()
        {
        }
    }

    public class DateRate
    {
        [DataTypeAttribute(DataType.Date)]
        public DateTime Date { get; set; }

        [DataTypeAttribute(DataType.Currency)]
        public decimal Rate { get; set; }
    } 
}