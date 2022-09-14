using LittleHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking
{
    public class AssetList
    {
        public void AddAsset()
        {
            string? type = null;
            string? brand = null;
            string? model = null;
            string? office = null;
            DateTime? purchaseDate = null;
            decimal? priceInUSD = null;
            string? currency = null;

            while (true)
            {
                bool exit = false;

                Console.Clear();
                TextManipulation.MakeHeading("Create a new asset");
                Console.WriteLine("\nPlease enter the following information:");

                while (string.IsNullOrEmpty(type)) 
                {
                    Console.Write("Type of asset ");
                    type = GetData.GetString(out exit, "x");
                    if (exit) break;
                }
                if (exit) break;

                while (string.IsNullOrEmpty(type)) 
                {
                    Console.Write("Brand ");
                    brand = GetData.GetString(out exit, "x");
                    if (exit) break;
                }
                if (exit) break;

                while (string.IsNullOrEmpty(type)) 
                {
                    Console.Write("Model ");
                    model = GetData.GetString(out exit, "x");
                    if (exit) break;
                }
                if (exit) break;

                while (string.IsNullOrEmpty(type)) 
                {
                    Console.Write("Used at which office ");
                    office = GetData.GetString(out exit, "x");
                    if (exit) break;
                }
                if (exit) break;

                while (string.IsNullOrEmpty(type))
                {
                    Console.Write("Date of purchase ");
                    purchaseDate = GetData.GetDateTime(out exit, "x");
                    if (exit) break;
                }
                if (exit) break;

                while (string.IsNullOrEmpty(type))
                {
                    Console.Write("Costs of purchase (in USD) ");
                    priceInUSD = GetData.GetDecimal(out exit, "x");
                    if (exit) break;
                }
                if (exit) break;

                while (string.IsNullOrEmpty(type))
                {
                    Console.Write("Paid for in which currency ");
                    currency = GetData.GetString(out exit, "x");
                    if (exit) break;
                }
                if (exit) break;

            }
        }
    }
}
