using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Win32
{
    public class EM
    {
        public static uint GETSEL               { get { return 0x00B0; } }
        public static uint LINEINDEX            { get { return 0x00BB; } }
        public static uint LINEFROMCHAR         { get { return 0x00C9; } }
        public static uint POSFROMCHAR          { get { return 0x00D6; } }
    }
}
