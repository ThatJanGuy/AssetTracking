using AssetTracking;
using LittleHelpers;
using System.Globalization;
using System.Text.Json;
using static LittleHelpers.GetInput;
using static LittleHelpers.TextManipulation;

CurrencyConverter currencyConverter = new CurrencyConverter();
AssetList assetList = new AssetList();
CultureInfo culture = CultureInfo.InvariantCulture;

// Add a bunch of test data to make function checking simpler.
Console.Clear();
while (true)
{
    Console.WriteLine("Would you like to add some test data? (y/n)");
    char input = Console.ReadKey().KeyChar;

    if ( input != 'y' && input != 'n')
    {
        ColoredText(
            "Invalid input. Only 'y' or 'n' allowed.",
            "Red"
            );
        continue;
    }

    if (input == 'n') break;

    assetList.AddTestData();
    break;
}

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
            assetList.AddAsset();
            break;

        case '2':
            int? idToEdit = null;
            while (idToEdit == null)
            {
                Console.Write("\nEnter the ID of the asset you wish to edit ('c' to cancel) > ");
                idToEdit = GetInt(out cancel, "c", 0, orderedList.Count - 1);
                if (cancel) break;
            }
            if (cancel) assetList.Display();

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
            if (cancel) Display();

            DeleteTransaction(orderedList[(int)idToDelete]);
            break;

        case '4':

            break;
        case 'x':

            break;
        default:
            ColoredText("\nPlease enter a valid option.\n(Press any key to continue)\n", ConsoleColor.Red);
            Console.ReadKey();
            assetList.Display();
            break;
    }
}

