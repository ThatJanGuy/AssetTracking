using AssetTracking;
using LittleHelpers;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using static LittleHelpers.GetInput;
using static LittleHelpers.TextManipulation;

// Setup
// Instantiate assetList, check if it has data and offer filling in test data if not.
// Update the local prices if any data is found. The displayList is used to keep assets
// identifyable after the Display() has been called.
// All CRUD operations shall happen in the assetList while data shall be collected
// in the main program.
AssetList assetList = new();
List<Asset> displayList = new();
int assetLifeTimeInYears = 3;
int assetLifeMonthsLeftForYellowWarning = 6;
int assetLifeMonthsLeftForRedWarning = 3;
// This should turn into ... something else that is more elegant
// and easier to edit. But for now it will do the job.
Dictionary<string, string> offices = new Dictionary<string, string>
        {
            {"Malmö", "SEK"},
            {"Copenhagen", "DKK"},
            {"Hamburg", "EUR"}
        };

Console.Clear();

Console.WriteLine("Checking Database...");
int assetsInDb = CheckForDb();
Console.WriteLine(assetsInDb + (assetsInDb == 1 ? " asset " : " assets ") + "found.");
if (assetsInDb == 0)
{
    while (true)
    {
        Console.WriteLine("Would you like to add some test data? (y/n)");
        Console.Write(" > ");
        char input = Console.ReadKey().KeyChar;
        Console.WriteLine();

        if (input != 'y' && input != 'n')
        {
            ColoredText(
                "\nInvalid input. Only 'y' or 'n' allowed.\n",
                "Red"
                );
            continue;
        }

        if (input == 'n') break;

        AddTestData();
        break;
    }
}
Console.WriteLine(assetList.UpdateLocalPriceToday());
Console.WriteLine("Press ANY KEY to continue.");
Console.ReadKey();

MainMenu();



void MainMenu()
{
    bool exitMenu = false;
    while (!exitMenu)
    {
        Console.Clear();

        Display(assetLifeTimeInYears, assetLifeMonthsLeftForYellowWarning, assetLifeMonthsLeftForRedWarning);

        MakeHeading("Main Menu", '=');
        Console.Write(
                    $"(1) to ADD an asset.\n" +
                    $"(2) to EDIT an asset.\n" +
                    $"(3) to DELETE an asset.\n" +
                    $"(4) for STATISTICS.\n" +
                    $"(x) to SAFE AND EXIT.\n" +
                    $"Enter choice > ");

        int menuChoice = Console.ReadKey().KeyChar;

        bool cancel = false;

        switch (menuChoice)
        {
            case '1':
                CreateAsset();
                break;

            case '2':
                int? positionInDisplayList = null;
                while (positionInDisplayList == null)
                {
                    Console.Write("\nEnter the ID of the asset you wish to edit ('c' to cancel) > ");
                    positionInDisplayList = GetInt(out cancel, "c", 0, displayList.Count - 1);
                    if (cancel) break;
                }
                if (cancel) break;

                Edit(displayList[positionInDisplayList.Value]);
                break;
            
            case '3':
                int? idToDelete = null;
                while (idToDelete == null)
                {
                    Console.Write("\nEnter the ID of the asset you wish to delete ('c' to cancel) > ");
                    idToDelete = GetInt(out cancel, "c", 0, displayList.Count - 1);
                    if (cancel) break;
                }
                if (cancel) break;

                Console.Clear();
                Display(0, 0, 0, true, displayList[idToDelete.Value]);
                Console.WriteLine("\nAre you sure you want to delete this asset? (y/n)");
                Console.Write(" > ");
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

                if (assetList.RemoveAsset(displayList[idToDelete.Value]))
                {
                    Console.WriteLine("Deletion successful");
                }
                
                break;
                
            case '4':
                Statistics();
                break;

            case 'x':
                exitMenu = true;
                break;

            default:
                ColoredText("\nPlease enter a valid option.\n(Press any key to continue)\n", ConsoleColor.Red);
                Console.ReadKey();
                Display(assetLifeTimeInYears, assetLifeMonthsLeftForYellowWarning, assetLifeMonthsLeftForRedWarning);
                break;
        }
    }
}

void AddTestData()
{
    bool success = assetList.AddAssets(new List<Asset>
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

    if (success) Console.WriteLine("Test data successfully added to database.");
    else Console.WriteLine("Test data injection unsuccessful. Check error messages for further information.");
}

// Checks if a database and entries are created. Returns 
// number of entries that have been found in the DB.
int CheckForDb()
{
    DbSession context = new();

    //context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
    return context.Assets.ToList().Count();
}

void CreateAsset()
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
        MakeHeading("Create a new asset ('x' to exit)");
        Console.WriteLine("\nPlease enter the following information:");

        while (string.IsNullOrEmpty(type))
        {
            Console.Write("Type of asset ".PadRight(29) + "> ");
            type = GetString(out exit, "x");
            if (exit) break;
        }
        if (exit) break;

        while (string.IsNullOrEmpty(brand))
        {
            Console.Write("Brand ".PadRight(29) + "> ");
            brand = GetString(out exit, "x");
            if (exit) break;
        }
        if (exit) break;

        while (string.IsNullOrEmpty(model))
        {
            Console.Write("Model ".PadRight(29) + "> ");
            model = GetString(out exit, "x");
            if (exit) break;
        }
        if (exit) break;

        while (string.IsNullOrEmpty(office))
        {
            Console.Write("Used at which office ".PadRight(29) + "> ");
            office = GetString(out exit, "x");
            if (exit) break;

            if (!offices.ContainsKey(ToTitle(office)))
            {
                List<string>? outputList = new();
                string? outputString = null;

                outputList = offices.Keys.ToList();
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

                ColoredText(
                    "Office not available!\n" +
                    "Available options are:\n" +
                    outputString,
                    "Red"
                    );
                office = null;
            }
            else
            {
                office = ToTitle(office);
                currency = offices[office];
            }
        }
        if (exit) break;

        while (purchaseDate == null)
        {
            Console.Write("Date of purchase ".PadRight(29) + "> ");
            purchaseDate = GetDateTime(out exit, "x");
            if (exit) break;
        }
        if (exit) break;

        while (priceInUSD == null)
        {
            Console.Write("Costs of purchase (in USD) ".PadRight(29) + "> ");
            priceInUSD = GetDecimal(out exit, "x");
            if (exit) break;
        }
        if (exit) break;

        if (!assetList.AddAsset(new Asset(type, brand, model, office, purchaseDate, priceInUSD, currency)))
        {
            ColoredText(
                "\nAsset could not be added to list.\n" +
                "Check error messages for further information.",
                ConsoleColor.Red
                );
        }
    }
}

void Display(int assetLifeTimeInYears = 0, int assetLifeMonthsLeftForYellowWarning = 0, int assetLifeMonthsLeftForRedWarning = 0, bool displayToEdit = false, Asset? editedAsset = null)
{
    assetList.UpdateLocalPriceToday();
    displayList = assetList.GetAssetList();

    if (displayToEdit)
    {
        // Create a nice heading for the output table that contains
        // numbers for the menu.
        Console.WriteLine(
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
        MakeHeading(
            " --".PadRight(6) + "| " +
            " -> 1".PadRight(19) + "| " +
            " -> 2".PadRight(10) + "| " +
            " -> 3".PadRight(14) + "| " +
            " -> 4".PadRight(10) + "| " +
            " -> 5".PadRight(15) + "| " +
            " -> 6".PadRight(15) + "| " +
            " --".PadRight(15) + "| " +
            " --".PadRight(19)
        );
        outputEntry(editedAsset, 0);
    }
    else
    {
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

        for (int i = 0; i < displayList.Count; i++)
        {
            outputEntry(displayList[i], i);
        }
    }

    void outputEntry(Asset asset, int id)
    {
        bool warningColor = false;

        if (asset.PurchaseDate.Value.AddYears(assetLifeTimeInYears) < DateTime.Today.AddMonths(assetLifeMonthsLeftForRedWarning))
        {
            warningColor = true;
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else if (asset.PurchaseDate.Value.AddYears(assetLifeTimeInYears) < DateTime.Today.AddMonths(assetLifeMonthsLeftForYellowWarning))
        {
            warningColor = true;
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        Console.WriteLine(
            id.ToString().PadRight(6) + "| " +
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
            asset.LocalPriceToday.ToString().PadLeft(19)
        );

        if (warningColor)
        {
            Console.ResetColor();
            warningColor = false;
        }
    }
}

void Edit(Asset asset)
{
    bool keepEditing = true;

    while (keepEditing)
    {
        Console.Clear();
        Display(0,0,0, true, asset);

        Console.Write("\nWhich value would you like to edit? (1-6, 'x' to exit) > ");
        char selection = Console.ReadKey().KeyChar;

        switch (selection)
        {
            case '1':
                bool exit = false;
                string? newType = null;

                Console.Write($"\nOld type: {asset.Type}\n");
                while (newType == null)
                {
                    Console.Write($"New type: ");
                    newType = GetInput.GetString(out exit, "x");
                    if (exit) break;
                }
                if (exit) break;
                asset.Type = newType;
                assetList.UpdateAsset(asset);
                break;

            case '2':
                exit = false;
                string? newBrand = null;

                Console.Write($"\nOld brand: {asset.Brand}\n");
                while (newBrand == null)
                {
                    Console.Write($"New brand: ");
                    newBrand = GetInput.GetString(out exit, "x");
                    if (exit) break;
                }
                if (exit) break;
                asset.Brand = newBrand;
                assetList.UpdateAsset(asset);
                break;

            case '3':
                exit = false;
                string? newModel = null;

                Console.Write($"\nOld model: {asset.Model}\n");
                while (newModel == null)
                {
                    Console.Write($"New model: ");
                    newModel = GetInput.GetString(out exit, "x");
                    if (exit) break;
                }
                if (exit) break;
                asset.Model = newModel;
                assetList.UpdateAsset(asset);
                break;

            case '4':
                exit = false;
                string? newOffice = null;

                Console.Write($"\nOld office: {asset.Office}");
                while (newOffice == null)
                {
                    Console.Write("\nNew office: ");
                    newOffice = GetString(out exit, "x");
                    if (exit) break;

                    if (!offices.ContainsKey(ToTitle(newOffice)))
                    {
                        List<string>? outputList = new();
                        string? outputString = null;

                        outputList = offices.Keys.ToList();
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

                        ColoredText(
                            "Office not available!\n" +
                            "Available options are:\n" +
                            outputString,
                            "Red"
                            );
                        newOffice = null;
                    }
                    else
                    {
                        asset.Office = ToTitle(newOffice);
                        asset.Currency = offices[newOffice];
                        assetList.UpdateAsset(asset);
                        break;
                    }
                }
                break;

            case '5':
                exit = false;
                DateTime? newDate = null;

                Console.Write($"\nOld purchase date: {asset.PurchaseDate}\n");
                while (newDate == null)
                {
                    Console.Write($"New purchase date: ");
                    newDate = GetInput.GetDateTime(out exit, "x");
                    if (exit) break;
                }
                if (exit) break;
                asset.PurchaseDate = newDate;
                assetList.UpdateAsset(asset);
                break;
            
            case '6':
                exit = false;
                decimal? newPrice = null;

                Console.Write($"\nOld price in US$: {asset.PriceInUSD}\n");
                while (newPrice == null)
                {
                    Console.Write($"New price in US$: ");
                    newPrice = GetInput.GetDecimal(out exit, "x");
                    if (exit) break;
                }
                if (exit) break;
                asset.PriceInUSD = newPrice;
                assetList.UpdateAsset(asset);
                break;
            
            case 'x':
                keepEditing = false;
                break;

            default:
                TextManipulation.ColoredText("Please select a valid option.\n(Press any key to continue)\n", ConsoleColor.Red);
                Console.ReadKey();
                break;
        }
    }

}

void Statistics()
{
    decimal? totalValue = null;
    int? numberOfAssets = null;
    int? assetsInMalmo = null;

    using( DbSession context = new())
    {
        totalValue = context.Assets.Select(a => a.PriceInUSD).Sum();
        numberOfAssets = context.Assets.Count();
        assetsInMalmo = context.Assets.Where(a => a.Office == "Malmö").Count();
    }

    Console.Clear();
    Display(assetLifeTimeInYears, assetLifeMonthsLeftForYellowWarning, assetLifeMonthsLeftForRedWarning);

    MakeHeading("\nStatistical data (examples):");
    Console.WriteLine( $"Total value of tracked assets:     {totalValue} US$");
    Console.WriteLine( $"Total number of trackes assets:    {numberOfAssets}");
    Console.WriteLine( $"Number of trackes assets in Malmö: {assetsInMalmo}");

    Console.WriteLine("\n(Press ANY KEY to CONTINUE.)");
    Console.ReadKey();
}