using LittleHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTracking
{
    internal class AssetList
    {
        private List<Asset> assets;
        private CurrencyConverter currencyConverter = new();
        private Dictionary<string, string> Offices = new Dictionary<string, string>
        {
            {"Malmö", "SEK"},
            {"Copenhagen", "DKK"},
            {"Hamburg", "EUR"}
        };

        public AssetList()
        {
            assets = new List<Asset>();
        }

        public void AddAsset()
        // Collects data from the command line and creates a new Asset
        // that it the adds to assetList. Uses the GetInput functions
        // of LittleHelpers a lot. They set the bool exit to true when
        // the user triggers the exit condition.
        {
            while (true)
            {
                bool exit = false;

                string? type = null;
                string? brand = null;
                string? model = null;
                string? office = null;
                DateTime? purchaseDate = null;
                decimal? priceInUSD = null;
                string? currency = null;

                Console.Clear();
                TextManipulation.MakeHeading("Create a new asset ('x' to exit)");
                Console.WriteLine("\nPlease enter the following information:");

                while (string.IsNullOrEmpty(type)) 
                {
                    Console.Write("Type of asset ".PadRight(21) + "> ");
                    type = GetInput.GetString(out exit, "x");
                    if (exit) break;
                }
                if (exit) break;

                while (string.IsNullOrEmpty(brand)) 
                {
                    Console.Write("Brand ".PadRight(21) + "> ");
                    brand = GetInput.GetString(out exit, "x");
                    if (exit) break;
                }
                if (exit) break;

                while (string.IsNullOrEmpty(model)) 
                {
                    Console.Write("Model ".PadRight(21) + "> ");
                    model = GetInput.GetString(out exit, "x");
                    if (exit) break;
                }
                if (exit) break;

                while (string.IsNullOrEmpty(office)) 
                {
                    Console.Write("Used at which office ".PadRight(21) + "> ");
                    office = GetInput.GetString(out exit, "x");
                    if (exit) break;
                    if (!Offices.ContainsKey(TextManipulation.ToTitle(office)))
                    {
                        List<string>? outputList = new();
                        string? outputString = null;

                        outputList = Offices.Keys.ToList();
                        outputList.Sort();

                        for (int i = 0; i < outputList.Count; i++)
                        {
                            if (i < outputList.Count - 1)
                            {
                                outputString += outputList[i] + ", ";
                            }
                            else
                            {
                                outputString += outputList[i] + ".\n";
                            }
                        }

                        TextManipulation.ColoredText(
                            "Office not available!\n" +
                            "Available options are:\n" +
                            outputString,
                            "Red"
                            );
                        office = null;
                    }
                    else
                    {
                        office = TextManipulation.ToTitle(office);
                        currency = Offices[office];
                    }
                }
                if (exit) break;

                while (purchaseDate == null)
                {
                    Console.Write("Date of purchase ".PadRight(21) + "> ");
                    purchaseDate = GetInput.GetDateTime(out exit, "x");
                    if (exit) break;
                }
                if (exit) break;

                while (priceInUSD == null)
                {
                    Console.Write("Costs of purchase (in USD) ".PadRight(21) + "> ");
                    priceInUSD = GetInput.GetDecimal(out exit, "x");
                    if (exit) break;
                }
                if (exit) break;

                // The addition of Offices makes this obsolete, but since currency is already set
                // the code won't execute anyway. So no harm is done leaving it here for
                // sentimental reasons.
                while (string.IsNullOrEmpty(currency))
                {
                    Console.Write("Local currency ".PadRight(21) + "> ");
                    currency = GetInput.GetString(out exit, "x");
                    if (exit) break;

                    // Evaluates if currencyConverter knows the currency.
                    // If not, available currencies are presented and currency
                    // is set to null and the loop is continued.
                    if (!string.IsNullOrEmpty(currency))
                    {
                        if (currencyConverter.IsCurrency(currency))
                        {
                            currency = currency.ToUpper();
                        }
                        else
                        {
                            TextManipulation.ColoredText(
                                "Currency not available!\n" +
                                "Available options are:\n" +
                                currencyConverter.DisplayCurrencies() +
                                ".\n",
                                "Red");
                            currency = null;
                            // Not strictly necessary. Just here in casse I'll add code later.
                            continue; 
                        }
                    }
                }
                if (exit) break;

                this.assets.Add(new Asset(type, brand, model, office, purchaseDate, priceInUSD, currency));
            }
        }

        public void AddTestData()
        {
            assets.AddRange( new List<Asset>
                { 
                    new Asset("Laptop", "Dell", "Latitude 4711", "Malmö", DateTime.Parse("2020-05-16"), 1200, "SEK"),
                    new Asset("Laptop", "HP", "Mega 12", "Malmö", DateTime.Parse("2020-08-06"), 1538, "SEK"),
                    new Asset("Phone", "Apple", "iPhone X", "Copenhagen", DateTime.Parse("2018-05-16"), 900, "DKK"),
                    new Asset("Toilet paper", "Lambi", "Xtra soft", "Copenhagen", DateTime.Parse("2020-02-16"), 12, "DKK"),
                    new Asset("Laptop", "Apple", "MacBook Air", "Hamburg", DateTime.Parse("2019-11-21"), 2520, "EUR"),
                    new Asset("Computer", "Unknown", "Unknown", "Hamburg", DateTime.Parse("2022-03-15"), 7500, "EUR"),
                    new Asset("Phone", "Google", "Pixel", "Malmö", DateTime.Parse("2021-02-18"), 1500, "SEK"),
                    new Asset("Laptop", "Dell", "Latitude 0815", "Hamburg", DateTime.Parse("2020-03-01"), 750, "EUR"),
                    new Asset("Laptop", "Dell", "Latitude 0815", "Copenhagen", DateTime.Parse("2019-10-16"), 750, "DKK")
                }
            );
        }

        public void Display()
        {
            // Setting variables to calculate whether or nor a line
            // shall be written in a warning color later.
            int lifeTimeYears = 3;
            int yellowWarningMonths = 6;
            int redWarningMonths = 3;

            Console.Clear();
            // Catches nulled asset lists to avoid later references to null.
            if (this.assets == null)
            {
                TextManipulation.ColoredText("No assets in list!\n" +
                    "Populate list before calling an output.\n",
                    ConsoleColor.Yellow
                );
                return;
            }

            // Populates localPriceToday field with output from currencyConverter.
            // Doing it right at the time of displaying the data makes sure the
            // amounts really are the latest. In a setting with proper data storage
            // this should be done when fetching the data and after an appropriate
            // amount of time whent by. Otherwise displaying the data would be
            // necessary to update it, which is highly problematic. But for now it's fine.
            foreach (Asset asset in this.assets)
            {
                asset.LocalPriceToday = currencyConverter.ConvertFromTo("USD", asset.Currency, asset.PriceInUSD);
            }

            List<Asset> outputList =
                (
                from asset in this.assets
                orderby asset.Office, asset.PurchaseDate
                select asset
                ).ToList();



            // Create a nice heading for the output table.
            TextManipulation.MakeHeading(
                "Type".PadRight(19) + "| " +
                "Brand".PadRight(10) + "| " +
                "Model".PadRight(14) + "| " +
                "Office".PadRight(10) + "| " +
                "Purchase Date".PadRight(15) + "| " +
                "Price in USD".PadRight(15) + "| " +
                "Currency".PadRight(15) + "| " +
                "Local price today".PadRight(19)
            );

            foreach (Asset asset in outputList)
            {
                bool warningColor = false;

                if (asset.PurchaseDate.Value.AddYears(lifeTimeYears) < DateTime.Today.AddMonths(redWarningMonths))
                {
                    warningColor = true;
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (asset.PurchaseDate.Value.AddYears(lifeTimeYears) < DateTime.Today.AddMonths(yellowWarningMonths))
                {
                    warningColor = true;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }

                    Console.WriteLine(
                    asset.Type.ToString().PadRight(19) + "| " +
                    asset.Brand.ToString().PadRight(10) + "| " +
                    asset.Model.ToString().PadRight(14) + "| " +
                    asset.Office.ToString().PadRight(10) + "| " +
                    // Since DateTime? is nullable the .Value member has to be called to allow
                    // for formating the string with .ToString. This will, however, throw
                    // an exception if .PriceInUSD is null. In this case this problem is handled
                    // by the .GetDateTime() method of LittleHelpers. Empty or null can't pass it.
                    asset.PurchaseDate.Value.ToString("yyyy-MM-dd").PadRight(15) + "| " +
                    asset.PriceInUSD.ToString().PadRight(15) + "| " +
                    asset.Currency.ToString().PadRight(15) + "| " +
                    asset.LocalPriceToday.ToString().PadLeft (19)
                );

                if (warningColor)
                {
                    Console.ResetColor();
                    warningColor = false;
                }
            }
        }
    }
}
