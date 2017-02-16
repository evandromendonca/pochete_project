using System;

namespace Pochete.Models
{
    public class CurrencyRate
    {
        public DateTime Date { get; set; }

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
