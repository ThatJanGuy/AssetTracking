using LittleHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static LittleHelpers.GetInput;
using static LittleHelpers.TextManipulation;


namespace AssetTracking
{
    internal class AssetList
    {
        private List<Asset> assets;
        private DbContext _context;
        private CurrencyConverter currencyConverter;
        

        public AssetList(DbContext context, CurrencyConverter currencyConverter)
        {
            this._context = context;
            this.currencyConverter = currencyConverter;
            this.assets = new List<Asset>();
        }

        public bool AddAsset(Asset asset)
        // Gets passed an Asset, checks for critical fields and either rejects the
        // Asset or adds it to the _context.
        {
            if (!string.IsNullOrEmpty(asset.Currency))
            {
                ColoredText(
                    "Currency must be set!\n" +
                    "Available options are:\n" +
                    currencyConverter.DisplayCurrencies() +
                    "\n", ConsoleColor.Red);
                return false;
            }

            if (currencyConverter.IsCurrency(asset.Currency))
            {
                asset.Currency = asset.Currency.ToUpper();
            }
            else
            {
                ColoredText(
                    "Requested currency not available!\n" +
                    "Available options are:\n" +
                    currencyConverter.DisplayCurrencies() +
                    ".\n",
                    "Red");
                return false;
            }

            _context.Add(asset);
            _context.SaveChanges();
            return true;
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

        public void Display(int assetLifeTimeInYears, int assetLifeMonthsLeftForYellowWarning, int assetLifeMonthsLeftForRedWarning)
        {
            Console.Clear();

            this.assets.OrderBy(asset => asset.Office)
                    .ThenBy(asset => asset.PurchaseDate)
                    .ToList();

            // Create a nice heading for the output table.
            MakeHeading(
                "(ID)".PadRight(6) + "| " +
                "Type".PadRight(19) + "| " +
                "Brand".PadRight(10) + "| " +
                "Model".PadRight(14) + "| " +
                "Office".PadRight(10) + "| " +
                "Purchase Date".PadRight(15) + "| " +
                "Price in USD".PadRight(15) + "| " +
                "Currency".PadRight(15) + "| " +
                "Local price today".PadRight(19)
            );


            for ( int i = 0; i < this.assets.Count; i++)
            {
                bool warningColor = false;

                if (this.assets[i].PurchaseDate.Value.AddYears(assetLifeTimeInYears) < DateTime.Today.AddMonths(assetLifeMonthsLeftForRedWarning))
                {
                    warningColor = true;
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (this.assets[i].PurchaseDate.Value.AddYears(assetLifeTimeInYears) < DateTime.Today.AddMonths(assetLifeMonthsLeftForYellowWarning))
                {
                    warningColor = true;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }

                    Console.WriteLine(
                        i.ToString().PadRight(6) + "| " +
                        this.assets[i].Type.ToString().PadRight(19) + "| " +
                        this.assets[i].Brand.ToString().PadRight(10) + "| " +
                        this.assets[i].Model.ToString().PadRight(14) + "| " +
                        this.assets[i].Office.ToString().PadRight(10) + "| " +
                        // Since DateTime? is nullable the .Value member has to be called to allow
                        // for formating the string with .ToString. This will, however, throw
                        // an exception if .PriceInUSD is null. In this case this problem is handled
                        // by the .GetDateTime() method of LittleHelpers. Empty or null can't pass it.
                        this.assets[i].PurchaseDate.Value.ToString("yyyy-MM-dd").PadRight(15) + "| " +
                        this.assets[i].PriceInUSD.ToString().PadRight(15) + "| " +
                        this.assets[i].Currency.ToString().PadRight(15) + "| " +
                        this.assets[i].LocalPriceToday.ToString().PadLeft (19)
                    );

                if (warningColor)
                {
                    Console.ResetColor();
                    warningColor = false;
                }
            }
        }

        public Asset GetAsset(int id)
        {
            return _context.Assets.Where(asset => asset.id == id);
        }

        // Connects to database and loads all contained assets into this.Assets.
        // In case there are no entries in the db the creation of test data is
        // being offered.
        public void LoadAssets()
        {
            Console.WriteLine("Loading assets from database...");

            this._context.Database.EnsureCreated();
            this.assets = this._context.Assets.ToList();
            _context.
            
            Console.WriteLine( $"{this.assets.Count} assets loaded.");

            if (this.assets.Count <= 0)
            {
                while (true)
                {
                    Console.WriteLine("Would you like to add some test data? (y/n)");
                    char input = Console.ReadKey().KeyChar;

                    if (input != 'y' && input != 'n')
                    {
                        ColoredText(
                            "Invalid input. Only 'y' or 'n' allowed.",
                            "Red"
                            );
                        continue;
                    }

                    if (input == 'n') break;

                    AddTestData();
                    break;
                }
            }

            GetLocalPriceToday(ref this.assets);
        }

        // Populates the LocalPriceToday fields of the passed in list of assets.
        // By having it as its own method it can be used on the assetList but also
        // temporary lists for sorting, statistics, etc. 
        private void GetLocalPriceToday(ref List<Asset> inputList)
        {
            foreach (Asset asset in inputList)
            {
                asset.LocalPriceToday = this.currencyConverter.ConvertFromTo("USD", asset.Currency, asset.PriceInUSD);
            }
        }
    }
}
