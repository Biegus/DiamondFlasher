using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DiamondFlasher
{
    class FlashCardLoader
    {
        [Flags]
        public enum LoadFlags
        {
            None=0,
            Default=1<<0,
            Invert=1<<2,
            Both=Default|Invert,
        }

        public static IEnumerable<Flashcard> Parse(IEnumerable<string> text, string seperator, LoadFlags mode)
        {
            Regex question = new Regex($".*?(?={seperator})");
            Regex answer = new Regex("(?<=-).*");
            foreach (var item in text)
            {

                if (string.IsNullOrEmpty(item.Trim()))
                    yield break;
                string a = question.Match(item).Value.Trim();
                string b = answer.Match(item).Value.Trim();
                if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b))
                    continue;
                if (mode.HasFlag(LoadFlags.Default))
                    yield return new Flashcard(a, b);
                if (mode.HasFlag(LoadFlags.Invert))
                    yield return new Flashcard(b, a);

            }
        }
        public static IEnumerable<Flashcard> LoadFromFile(string path,string seperator,LoadFlags mode=LoadFlags.Default)
        {
            return Parse(File.ReadLines(path), seperator, mode);
        }
    }
}
