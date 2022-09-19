using AssetTracking;
using LittleHelpers;
using System.Globalization;

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
        TextManipulation.ColoredText(
            "Invalid input. Only 'y' or 'n' allowed.",
            "Red"
            );
        continue;
    }

    if (input == 'n') break;

    assetList.AddTestData();
    break;
}

assetList.AddAsset();

assetList.Display();
