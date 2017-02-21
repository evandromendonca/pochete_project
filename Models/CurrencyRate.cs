using System;
using System.ComponentModel.DataAnnotations;

namespace Pochete.Models
{
    public class CurrencyRate
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Currency)]
        public decimal Rate { get; set; }

        public int CurrencyId { get; set; }

        public int ReferenceCurrencyId { get; set; }

        public virtual Currency Currency { get; set; }

        public virtual Currency ReferenceCurrency { get; set; }

        public CurrencyRate()
        {
        }
    }
}
