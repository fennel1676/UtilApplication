using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AKI.Win32.API
{
    public class Winmm
    {
        private const int MAXPNAMELEN = 32;

        public struct joycapsaStruct
        {
            public short wMid;               /* manufacturer ID */
            public short wPid;               /* product ID */

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szPname;           /* product name (NULL terminated string) */

            public uint wXmin;               /* minimum x position value */
            public uint wXmax;               /* maximum x position value */
            public uint wYmin;               /* minimum y position value */
            public uint wYmax;               /* maximum y position value */
            public uint wZmin;               /* minimum z position value */
            public uint wZmax;               /* maximum z position value */
            public uint wNumButtons;         /* number of buttons */
            public uint wPeriodMin;          /* minimum message period when captured */
            public uint wPeriodMax;          /* maximum message period when captured */

            public uint wRmin;               /* minimum r position value */
            public uint wRmax;               /* maximum r position value */
            public uint wUmin;               /* minimum u (5th axis) position value */
            public uint wUmax;               /* maximum u (5th axis) position value */
            public uint wVmin;               /* minimum v (6th axis) position value */
            public uint wVmax;               /* maximum v (6th axis) position value */
            public uint wCaps;               /* joystick capabilites */
            public uint wMaxAxes;            /* maximum number of axes supported */
            public uint wNumAxes;            /* number of axes in use */
            public uint wMaxButtons;         /* maximum number of buttons supported */

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szRegKey;          /* registry key */

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szOEMVxD;          /* OEM VxD in use */
        }

        public struct joyinfoStruct
        {
            public uint wXpos;                 /* x position */
            public uint wYpos;                 /* y position */
            public uint wZpos;                 /* z position */
            public uint wButtons;              /* button states */
        }

        public struct joyinfoexStruct
        {
            public UInt32 dwSize;                /* size of structure */
            public UInt32 dwFlags;               /* flags to indicate what to return */
            public UInt32 dwXpos;                /* x position */
            public UInt32 dwYpos;                /* y position */
            public UInt32 dwZpos;                /* z position */
            public UInt32 dwRpos;                /* rudder/4th axis position */
            public UInt32 dwUpos;                /* 5th axis position */
            public UInt32 dwVpos;                /* 6th axis position */
            public UInt32 dwButtons;             /* button states */
            public UInt32 dwButtonNumber;        /* current button number pressed */
            public UInt32 dwPOV;                 /* point of view state */
            public UInt32 dwReserved1;           /* reserved for communication between winmm & driver */
            public UInt32 dwReserved2;           /* reserved for future expansion */
        }

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        public static extern int joyConfigChanged(ulong dwFlags);

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        public static extern int joyGetDevCaps(IntPtr uJoyID, ref joycapsaStruct pjc, uint cbjc);

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        public static extern uint joyGetNumDevs();

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        public static extern int joyGetPos(uint uJoyID, ref joyinfoStruct pji);

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        public static extern int joyGetPosEx(uint uJoyID, ref joyinfoexStruct pji);

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        public static extern int joyGetThreshold(uint uJoyID, ref uint puThreshold);

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        public static extern int joyReleaseCapture(uint uJoyID);

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        public static extern int joySetCapture(IntPtr hwnd, uint uJoyID, uint uPeriod, int fChanged);

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        public static extern int joySetThreshold(uint uJoyID, uint uThreshold);
    }
}
