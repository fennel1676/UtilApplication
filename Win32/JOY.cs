using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Win32
{
    public class JOYERR
    {
        /* joystick error return values */
        public static uint NOERROR          { get { return 0; } }            /* no error */
        public static uint PARMS            { get { return 160 + 5; } }      /* bad parameters */
        public static uint NOCANDO          { get { return 160 + 6; } }      /* request not completed */
        public static uint UNPLUGGED        { get { return 160 + 7; } }      /* joystick is unplugged */
    }

    public class JOY
    {
        /* constants used with JOYINFO and JOYINFOEX structures and MM_JOY* messages */
        public static uint BUTTON1          { get { return 0x0001; } }
        public static uint BUTTON2          { get { return 0x0002; } }
        public static uint BUTTON3          { get { return 0x0004; } }
        public static uint BUTTON4          { get { return 0x0008; } }
        public static uint BUTTON1CHG       { get { return 0x0100; } }
        public static uint BUTTON2CHG       { get { return 0x0200; } }
        public static uint BUTTON3CHG       { get { return 0x0400; } }
        public static uint BUTTON4CHG       { get { return 0x0800; } }

        /* constants used with JOYINFOEX */
        public static uint BUTTON5          { get { return 0x00000010; } }
        public static uint BUTTON6          { get { return 0x00000020; } }
        public static uint BUTTON7          { get { return 0x00000040; } }
        public static uint BUTTON8          { get { return 0x00000080; } }
        public static uint BUTTON9          { get { return 0x00000100; } }
        public static uint BUTTON10         { get { return 0x00000200; } }
        public static uint BUTTON11         { get { return 0x00000400; } }
        public static uint BUTTON12         { get { return 0x00000800; } }
        public static uint BUTTON13         { get { return 0x00001000; } }
        public static uint BUTTON14         { get { return 0x00002000; } }
        public static uint BUTTON15         { get { return 0x00004000; } }
        public static uint BUTTON16         { get { return 0x00008000; } }
        public static uint BUTTON17         { get { return 0x00010000; } }
        public static uint BUTTON18         { get { return 0x00020000; } }
        public static uint BUTTON19         { get { return 0x00040000; } }
        public static uint BUTTON20         { get { return 0x00080000; } }
        public static uint BUTTON21         { get { return 0x00100000; } }
        public static uint BUTTON22         { get { return 0x00200000; } }
        public static uint BUTTON23         { get { return 0x00400000; } }
        public static uint BUTTON24         { get { return 0x00800000; } }
        public static uint BUTTON25         { get { return 0x01000000; } }
        public static uint BUTTON26         { get { return 0x02000000; } }
        public static uint BUTTON27         { get { return 0x04000000; } }
        public static uint BUTTON28         { get { return 0x08000000; } }
        public static uint BUTTON29         { get { return 0x10000000; } }
        public static uint BUTTON30         { get { return 0x20000000; } }
        public static uint BUTTON31         { get { return 0x40000000; } }
        public static uint BUTTON32         { get { return 0x80000000; } }

        /* constants used with JOYINFOEX structure */
        public static int POVCENTERED       { get { return -1; } }
        public static uint POVFORWARD       { get { return 0; } }
        public static uint POVRIGHT         { get { return 9000; } }
        public static uint POVBACKWARD      { get { return 18000; } }
        public static uint POVLEFT          { get { return 27000; } }

        public static uint RETURNX          { get { return 0x00000001; } }
        public static uint RETURNY          { get { return 0x00000002; } }
        public static uint RETURNZ          { get { return 0x00000004; } }
        public static uint RETURNR          { get { return 0x00000008; } }
        public static uint RETURNU          { get { return 0x00000010; } }     /* axis 5 */
        public static uint RETURNV          { get { return 0x00000020; } }     /* axis 6 */
        public static uint RETURNPOV        { get { return 0x00000040; } }
        public static uint RETURNBUTTONS    { get { return 0x00000080; } }
        public static uint RETURNRAWDATA    { get { return 0x00000100; } }
        public static uint RETURNPOVCTS     { get { return 0x00000200; } }
        public static uint RETURNCENTERED   { get { return 0x00000400; } }
        public static uint USEDEADZONE      { get { return 0x00000800; } }
        public static uint RETURNALL        { get { return (RETURNX | RETURNY | RETURNZ | RETURNR | RETURNU | RETURNV | RETURNPOV | RETURNBUTTONS); } }
        public static uint CAL_READALWAYS   { get { return 0x00010000; } }
        public static uint CAL_READXYONLY   { get { return 0x00020000; } }
        public static uint CAL_READ3        { get { return 0x00040000; } }
        public static uint CAL_READ4        { get { return 0x00080000; } }
        public static uint CAL_READXONLY    { get { return 0x00100000; } }
        public static uint CAL_READYONLY    { get { return 0x00200000; } }
        public static uint CAL_READ5        { get { return 0x00400000; } }
        public static uint CAL_READ6        { get { return 0x00800000; } }
        public static uint CAL_READZONLY    { get { return 0x01000000; } }
        public static uint CAL_READRONLY    { get { return 0x02000000; } }
        public static uint CAL_READUONLY    { get { return 0x04000000; } }
        public static uint CAL_READVONLY    { get { return 0x08000000; } }

        /* joystick ID constants */
        public static uint JOYSTICKID1      { get { return 0; } }
        public static uint JOYSTICKID2      { get { return 1; } }

        /* joystick driver capabilites */
        public static uint CAPS_HASZ        { get { return 0x0001; } }
        public static uint CAPS_HASR        { get { return 0x0002; } }
        public static uint CAPS_HASU        { get { return 0x0004; } }
        public static uint CAPS_HASV        { get { return 0x0008; } }
        public static uint CAPS_HASPOV      { get { return 0x0010; } }
        public static uint CAPS_POV4DIR     { get { return 0x0020; } }
        public static uint CAPS_POVCTS      { get { return 0x0040; } }
    }
}
