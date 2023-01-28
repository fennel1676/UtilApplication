using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Win32
{
    public class PRF
    {
        public static long CHECKVISIBLE         { get { return 0x00000001L; } }
        public static long NONCLIENT            { get { return 0x00000002L; } }
        public static long CLIENT               { get { return 0x00000004L; } }
        public static long ERASEBKGND           { get { return 0x00000008L; } }
        public static long CHILDREN             { get { return 0x00000010L; } }
        public static long OWNED                { get { return 0x00000020L; } }
    }
}
