namespace AssetTracking
{
    internal class AssetList
    {
        public bool AddAsset(Asset newAsset)
        {
            DbSession context = new();

            context.Add(newAsset);
            // I would like to add a check if more than one asset has
            // been changed, but it feels so useless...
            if (context.SaveChanges() > 0) return true;
            return false;
        }

        public bool AddAssets(List<Asset> newAssets)
        {
            DbSession context = new();

            context.AddRange(newAssets);
            if (context.SaveChanges() > 0) return true;
            return false;
        }

        public Asset GetAsset(int id)
        {
            DbSession context = new();
            return context.Assets.Find(id);
        }

        public List<Asset> GetAssetList()
        {
            DbSession context = new();

            var query = context.Assets
                    .OrderBy(asset => asset.Office)
                    .ThenBy(asset => asset.PurchaseDate);

            return query.ToList();
        }

        public bool RemoveAsset(Asset asset)
        {
            DbSession context = new();

            context.Assets.Remove(asset);
            if (context.SaveChanges() > 0) return true;
            return false;
        }

        public bool UpdateAsset(Asset asset)
        {
            DbSession context = new();

            context.Assets.Update(asset);
            if (context.SaveChanges() > 0) return true;
            return false;
        }

        public string UpdateLocalPriceToday()
        {
            DbSession context = new();
            CurrencyConverter currencyConverter = new();

            if (!context.Assets.Any())
                return "No assets found. Operation aborted.";

            foreach (Asset asset in context.Assets)
            {
                asset.LocalPriceToday = currencyConverter.ConvertFromTo("USD", asset.Currency, asset.PriceInUSD);
            }

            int numberOfPricesUpdated = context.SaveChanges();
            if (numberOfPricesUpdated == 1)
            {
                return $"{numberOfPricesUpdated} local price updated.";
            }
            else
            {
                return $"{numberOfPricesUpdated} local prices updated.";
            }
        }
    }
}
