using AssetTracking;
using LittleHelpers;
using System.Globalization;
using System.Text.Json;
using static LittleHelpers.GetInput;
using static LittleHelpers.TextManipulation;

// Setup
// Instantiate assetList and pass it a DbContext and a CurrencyConverter
// All the magic shall happen in the assetList while data shall be collected
// in the main program.
AssetList assetList = new AssetList(new DbSession(), new CurrencyConverter());
int assetLifeTimeInYears = 3;
int assetLifeMonthsLeftForYellowWarning = 6;
int assetLifeMonthsLeftForRedWarning = 3;

Console.Clear();
assetList.LoadAssets();
assetList.Display(assetLifeTimeInYears, assetLifeMonthsLeftForYellowWarning, assetLifeMonthsLeftForRedWarning);



void MainMenu() {
    while (true)
    {
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
                /*
            case '2':
                int? idToEdit = null;
                while (idToEdit == null)
                {
                    Console.Write("\nEnter the ID of the asset you wish to edit ('c' to cancel) > ");
                    idToEdit = GetInt(out cancel, "c", 0, orderedList.Count - 1);
                    if (cancel) break;
                }
                if (cancel) assetList.Display(assetLifeTimeInYears, assetLifeMonthsLeftForYellowWarning, assetLifeMonthsLeftForRedWarning);

                Guid targetTransactionGuid = orderedList[idToEdit.Value].Id;

                Edit(targetTransactionGuid);
                break;
            
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
                assetList.Display(assetLifeTimeInYears, assetLifeMonthsLeftForYellowWarning, assetLifeMonthsLeftForRedWarning);
                break;
        }
    }
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
                "Check error messages for information why.",
                ConsoleColor.Red
                );
        }
    }
}