using System;
using System.Collections.Generic;
using System.Text;

namespace DiamondFlasher
{
    abstract class ExitableContext:Context
    {
        
        public MainContext MainContext { get; }
        public ExitableContext( MainContext main)
        {
         
            this.MainContext = main?? throw new ArgumentNullException(nameof(main));
        }
        public abstract void Exit();
       
    }
    static class ExitableCommands
    {
        [ContextMethod(typeof(ExitableContext), "exit_context")]
        public static void ExitSession(string[] arguments, ExitableContext context)
        {
            context.Exit();
        }
    }
}
