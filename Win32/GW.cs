using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Win32
{
    public class GW
    {
        public static int HWNDFIRST         { get { return 0; } }
        public static int HWNDLAST          { get { return 1; } }
        public static int HWNDNEXT          { get { return 2; } }
        public static int HWNDPREV          { get { return 3; } }
        public static int OWNER             { get { return 4; } }
        public static int CHILD             { get { return 5; } }
        public static int ENABLEDPOPUP      { get { return 6; } }
        public static int MAX               { get { return 6; } }
    }
}
