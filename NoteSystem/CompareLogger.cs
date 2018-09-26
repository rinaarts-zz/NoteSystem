using System;
using System.Collections.Generic;
using System.Text;

namespace NoteSystem
{
    class CompareLogger
    {
        public static bool Active { get; set; }

        public static void WriteLine(string message)
        {
            if (Active)
            {
                Console.WriteLine(message);
            }
        }
    }
}
