using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Win32
{
    public class GWL
    {
        public static int WNDPROC       { get { return -4; } }
        public static int HINSTANCE     { get { return -6; } }
        public static int HWNDPARENT    { get { return -8; } }
        public static int STYLE         { get { return -16; } }
        public static int EXSTYLE       { get { return -20; } }
        public static int USERDATA      { get { return -21; } }
        public static int ID            { get { return -12; } }
    }
}
