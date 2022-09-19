using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking
{

    internal class CurrencyConverter
    {
        public Dictionary<string, decimal>? RatesEuroToX = new Dictionary<string, decimal>
        ///<summary>
        ///Contains the rate of Euro to all available currencies.
        ///This shall be filled by data acquired through the ECB API later.
        ///</summary>
        {
            {"USD", 0.9954M},
            {"SEK", 10.7541M},
            {"DKK", 7.4366M},
            {"EUR", 1M}
        };

        public CurrencyConverter()
        {
        }

        public string DisplayCurrencies()
        ///<summary>
        ///Returns all available currencies as a string
        ///so they can be formated by the calling function.
        ///</summary>
        {
            List<string>? outputList = new();
            string? outputString = null;

            
            if (RatesEuroToX == null || RatesEuroToX.Count == 0)
            {
                // In a perfect world this would be an exception. But since so far the
                // currencies are hard coded it doesn't make a difference.
                // This won't ever be called. :)
                return "Currencies not initialised!";
            }

            outputList = RatesEuroToX.Keys.ToList();
            outputList.Sort();
            
            for(int i = 0; i < outputList.Count; i++)
            {
                if (i < outputList.Count - 1)
                {
                    outputString += outputList[i] + ", ";
                }
                else
                {
                    outputString += outputList[i];
                }
            }
            return outputString;
        }

        public decimal? ExchangeRateEuroTo(string currency)
        ///<summary>
        ///Return the exchange rate from Euro tho the provided currency if the currency
        ///exists in the dictionary. Otherwise returns null.
        ///</summary>
        {
            if (RatesEuroToX.TryGetValue(currency.ToUpper(), out decimal rate))
            {
                return rate;
            }
            return null;
        }

        public decimal? ConvertFromTo(string sourceCurrency, string targetCurrency, decimal? amount)
        ///<summary>
        ///Returns the amount of sourceCurrency in targetCurrency.
        ///</summary>
        {
            return Math.Round(Convert.ToDecimal(ExchangeRateEuroTo(targetCurrency) / ExchangeRateEuroTo(sourceCurrency) * amount), 2);
        }



        /// <summary>
        /// Returns true if rate for currency is available or false if not.
        /// </summary>
        public bool IsCurrency(string currency)
        {
            if (RatesEuroToX == null || RatesEuroToX.Count == 0)
            {
                return false;
            }

            return RatesEuroToX.ContainsKey(currency.ToUpper());
        }



    }
}