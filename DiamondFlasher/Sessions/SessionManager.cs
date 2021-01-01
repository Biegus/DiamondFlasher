using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiamondFlasher
{
    class SessionManager
    {

        private readonly static Random random = new Random();
        private enum ContextResult
        {
            None,
            Exit,
            Back
        }
        private static IEnumerable<ReadOnlyFlashcardResult> RunInnerSession(MainContext context, IEnumerable<Flashcard> words)
        {

            Flashcard[] onStart = words.ToArray();
            Dictionary<Flashcard, FlashcardResult> results = new Dictionary<Flashcard, FlashcardResult>(onStart.Length);
            List<Flashcard> toLearn = onStart.ToList();
            

            ConsoleHelper.WriteLine($"-----Session----\n<{Styles.Information}>Type</color><{Styles.Command}> _</color><{Styles.Information}> to get access to all commands</color>\n" +
                $"<{Styles.Information}>Type</color><{Styles.Command}>_y</color> <{Styles.Information}>if you're sure you know the answer for sure, and you don't need to type it </color>\n" +
                $"<{Styles.Information}>Type</color><{Styles.Command}>_n</color> <{Styles.Information}>if you're sure you don't know the answer</color> ");
            IEnumerable<ReadOnlyFlashcardResult> Prepare()
            {
                Console.WriteLine("-----End of the Session----");
                return results.Select(item => new ReadOnlyFlashcardResult(item.Value));
            }
            while (toLearn.Count > 0)
            {
                ContextResult CommandsCheck(string text)
                {
                    if (text == "_")
                    {
                        bool exit = false;
                        ConsoleHelper.WriteLine("Type \"help to see commands\"", Styles.Information);
                        Context.LoadContext<SessionBreakContext>(new SessionBreakContext(context, toLearn.AsReadOnly(), Array.AsReadOnly(onStart), exit: () => exit = true));
                        if (exit)
                        {
                           
                            return ContextResult.Exit;
                        }

                        return ContextResult.Back;
                    }
                    return ContextResult.None;
                }
                bool canGo;
                string line;
                var flashcard = RandomHelper.GetRandomElement(random, toLearn);
                do
                {
                    canGo = true;
                    ConsoleHelper.WriteLine($"<{Styles.Symbols}>Question:</color> <{Styles.Question}>{flashcard.Question}</color>");

                    line = ConsoleHelper.ReadLine(Styles.Symbols).Trim();
                    switch (CommandsCheck(line))
                    {
                        case ContextResult.Back: canGo = false; break;
                        case ContextResult.Exit: return Prepare();
                    }
                } while (!canGo);
                bool correctAnswer = false;
                if (line.Equals(flashcard.Answer, StringComparison.InvariantCultureIgnoreCase))
                {
                    ConsoleHelper.WriteLine("You wrote the correct answer", Styles.Information);
                   
                    correctAnswer = true;
                }
                else 
                {
                    ConsoleHelper.WriteLine($"<{Styles.Symbols}>Correct answer: </color><{Styles.Answer }>{flashcard.Answer}</color>");
                    correctAnswer = line.Equals("_y");
                    if (!line.Equals("_y")&&!line.Equals("_n"))
                    {
                       
                        bool wasCorrectOption;
                        bool forceContinue = false;
                        ConsoleHelper.WriteLine($"<{Styles.QuestionToUser}>Was your answer correct?</color>\n" +
                        $"<{Styles.Command}>y:yes\nn:no</color>");
                        do
                        {
                            wasCorrectOption = true;
                            string answer = ConsoleHelper.ReadLine().ToLower();
                            switch (answer)
                            {
                                case "yes":
                                case "ok":
                                case "sure":
                                case "good":
                                case "correct":
                                case "perfect":
                                case "y": correctAnswer = true; break;
                                case "no":
                                case "not correct":
                                case "bad":
                                case "not good":
                                case "n": break;
                                case "_":
                                    if (CommandsCheck(answer) == ContextResult.Exit) return Prepare();
                                    else forceContinue = true;
                                    break;
                                default:
                                    ConsoleHelper.WriteLine("Incorrect response", Styles.BadInformation);
                                    wasCorrectOption = false;
                                    break;

                            }
                        } while (!wasCorrectOption);


                        if (forceContinue)
                            continue;
                    }    
                 
                }
              
                if (!results.ContainsKey(flashcard))
                {
                    var result = new FlashcardResult();
                    result.Card = flashcard;
                    results[flashcard] = result;
                }
                   
                if (correctAnswer)
                {
                    toLearn.Remove(flashcard);
                    results[flashcard].Completed++;
                }
                else
                {
                    results[flashcard].AddMistakes(line);
                }

                Console.WriteLine("###");

            }
         
            return Prepare();
        }
        public static void RunSession(MainContext context,IEnumerable<Flashcard> words=null)
        {
          
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var res= RunInnerSession(context,words??context.WordsBank.Flashcards);
            if (!res.Any())
                return;
           
            
            ConsoleHelper.WriteLine("Summary:", Styles.Information);
            StringBuilder bulider = new StringBuilder();
            foreach (var item in res)
            {
                string failedText = (item.Mistakes.Count > 0) ? $"<{Styles.BadInformation}>Failed:</color> <{Styles.Information}>{item.Mistakes.Count}</color>" : string.Empty;
                string correctText = (item.Completed > 0) ? $"<{Styles.Command}>Correct:</color> <{Styles.Information}>{item.Completed}</color>" : string.Empty;
                bulider.AppendLine($"{item.Flashcard} {failedText} {correctText} ({Math.Round( item.Completed/(double)(item.Mistakes.Count+item.Completed)*100,2)}% correct)");
                    
            }
            ConsoleHelper.Write(bulider.ToString(),Styles.Symbols);

            Context.LoadContext<EndOfSessionContext>(new EndOfSessionContext(context, res));
        }
    }
}
