using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DiamondFlasher
{
    public class ConsoleHelper
    {
        public static void WriteLine(string text,ConsoleColor color)
        {
            Write($"{text}\n", color);
        }
        public static void WriteLine(string text)
        {
            Write($"{text}\n");
        }
        public static void Write(string text, ConsoleColor color)
        {
            using ColorChanger changer = new ColorChanger(color);
                Write(text);
        }  
        public static string ReadLine(ConsoleColor color = Styles.UserInput,bool useSymbol=true)
        {
            if (useSymbol)
                ConsoleHelper.Write(">>", Styles.Symbols);
            using (new ColorChanger(color))
                return Console.ReadLine();
        }
        public static void Write (string text)
        {
            Regex tagExpression = new Regex(@"<\w.*?>.*?</color>",RegexOptions.Singleline);
            var mathes = tagExpression.Matches(text);
          
            if (mathes.Count == 0)
            {
                Console.Write(text);
                return;
            }
            int i = 0;
            StringBuilder bulider = new StringBuilder();
            Match goodMatch;
            while (i<text.Length)
            {
                if ((goodMatch=mathes.FirstOrDefault(item =>item.Index==i)) ==null)
                {
                    bulider.Append(text[i]);
                    i++;
                }
                else
                {

                    Console.Write(bulider);
                    bulider.Clear();
                    Regex valueExpression = new Regex("(?<=^<.*?>).*?(?=</color>)", RegexOptions.Singleline);
                    Regex colorExpression = new Regex("(?<=^<).*?(?=>)", RegexOptions.Singleline);
                    using (new ColorChanger((ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorExpression.Match(goodMatch.Value).Value)))
                        Console.Write(valueExpression.Match(goodMatch.Value).Value);
                    i += goodMatch.Value.Length;
                }
            }
            Console.Write(bulider);



        }

      
    }
}
