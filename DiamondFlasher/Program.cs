using System;
using System.Text.RegularExpressions;

namespace DiamondFlasher
{
    class Program
    {
        static void Main(string[] args)
        {

            WordsBank bank = new WordsBank();
            MainContext main = new MainContext(bank);
            ConsoleHelper.WriteLine($"All started, type <{Styles.Command}>\"help\"</color> to see commands in the current context", Styles.Information);
            Context.LoadContext<MainContext>(main);
        }

    }
}
