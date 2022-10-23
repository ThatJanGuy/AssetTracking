using AssetTracking;
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

Console.Clear();

Console.WriteLine("Checking Dabtabase...");
int assetsInDb = CheckForDb();
Console.WriteLine(assetsInDb + assetsInDb == 1 ? "asset" : "assets" + "found.");
if (assetsInDb == 0)
{
    while (true)
    {
        Console.WriteLine("Would you like to add some test data? (y/n)");
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

        AddTestData();
        break;
    }
}

Console.WriteLine(assetList.UpdateLocalPriceToday());

Console.WriteLine("Press ANY KEY to continue.");

MainMenu();



void MainMenu()
{
    while (true)
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
                int? idToEdit = null;
                while (idToEdit == null)
                {
                    Console.Write("\nEnter the ID of the asset you wish to edit ('c' to cancel) > ");
                    idToEdit = GetInt(out cancel, "c", 0, displayList.Count - 1);
                    if (cancel) break;
                }
                if (cancel) break;

                Edit(displayList[idToEdit.Value].id);
                break;
            /*
            case '3':
                int? idToDelete = null;
                while (idToDelete == null)
                {
                    Console.Write("\nEnter the ID of the asset you wish to delete ('c' to cancel) > ");
                    idToDelete = GetInt(out cancel, "c", 0, orderedList.Count - 1);
                    if (cancel) break;
                }
                if (cancel) Display(assetLifeTimeInYears, assetLifeMonthsLeftForYellowWarning, assetLifeMonthsLeftForRedWarning);

                DeleteTransaction(orderedList[(int)idToDelete]);
                break;

            case '4':

                break;
            case 'x':

                break;*/
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

    context.Database.EnsureCreated();
    return context.Assets.ToList().Count();
}

void CreateAsset()
{
    while (true)
    {
        bool exit = false;
        Dictionary<string, string> offices = new Dictionary<string, string>
        {
            {"Malmö", "SEK"},
            {"Copenhagen", "DKK"},
            {"Hamburg", "EUR"}
        };

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
            Console.Write("Type of asset ".PadRight(21) + "> ");
            type = GetString(out exit, "x");
            if (exit) break;
        }
        if (exit) break;

        while (string.IsNullOrEmpty(brand))
        {
            Console.Write("Brand ".PadRight(21) + "> ");
            brand = GetString(out exit, "x");
            if (exit) break;
        }
        if (exit) break;

        while (string.IsNullOrEmpty(model))
        {
            Console.Write("Model ".PadRight(21) + "> ");
            model = GetString(out exit, "x");
            if (exit) break;
        }
        if (exit) break;

        while (string.IsNullOrEmpty(office))
        {
            Console.Write("Used at which office ".PadRight(21) + "> ");
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
            Console.Write("Date of purchase ".PadRight(21) + "> ");
            purchaseDate = GetDateTime(out exit, "x");
            if (exit) break;
        }
        if (exit) break;

        while (priceInUSD == null)
        {
            Console.Write("Costs of purchase (in USD) ".PadRight(21) + "> ");
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

void Display(int assetLifeTimeInYears = 0, int assetLifeMonthsLeftForYellowWarning = 0, int assetLifeMonthsLeftForRedWarning = 0, bool displayToEdit = false, int idToDisplayToEdit = 0)
{
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
            " -> 7".PadRight(15) + "| " +
            " --".PadRight(19)
        );
        outputEntry(idToDisplayToEdit);
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
            outputEntry(i);
        }
    }

    void outputEntry(int id)
    {
        bool warningColor = false;

        if (displayList[id].PurchaseDate.Value.AddYears(assetLifeTimeInYears) < DateTime.Today.AddMonths(assetLifeMonthsLeftForRedWarning))
        {
            warningColor = true;
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else if (displayList[id].PurchaseDate.Value.AddYears(assetLifeTimeInYears) < DateTime.Today.AddMonths(assetLifeMonthsLeftForYellowWarning))
        {
            warningColor = true;
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        Console.WriteLine(
            id.ToString().PadRight(6) + "| " +
            displayList[id].Type.ToString().PadRight(19) + "| " +
            displayList[id].Brand.ToString().PadRight(10) + "| " +
            displayList[id].Model.ToString().PadRight(14) + "| " +
            displayList[id].Office.ToString().PadRight(10) + "| " +
            // Since DateTime? is nullable the .Value member has to be called to allow
            // for formating the string with .ToString. This will, however, throw
            // an exception if .PriceInUSD is null. In this case this problem is handled
            // by the .GetDateTime() method of LittleHelpers. Empty or null can't pass it.
            displayList[id].PurchaseDate.Value.ToString("yyyy-MM-dd").PadRight(15) + "| " +
            displayList[id].PriceInUSD.ToString().PadRight(15) + "| " +
            displayList[id].Currency.ToString().PadRight(15) + "| " +
            displayList[id].LocalPriceToday.ToString().PadLeft(19)
        );

        if (warningColor)
        {
            Console.ResetColor();
            warningColor = false;
        }
    }

}