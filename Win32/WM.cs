using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Win32
{
    public class WM
    {
        public static UInt32 PAINT              { get { return 0x000F; } }
        public static UInt32 CLOSE              { get { return 0x0010; } }        
        public static UInt32 INPUT              { get { return 0x00FF; } }
        public static UInt32 KEYFIRST           { get { return 0x0100; } }
        public static UInt32 KEYBOARD_LL        { get { return 0x0013; } }
        public static UInt32 KEYDOWN            { get { return 0x0100; } }
        public static UInt32 KEYUP              { get { return 0x0101; } }
        public static UInt32 CHAR               { get { return 0x0102; } }
        public static UInt32 DEADCHAR           { get { return 0x0103; } }
        public static UInt32 SYSKEYDOWN         { get { return 0x0104; } }
        public static UInt32 SYSKEYUP           { get { return 0x0105; } }
        public static UInt32 SYSCHAR            { get { return 0x0106; } }
        public static UInt32 SYSDEADCHAR        { get { return 0x0107; } }
        public static UInt32 HOTKEY             { get { return 0x0312; } }
        public static UInt32 MOUSEMOVE          { get { return 0x0200; } }
        public static UInt32 LBUTTONDOWN        { get { return 0x0201; } }
        public static UInt32 LBUTTONUP          { get { return 0x0202; } }
        public static UInt32 RBUTTONDOWN        { get { return 0x0204; } }
        public static UInt32 LBUTTONDBLCLK      { get { return 0x0203; } }
        public static UInt32 MOUSELEAVE         { get { return 0x02A3; } }
        public static UInt32 ERASEBKGND         { get { return 0x0014; } }
        public static UInt32 PRINT              { get { return 0x0317; } }
        public static UInt32 HSCROLL            { get { return 0x0114; } }
        public static UInt32 VSCROLL            { get { return 0x0115; } }
        public static UInt32 PRINTCLIENT        { get { return 0x0318; } }

    }
}
