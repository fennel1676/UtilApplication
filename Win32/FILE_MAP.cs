using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Win32
{
    public class FILE_MAP
    {
        public static uint WRITE { get { return 0x0002; } }
        public static uint READ { get { return 0x0004; } }
        public static uint ALL_ACCESS { get { return 0x000F001F; } }
        public static uint EXECUTE { get { return 0x0020; } }
        public static uint COPY { get { return 0x00000001; } }
        public static uint RESERVE { get { return 0x80000000; } }
    }
}

