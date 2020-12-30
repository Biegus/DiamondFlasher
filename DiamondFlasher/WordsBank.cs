using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DiamondFlasher.Helpers;

namespace DiamondFlasher
{
    class WordsBank
    {

        private readonly List<Flashcard> flashcards = new List<Flashcard>();
        public ReadOnlyCollection<Flashcard> Flashcards { get; }
       
        public event EventHandler<Flashcard> OnWordAdded = delegate { };
        public WordsBank()
        {
            Flashcards = new ReadOnlyCollection<Flashcard>(flashcards);
        }
        public void Change(int id,Flashcard nw)
        {
            flashcards[id] = nw;
        }
        public IEnumerable<Flashcard> GetRange(int min, int max)
        {
            return flashcards.Skip(min).Take(max-min);
        }
        public void Add(IEnumerable<Flashcard> words)
        {
            if (words is null)
            {
                throw new ArgumentNullException(nameof(words));
            }
            foreach (var item in words)
            {

                Add(item);              
            }     
        }
        
        public void Add(Flashcard flashcard)
        {
            this.flashcards.Add(flashcard);
            this.OnWordAdded(this, flashcard);
        }
        public void Remove(Flashcard first, params Flashcard[] other)
        {
            Remove(first);
         
            if (other != null)
                Remove(other);
        }
        public void Clear()
        {
            this.flashcards.Clear();
        }
        public void Remove(IEnumerable<Flashcard> words)
        {
            if (words is null)
            {
                throw new ArgumentNullException(nameof(words));
            }
            foreach (var item in words)
            {
                Remove(item);
            }
               
        }
        public void Remove(Flashcard word,bool quiet=false)
        {
            if (!quiet)
                ConsoleHelper.WriteLine($"{word} has been removed");
            this.flashcards.Remove(word);
        }
        
    }
}
