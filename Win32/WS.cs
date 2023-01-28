using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Win32
{
    public class WS
    {
        public static long EX_NOACTIVATE { get { return 0x08000000L; } }
        public static long EX_TOOLWINDOW { get { return 0x08L; } }
        public static long EX_APPWINDOW { get { return 0x40000L; } }

    }
}
