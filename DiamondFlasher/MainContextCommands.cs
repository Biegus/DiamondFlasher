using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using DiamondFlasher;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

using System.CodeDom.Compiler;
#pragma warning disable IDE0060
namespace DiamondFlasher
{


    class MainContextCommands
    {
        private const string LOAD_FILE_MODE_DESCR = "?0->key-value 1->value-key 2->both";

        [ContextMethod(typeof(MainContext), "load_file",
         ArgumentsDescription = new string[]
          {
                "file_name",
                "?seperator",
                LOAD_FILE_MODE_DESCR
          },
          MinimumArguments = 1)]
        public static void LoadFile(string[] arguments, MainContext context)
        {

            string GetArguments(int index, string ifEmpty = null)
            {
                if (arguments.Length <= index)
                {
                    return ifEmpty;
                }
                else return arguments[index];
            }
            int countBefore = context.WordsBank.Flashcards.Count;
            FlashCardLoader.LoadFlags loadMode = GetArguments(2, "0") switch
            {
                "0" => FlashCardLoader.LoadFlags.Default,
                "1" => FlashCardLoader.LoadFlags.Invert,
                "2" => FlashCardLoader.LoadFlags.Both,
                _ => throw new ArgumentException("Uknown value as third argument")
            };
            context.WordsBank.Add(FlashCardLoader.LoadFromFile(GetArguments(0), GetArguments(1, "-"), loadMode));
            ConsoleHelper.WriteLine($"Correctly loaded {context.WordsBank.Flashcards.Count - countBefore } flashcards from {arguments[0]}", Styles.Information);

        }
        [ContextMethod(typeof(MainContext), "load_all_file",
          ArgumentsDescription = new string[]
           {
                "dir_name",
                "?seperator",
                LOAD_FILE_MODE_DESCR,
                "?ext"
           },
           MinimumArguments = 1)]
        public static void LoadAllFile(string[] arguments, MainContext context)
        {
            string ext = arguments.Length < 4 ? ".txt" : arguments[3];
            foreach (var file in
                   (from file in Directory.GetFiles(arguments[0])
                    where System.IO.Path.GetExtension(file) == ext
                    select file))
            {
                string[] args = arguments.Take(3).ToArray();
                args[0] = file;

                LoadFile(args, context);
            }
        }
        [ContextMethod(typeof(MainContext), "load_file_desktop",
          ArgumentsDescription = new string[]
           {
                "file_name",
                "?seperator",
                LOAD_FILE_MODE_DESCR
           },
           MinimumArguments = 1)]
        public static void LoadFileFromDesktop(string[] arguments, MainContext context)
        {
            arguments[0] = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), arguments[0]);
            LoadFile(arguments, context);
        }
        [ContextMethod(typeof(MainContext),"parse_raw",MinimumArguments =0)]
        public static void ParseRaw(string[] arguments, MainContext context)
        {  
            LinkedList<string> list = new LinkedList<string>();
            do
            {
                string line = ConsoleHelper.ReadLine(Styles.Symbols, false);
                if (!line.Equals(string.Empty))
                {
                    list.AddLast(line);
                   
                }   
                else
                    break;

            } while(true);
            var parsed = FlashCardLoader.Parse(list, "-", FlashCardLoader.LoadFlags.Default);
            int lenBef = context.WordsBank.Flashcards.Count;
            context.WordsBank.Add(parsed);
            ConsoleHelper.WriteLine($"Added {context.WordsBank.Flashcards.Count - lenBef} words");

        }
        [ContextMethod(typeof(MainContext), "unload_all")]
        public static void ClearEverything(string[] arguments, MainContext context)
        {
            int wordsBefore = context.WordsBank.Flashcards.Count;
            context.WordsBank.Clear();
            ConsoleHelper.WriteLine($"Cleared {wordsBefore}", Styles.Information);
        }
        [ContextMethod(typeof(MainContext), "unload_range",
             MinimumArguments = 1,
            ArgumentsDescription = new[]
            {
                "range"
            }
            )]
        public static void UnloadRange(string[] arguments, MainContext context)
        {
            var p = ParseRange(context, arguments[0]);
            foreach (var flashcard in context.WordsBank.GetRange(p.min, p.max).ToArray())
            {
                context.WordsBank.Remove(flashcard, true);
            }
            ConsoleHelper.WriteLine("The indicated words have been deleted", Styles.Information);
        }
        [ContextMethod(typeof(MainContext), "unload_regex")]
        public static void ClearRegex(string[] arguments, MainContext context)
        {
            bool any = false;
            foreach (var item in context.WordsBank.Flashcards.ToArray())
            {
                any = true;
                if (Regex.Match(item.Question, arguments[0]).Success)
                {

                    context.WordsBank.Remove(item);
                }

            }
            if (!any)
            {
                ConsoleHelper.WriteLine("Nothing has matched", Styles.Information);
            }
            ;
        }

        [ContextMethod(typeof(MainContext), "add_manually",
           ArgumentsDescription = new string[]
           {
                "key",
                "value"
           },
           MinimumArguments = 2
           )]
        public static void AddFlashcard(string[] arguments, MainContext context)
        {
            Flashcard newF;
            context.WordsBank.Add(newF = new Flashcard(arguments[0], arguments[1]));
            ConsoleHelper.WriteLine($"Added manually {newF}", Styles.Information);
        }
        [ContextMethod(typeof(MainContext), "start_session",
            ArgumentsDescription = new[]
            {
                "?range",
            })]
        public static void StartSession(string[] arguments, MainContext context)
        {
            if (context.WordsBank.Flashcards.Count == 0)
            {
                ConsoleHelper.WriteLine("You haven't added any words yet", Styles.Information);
                return;
            }

            IEnumerable<Flashcard> words;
            if (arguments.Length < 1)
                words = context.WordsBank.Flashcards;
            else
            {

                var p = ParseRange(context, arguments[0]);
                words = context.WordsBank.GetRange(p.min, p.max);

            }

            SessionManager.RunSession(context, words);
        }
        private static (int min, int max) ParseRange(MainContext context, string text)
        {
            var splited = text.Split();
            int min = int.Parse(splited[0]) - 1;
            int max = splited.Length > 1 ? int.Parse(splited[1]) : context.WordsBank.Flashcards.Count;
            return (min, max);
        }

        [ContextMethod(typeof(MainContext), "invert",MinimumArguments =0)]
        public static void Invert(string[] arguments, MainContext context)
        {
            for(int x=0;x<context.WordsBank.Flashcards.Count;x++)
            {
                var original = context.WordsBank.Flashcards[x];
                context.WordsBank.Change(x, new Flashcard(original.Answer, original.Question));
            }
            ConsoleHelper.WriteLine("The keys and values got inverted",Styles.Information);
        }
        [ContextMethod(typeof(MainContext), "set", MinimumArguments = 3,
            ArgumentsDescription =new[]
            {
                "id",
                "question",
                "?answer"
             })]
        public static void Set(string[] arguments, MainContext context)
        {
            int id = int.Parse(arguments[0]) - 1;
            string question = arguments[1];
            string answer = (arguments.Length > 1) ? arguments[2] : context.WordsBank.Flashcards[id].Answer;
            context.WordsBank.Change(id, new Flashcard(question, answer));
        }
    
        [ContextMethod(typeof(MainContext),"show_all")]
        public static void ShowAll(string[] arguments, MainContext context)
        {
            if(context.WordsBank.Flashcards.Any()==false)
            {
                ConsoleHelper.WriteLine("There's no any flashcard yet",Styles.Information);
                return;
            }
            ConsoleHelper.WriteLine("All flashcards:", Styles.Information);
            int i = 1;
            ConsoleHelper.Write(context.WordsBank.Flashcards.Aggregate(new StringBuilder(), (b, w) => b.Append($"{i++}:{w}\n")).ToString(),Styles.Symbols);
        }
       
        [ContextMethod(typeof(MainContext), "clear")]
        public static void Clean(string[] arguments, MainContext context)
        {
            Console.Clear();
            ConsoleHelper.WriteLine("Cleaned", Styles.Information);
        }
         [ContextMethod(typeof(MainContext), "format_q_regex")]
        public static void FormatQuestionRegex(string[] arguments, MainContext context)
        {
            bool any = false;
            foreach(var item in context.WordsBank.Flashcards.ToArray())
            {
                string question = Regex.Replace(item.Question, arguments[0], arguments[1]);
                if(!question.Equals(item.Question))
                {
                    any = true;
                    context.WordsBank.Remove(item,true);
                    Flashcard nw = new Flashcard(question, item.Answer);
                    context.WordsBank.Add(nw);
                    ConsoleHelper.WriteLine($"\"{item.Question}\" was changed to \"{question}\"",Styles.Information);
                }
            }
            if (!any)
                ConsoleHelper.WriteLine("Nothing matched", Styles.Information);
        }

        [ContextMethod(typeof(Context), "help")]

        public static void ShowCommands(string[] arguments, Context context)

        {
         
            ConsoleHelper.WriteLine($"Available commands (<{Styles.Answer}>{context.GetType().Name}</color>):", Styles.Information);
           

            var commands = Context.GetCommands(Context.GetUpperContexType());
            string text = commands.Select(item =>
            {
                string argsDescr = item.Value.attribute.ArgumentsDescription.Aggregate(new StringBuilder(), (b, val) => b.Append($" [{ val}]")).ToString();
                return $"<{Styles.Command}>{item.Key}</color><{Styles.Symbols}>{argsDescr}</color> ";

            }).Aggregate(new StringBuilder(), (b, v) => b.Append($"{v}\n"))
              .ToString();
            ConsoleHelper.Write(text);
        }
       


    }

}
