namespace LittleHelpers
{
    public static class TextManipulation
    {

        // ColoredText
        // -----------
        // Accepts: A string + (a string that spells a color | a colour of type ConsoleColor)
        // Does:    Writes the string in the color via Console.Write. No line breaks added.
        // Returns: Nothing
        public static void ColoredText(string text, string color)
        {
            if (Enum.TryParse<ConsoleColor>(color, out ConsoleColor setColor))
            {
                ColoredText(text, setColor);
            }
            else
            {
                ColoredText(
                    "ProgrammerNeedsAHeadCheck_Error: Color not found in ConsoleColor Enum.",
                    ConsoleColor.Yellow);
            }
        }

        public static void ColoredText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }


        // MakeHeading
        // -----------
        // Accepts: A string + (optional) a char (default = '-')
        // Does:    Write the string in one line and an equally long string of
        //          the given char in the next. Uses Console.WriteLine();
        // Returns: Nothing 
        public static void MakeHeading(string heading)
        {
            MakeHeading(heading, '-'); 
        }

        public static void MakeHeading(string heading, char lineChar)
        {
            Console.WriteLine(heading);
            Console.WriteLine(string.Concat(Enumerable.Repeat<char>(lineChar, heading.Length)));
        }


        /// <summary>
        /// Accepts a string and returns it with the first letter of every word capitalised.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns></returns>

        public static string ToTitle(string input)
        {
            input = input.Trim().ToLower();
            List<string> words = new();
            string output = "";

            foreach(string word in input.Split(' '))
            {
                if (word.Trim().Length == 0)
                {
                    continue;
                }
                else if (word.Trim().Length == 1)
                {
                    words.Add(word.ToUpper());
                }
                else
                {
                    words.Add(word[0].ToString().ToUpper() + word.Substring(1));
                }
            }

            foreach (string word in words)
            {
                output += word + " ";
            }

            return output.Trim();
        }
    }
}