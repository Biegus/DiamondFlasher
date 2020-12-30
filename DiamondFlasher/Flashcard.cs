using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DiamondFlasher
{
    readonly struct Flashcard:IComparable<Flashcard>
    {
        private static long LastId = long.MinValue;
        public string Question { get; }
        public string Answer { get; }
        public long Id { get; }

        public Flashcard(string question, string answer)
        {
            Id = LastId++;
            Question = question;
            Answer = answer;
        }

        public class QuestionEqualer : IEqualityComparer<Flashcard>
        {
            public bool Equals([AllowNull] Flashcard x, [AllowNull] Flashcard y)
            {
                return String.Equals(x.Question, y.Question);
            }

            public int GetHashCode([DisallowNull] Flashcard obj)
            {
                return obj.Question?.GetHashCode() ?? 0;
            }
        }
       
        public override string ToString()
        {
            return $"\"<{Styles.Question}>{Question}</color> - <{Styles.Answer}>{Answer}</color>\"";
        }

        public int CompareTo([AllowNull] Flashcard other)
        {
            return this.Id.CompareTo(other.Id);
        }
    }
  
}
