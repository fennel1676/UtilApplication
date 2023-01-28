using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Win32
{
    public class PAGE
    {
        public static uint NOACCESS             { get { return 0x01; } }
        public static uint READONLY             { get { return 0x02; } }
        public static uint READWRITE            { get { return 0x04; } }
        public static uint WRITECOPY            { get { return 0x08; } }
    }
}
