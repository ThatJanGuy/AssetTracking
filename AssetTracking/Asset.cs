using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking
{
    internal class Asset
    {
        public int id { get; set; }
        public string? Type { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Office { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal? PriceInUSD { get; set; }
        public string? Currency { get; set; }
        public decimal? LocalPriceToday { get; set; }

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
