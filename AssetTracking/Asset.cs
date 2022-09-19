using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking
{
    internal class Asset
    {
        private string? type;
        private string? brand;
        private string? model;
        private string? office;
        private DateTime? purchaseDate;
        private decimal? priceInUSD;
        private string? currency;
        private decimal? localPriceToday;

        // Create properties for all fields. Feels a bit overkill but
        // is probably a good idea to get a feel fot it.1

        public string? Type { get => type; set => type = value; }
        public string? Brand { get => brand; set => brand = value; }
        public string? Model { get => model; set => model = value; }
        public string? Office { get => office; set => office = value; }
        public DateTime? PurchaseDate { get => purchaseDate; set => purchaseDate = value; }
        public decimal? PriceInUSD { get => priceInUSD; set => priceInUSD = value; }
        public string? Currency { get => currency; set => currency = value; }
        internal decimal? LocalPriceToday
        {
            get
            {
                return localPriceToday;
            }

            set 
            { 
                localPriceToday = value;
            }
        }

        public Asset(string? type, string? brand, string? model, string? office, DateTime? purchaseDate, decimal? priceInUSD, string? currency)
        {
            this.Type = type;
            this.Brand = brand;
            this.Model = model;
            this.Office = office;
            this.PurchaseDate = purchaseDate;
            this.PriceInUSD = priceInUSD;
            this.Currency = currency;
        }

    }
}
