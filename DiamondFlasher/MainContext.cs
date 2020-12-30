using System;
using System.Collections.Generic;
using System.Text;

namespace DiamondFlasher
{
    class MainContext : Context
    {
        public WordsBank WordsBank { get; }
        public MainContext(WordsBank wordsBank)
        {
            WordsBank = wordsBank;
        }
    }
}
