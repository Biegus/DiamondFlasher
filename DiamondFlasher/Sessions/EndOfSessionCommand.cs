using System;
using System.Collections.Generic;
using System.Text;

namespace DiamondFlasher.Sessions
{
    static class EndOfSessionCommand
    {
        [ContextMethod(typeof(EndOfSessionContext),"unload_perfects",MinimumArguments =0)]        
        public static void UnloadPerfects(string[] args,EndOfSessionContext context)
        {    
            bool any = false ;
            foreach(var item in GetCertain(context,true))
            {

                any = true;
                context.MainContext.WordsBank.Remove(item);
            }
            if (!any)
                ConsoleHelper.WriteLine("There are no flashcards matching",Styles.Information);
           
        }
        [ContextMethod(typeof(EndOfSessionContext), "next_without_perfects", MinimumArguments = 0)]
        public static void NextWithoutPerfects(string[] ags, EndOfSessionContext context)
        {
            Context.ExitContext<EndOfSessionContext>();
            SessionManager.RunSession(context.MainContext, GetCertain(context,false));
          
        }
        private static IEnumerable<Flashcard> GetCertain(EndOfSessionContext context,bool perfects)
        {
            foreach (var item in context.Results)
            {
                if ((item.Mistakes.Count < 1 && item.Completed > 0)==perfects)
                {
                    yield return item.Flashcard;
                }

            }

        }
    }
}
