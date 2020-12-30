using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DiamondFlasher
{
      class FlashcardResult
    {
        private readonly List<string> mistakes = new List<string>();
        public Flashcard Card { get; set; }
        public int Completed { get; set; }

        public ReadOnlyCollection<string> Mistakes => mistakes.AsReadOnly();
       
        public void AddMistakes(string mistake)
        {
           mistakes.Add(mistake);
        }
    }
    struct ReadOnlyFlashcardResult
    {
        private FlashcardResult result;
        public Flashcard Flashcard => result.Card;
        public int Completed => result.Completed;
        public ReadOnlyCollection<string> Mistakes => result.Mistakes;
        public ReadOnlyFlashcardResult(FlashcardResult result)
        {
            this.result = result;
        }
      
    }
    class EndOfSessionContext:ExitableContext
    {
    
        private ReadOnlyFlashcardResult[] results;
        public  ReadOnlyCollection<ReadOnlyFlashcardResult> Results { get; }
      
        public EndOfSessionContext(MainContext main, IEnumerable<ReadOnlyFlashcardResult> results)
            :base(main)
        {
           
            this.results = results.ToArray();
            Results = new ReadOnlyCollection<ReadOnlyFlashcardResult>(this.results);

        }
        public override void Exit()
        {
            SessionBreakContext.ExitContext<EndOfSessionContext>();
        }
    }
}
