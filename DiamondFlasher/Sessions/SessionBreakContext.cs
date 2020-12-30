using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DiamondFlasher
{
    class SessionBreakContext:ExitableContext
    {
        public ReadOnlyCollection<Flashcard> WordsLeft { get; }
        public ReadOnlyCollection<Flashcard> WordsOnStart { get; }
        private readonly Action exit;

        public SessionBreakContext(MainContext mainContext, ReadOnlyCollection<Flashcard> wordsLeft, ReadOnlyCollection<Flashcard> wordsOnStart, Action exit)
            :base(mainContext)
        {

            this.exit = exit ?? throw new ArgumentNullException(nameof(exit));
            WordsLeft = wordsLeft;
            WordsOnStart = wordsOnStart;
        
        }
        public override void Exit()
        {
            exit();
            Context.ExitContext<SessionBreakContext>();
        }
    }
}
