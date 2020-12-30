using System;
using System.Collections.Generic;
using System.Text;

namespace DiamondFlasher
{
    class ColorChanger:IDisposable
    {
        private ConsoleColor colorBefore;
        public void Dispose()
        {
            Console.ForegroundColor = colorBefore;
        }
        public ColorChanger(ConsoleColor newColor)
        {
            colorBefore = Console.ForegroundColor;
            Console.ForegroundColor = newColor;
        }

       
    }
}
