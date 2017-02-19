using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Pochete.Data;
using Pochete.Models;
using Newtonsoft.Json;

namespace Pochete.Helpers
{
    public class HelperCurrencyConverter
    {
        public HelperCurrencyConverter()
        {
        }

        public decimal Convert(DateTime date, string firstCode, string secondCode, decimal amount, ApplicationDbContext context = null, 
            string currencyReferenceCode = "USD", bool searchOnline = true, bool saveNewRates = true)
        {
            // first check if the amount is zero, or the currency is the same, in this case just return the same amount
            if (amount == 0 || firstCode == secondCode)
                return amount;

            // define the variable to do the math
            decimal firstRate = 0;
            decimal secondRate = 0;

            // check first and second rate based on reference and database in case of context
            GetRatesOffline(date, firstCode, secondCode, ref firstRate, ref secondRate, context, currencyReferenceCode);

            Dictionary<string, decimal> foundRates = null;

            // if one of rates was not found and the online search is enabled, try to find it online
            if ((firstRate == 0 || secondRate == 0) && searchOnline)
                foundRates = GetRatesOnline(date, firstCode, secondCode, ref firstRate, ref secondRate, currencyReferenceCode);

            // if the method is configured to save the currencies rates on database
            if (saveNewRates && foundRates != null)
                StoreRatesByDate(foundRates, date, currencyReferenceCode, context);

            // throw an exception in case of not found rate
            if (firstRate == 0 || secondRate == 0)
                throw new Exception("The rate of selected currencies was not found.");

                return Math.Round((amount/firstRate) * secondRate, 4);
        }

        private void GetRatesOffline(DateTime date, string firstCode, string secondCode, ref decimal firstRate, ref decimal secondRate, 
            ApplicationDbContext context = null, string referenceCurrency = "USD")
        {
            // verify is some currency is the reference, only one can be because we already know they are not equal
            if (firstCode == referenceCurrency)
                firstRate = 1;
            else if (secondCode == referenceCurrency)
                secondRate = 1;
            
            // try to get the values of first and second rate in the database, if the context was provided
            if (context == null)
                return;

            // if the rates are not set, try to find it in the given context, if the context is null, the approach will return zero
            if (firstRate == 0)
                firstRate = context?.CurrenciesRates.SingleOrDefault(o => o.Currency.Code == firstCode && o.Date == date && o.ReferenceCurrency.Code == referenceCurrency)?.Rate ?? 0;

            if (secondRate == 0)
                secondRate = context?.CurrenciesRates.SingleOrDefault(o => o.Currency.Code == secondCode && o.Date == date && o.ReferenceCurrency.Code == referenceCurrency)?.Rate ?? 0;
        }

        private Dictionary<string, decimal> GetRatesOnline(DateTime date, string firstCode, string secondCode, ref decimal firstRate, ref decimal secondRate, 
            string referenceCurrency, ApplicationDbContext context = null)
        {
            // get the rates online from a provider
            Dictionary<string, decimal> values = RatesFromProvider(date, referenceCurrency);

            if (firstRate == 0)
                firstRate = values.SingleOrDefault(o => o.Key == firstCode).Value;
            
            if (secondRate == 0)
                secondRate = values.SingleOrDefault(o => o.Key == secondCode).Value;

            return values;
        }

        private Dictionary<string, decimal> RatesFromProvider(DateTime date, string referenceCurrency)
        {
            // get the rates from fixer
            HttpClient hc = new HttpClient();
            string response = hc.GetStringAsync($"http://api.fixer.io/{date.ToString("yyyy-MM-dd")}?base={referenceCurrency}").Result;

            // get the values of the rates for the selected day
            return JsonConvert.DeserializeObject<Dictionary<string, decimal>>(
                JsonConvert.DeserializeObject<Dictionary<string, object>>(response)["rates"].ToString());
        }

        public void StoreRatesByDate(Dictionary<string, decimal> values, DateTime date, string currencyReferenceCode, ApplicationDbContext context)
        {
            Currency referenceCurrency = context.Currencies.SingleOrDefault(o => o.Code == currencyReferenceCode);

            // for each value, make a insert in the database
            foreach(KeyValuePair<string, decimal> value in values)
            {
                // try to get the currency
                Currency currency = context.Currencies.SingleOrDefault(o => o.Code == value.Key);
                decimal rate = 0;

                // check if the currency exists, if not, add new
                if (currency == null)
                {
                    currency = new Currency() { Code = value.Key };
                    context.Currencies.Add(currency);
                }

                // check if the rate exists, if not, add new
                if (!context.CurrenciesRates.Any(o => o.Currency.Code == value.Key && o.Date == date && o.ReferenceCurrency == referenceCurrency))
                {
                    rate = value.Value;
                    context.CurrenciesRates.Add(
                        new CurrencyRate() { Date = date, Rate = rate, Currency = currency, ReferenceCurrency = referenceCurrency});
                }
            }

            // save the new rates
            context.SaveChanges();
        }
    }
}