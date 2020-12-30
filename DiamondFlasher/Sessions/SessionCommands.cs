using System;
using System.Collections.Generic;
using System.Text;

namespace DiamondFlasher
{
    static class SessionCommands
    {
       
        [ContextMethod(typeof(SessionBreakContext), "back")]
        public static void CloseCommands(string[] arguments, SessionBreakContext context)
        {
            Context.ExitContext<SessionBreakContext>();
        }

    }
}
