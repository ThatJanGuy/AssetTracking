using System.Globalization;

namespace LittleHelpers
{
    public static class GetInput
    {
        // GetString
        // ---------
        // Accepts: An out bool + (optional) a string (default = "exit")
        // Does:    - Read a string from the console and check if it's empty or null.
        //          - Set the out bool to true when the read string matches
        //          the parameter string. Not case sensitive.
        // Returns: A string or null when invalid.
        //          A boolean to the out variable.
        public static string? GetString(out bool exit)
        {
            return GetString(out exit, "exit");
        }

        public static string? GetString(out bool exit, string exitTerm)
        {
            string? input = Console.ReadLine() ?? "";

            if (exitTerm.Trim().Split(' ').Length > 1)
            {
                TextManipulation.ColoredText(
                    "ProgrammerNeedsAHeadCheck_Error: exitTerm needs to be a single word.\n",
                    ConsoleColor.Yellow
                    );
            }
            else if (input?.Trim().ToLower() == exitTerm.Trim().ToLower())
            {
                exit = true;
                return null;
            }

            if (String.IsNullOrEmpty(input))
            {
                TextManipulation.ColoredText(
                    "Empty values are not accepted.\n",
                    ConsoleColor.Red
                    );
                exit = false;
                return null;
            }
            else
            {
                exit = false;
                return input;
            }
        }




        // GetInt
        // ------
        // Accepts: An out bool +
        //          (optional) a string (default = "exit") +
        //          (optional) two ints (default = 0).
        // Does:    - Read a string from the console and check it for validity to be
        //          parsed to an int.
        //          - Set the out bool to true when the read string matches
        //          the parameter string. Not case sensitive.
        //          - Reject the input if it is not between the int parameters.
        // Returns: An int or null when invalid.
        //          A boolean to the out variable.
        public static int? GetInt(out bool exit)
        {
            return GetInt(out exit, "exit", 0, 0);
        }

        public static int? GetInt(out bool exit, string exitTerm)
        {
            return GetInt(out exit, exitTerm, 0, 0);
        }

        public static int? GetInt(out bool exit, string exitTerm, int min, int max)
        {
            string? input = Console.ReadLine();
            int? result = null;

            if (exitTerm.Trim().Split(' ').Length > 1)
            {
                TextManipulation.ColoredText(
                    "ProgrammerNeedsAHeadCheck_Error: exitTerm needs to a a single word.\n",
                    ConsoleColor.Yellow
                    );
            }
            else if (input?.Trim().ToLower() == exitTerm.Trim().ToLower())
            {
                exit = true;
                return null;
            }

            if (min < int.MinValue)
            {
                TextManipulation.ColoredText(
                    "ProgrammerNeedsAHeadCheck_Error: min can't be lower than int.MinValue, which is " + int.MinValue + ".\n",
                    ConsoleColor.Yellow
                    );
            }

            if (max > int.MaxValue)
            {
                TextManipulation.ColoredText(
                    "ProgrammerNeedsAHeadCheck_Error: max can't be lower than int.MaxValue, which is " + int.MaxValue + ".\n",
                    ConsoleColor.Yellow
                    );
            }

            if (min > max)
            {
                // This needs to become an exception error
                TextManipulation.ColoredText(
                    "ProgrammerNeedsAHeadCheck_Error: 'min' needs to be smaller than 'max'\n",
                    ConsoleColor.Yellow
                    );
                exit = false;
                return null;
            }

            if (String.IsNullOrEmpty(input))
            {
                TextManipulation.ColoredText(
                    "Empty values are not accepted.\n",
                    ConsoleColor.Red
                    );
                exit = false;
                return null;
            }

            if (!int.TryParse(input, out int value))
            {
                TextManipulation.ColoredText(
                    "Only integers are accepted.\n",
                    ConsoleColor.Red
                    );
                exit = false;
                return null;
            }

            if ((value < min || value > max)! & (min != max))
            {
                TextManipulation.ColoredText(
                    $"Value must be between {min} and {max} inclusive.\n",
                    ConsoleColor.Red
                    );
                exit = false;
                return null;
            }
            else
            {
                exit = false;
                result = value;
                return result;
            }

        }



        // GetDecimal
        // ------
        // Accepts: An out bool +
        //          (optional) a string (default = "exit") +
        //          (optional) two decimnals (default = 0).
        // Does:    - Read a string from the console and check it for validity to be
        //          parsed to a decimal.
        //          - Set the out bool to true when the read string matches
        //          the parameter string. Not case sensitive.
        //          - Reject the input if it is not between the int parameters.
        // Returns: A string or null when invalid.
        //          A boolean to the out variable.
        public static decimal? GetDecimal(out bool exit)
        {
            return GetDecimal(out exit, "exit", 0, 0);
        }

        public static decimal? GetDecimal(out bool exit, string exitTerm)
        {
            return GetDecimal(out exit, exitTerm, 0, 0);
        }

        public static decimal? GetDecimal(out bool exit, string exitTerm, decimal min, decimal max)
        {
            string? input = Console.ReadLine();
            decimal? result = null;

            if (exitTerm.Trim().Split(' ').Length > 1)
            {
                TextManipulation.ColoredText(
                    "ProgrammerNeedsAHeadCheck_Error: exitTerm needs to a a single word.\n",
                    ConsoleColor.Yellow
                    );
            }
            else if (input?.Trim().ToLower() == exitTerm.Trim().ToLower())
            {
                exit = true;
                return null;
            }

            if (min < decimal.MinValue)
            {
                TextManipulation.ColoredText(
                    "ProgrammerNeedsAHeadCheck_Error: min can't be lower than decimal.MinValue, which is " + decimal.MinValue + ".\n",
                    ConsoleColor.Yellow
                    );
            }

            if (max > decimal.MaxValue)
            {
                TextManipulation.ColoredText(
                    "ProgrammerNeedsAHeadCheck_Error: max can't be lower than decimal.MaxValue, which is " + decimal.MaxValue + ".\n",
                    ConsoleColor.Yellow
                    );
            }

            if (min > max)
            {
                // This needs to become an exception error
                TextManipulation.ColoredText(
                    "ProgrammerNeedsAHeadCheck_Error: 'min' needs to be smaller than 'max'\n",
                    ConsoleColor.Yellow
                    );
                exit = false;
                return null;
            }

            if (String.IsNullOrEmpty(input))
            {
                TextManipulation.ColoredText(
                    "Empty values are not accepted.\n",
                    ConsoleColor.Red
                    );
                exit = false;
                return null;
            }
            else if (!decimal.TryParse(input, out decimal value))
            {
                TextManipulation.ColoredText(
                    "Only numbers are accepted!\n" +
                    "Decimals with a decimal comma are ok.\n",
                    ConsoleColor.Red
                    );
                exit = false;
                return null;
            }
            else if ((result < min || result > max)! & (min == max))
            {
                TextManipulation.ColoredText(
                    $"Value must be between {min} and {max} inclusive.\n",
                    ConsoleColor.Red
                    );
                exit = false;
                return null;
            }
            else
            {
                exit = false;
                result = value;
                return result;
            }

        }


        // GetDateTime
        // ------
        // Accepts: An out bool +
        //          (optional) a string (default = "exit")
        // Does:    - Read a string from the console and check it for validity to be
        //          parsed to a DateTime.
        //          - Set the out bool to true when the read string matches
        //          the parameter string. Not case sensitive.
        // Returns: A DateTime or null when invalid.
        //          A boolean to the out variable.
        public static DateTime? GetDateTime(out bool exit)
        {
            return GetDateTime(out exit, "exit");
        }
        public static DateTime? GetDateTime(out bool exit, string exitTerm)
        {
            string? input = Console.ReadLine();
            DateTime? result = null;

            if (exitTerm.Trim().Split(' ').Length > 1)
            {
                TextManipulation.ColoredText(
                    "ProgrammerNeedsAHeadCheck_Error: exitTerm needs to be a single word.\n",
                    ConsoleColor.Yellow
                    );
            }
            else if (input?.Trim().ToLower() == exitTerm.Trim().ToLower())
            {
                exit = true;
                return null;
            }

            if (String.IsNullOrEmpty(input))
            {
                TextManipulation.ColoredText(
                    "Empty values are not accepted.\n",
                    ConsoleColor.Red
                    );
                exit = false;
                return null;
            }
            else if (!DateTime.TryParse(input, out DateTime value))
            {
                TextManipulation.ColoredText(
                    "No legal input!\n" +
                    $"(Format: {CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern})\n",
                    ConsoleColor.Red
                    );
                exit = false;
                return null;
            }
            else
            {
                exit = false;
                result = value;
                return result;
            }
        }
    }
}