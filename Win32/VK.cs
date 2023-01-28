using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Win32
{
    public class VK
    {
        public static uint LBUTTON               { get { return 0x01; } }
        public static uint RBUTTON               { get { return 0x02; } }
        public static uint CANCEL                { get { return 0x03; } }
        public static uint MBUTTON               { get { return 0x04; } }    /* NOT contiguous with L & RBUTTON */
        public static uint XBUTTON1              { get { return 0x05; } }    /* NOT contiguous with L & RBUTTON */
        public static uint XBUTTON2              { get { return 0x06; } }    /* NOT contiguous with L & RBUTTON */
        public static uint BACK                  { get { return 0x08; } }
        public static uint TAB                   { get { return 0x09; } }
        public static uint CLEAR                 { get { return 0x0C; } }
        public static uint RETURN                { get { return 0x0D; } }
        public static uint SHIFT                 { get { return 0x10; } }
        public static uint CONTROL               { get { return 0x11; } }
        public static uint MENU                  { get { return 0x12; } }
        public static uint PAUSE                 { get { return 0x13; } }
        public static uint CAPITAL               { get { return 0x14; } }
        public static uint KANA                  { get { return 0x15; } }
        public static uint HANGEUL               { get { return 0x15; } }  /* old name - should be here for compatibility */
        public static uint HANGUL                { get { return 0x15; } }
        public static uint JUNJA                 { get { return 0x17; } }
        public static uint FINAL                 { get { return 0x18; } }
        public static uint HANJA                 { get { return 0x19; } }
        public static uint KANJI                 { get { return 0x19; } }
        public static uint ESCAPE                { get { return 0x1B; } }
        public static uint CONVERT               { get { return 0x1C; } }
        public static uint NONCONVERT            { get { return 0x1D; } }
        public static uint ACCEPT                { get { return 0x1E; } }
        public static uint MODECHANGE            { get { return 0x1F; } }
        public static uint SPACE                 { get { return 0x20; } }
        public static uint PRIOR                 { get { return 0x21; } }
        public static uint NEXT                  { get { return 0x22; } }
        public static uint END                   { get { return 0x23; } }
        public static uint HOME                  { get { return 0x24; } }
        public static uint LEFT                  { get { return 0x25; } }
        public static uint UP                    { get { return 0x26; } }
        public static uint RIGHT                 { get { return 0x27; } }
        public static uint DOWN                  { get { return 0x28; } }
        public static uint SELECT                { get { return 0x29; } }
        public static uint PRINT                 { get { return 0x2A; } }
        public static uint EXECUTE               { get { return 0x2B; } }
        public static uint SNAPSHOT              { get { return 0x2C; } }
        public static uint INSERT                { get { return 0x2D; } }
        public static uint DELETE                { get { return 0x2E; } }
        public static uint HELP                  { get { return 0x2F; } }
        public static uint LWIN                  { get { return 0x5B; } }
        public static uint RWIN                  { get { return 0x5C; } }
        public static uint APPS                  { get { return 0x5D; } }
        public static uint SLEEP                 { get { return 0x5F; } }
        public static uint NUMPAD0               { get { return 0x60; } }
        public static uint NUMPAD1               { get { return 0x61; } }
        public static uint NUMPAD2               { get { return 0x62; } }
        public static uint NUMPAD3               { get { return 0x63; } }
        public static uint NUMPAD4               { get { return 0x64; } }
        public static uint NUMPAD5               { get { return 0x65; } }
        public static uint NUMPAD6               { get { return 0x66; } }
        public static uint NUMPAD7               { get { return 0x67; } }
        public static uint NUMPAD8               { get { return 0x68; } }
        public static uint NUMPAD9               { get { return 0x69; } }
        public static uint MULTIPLY              { get { return 0x6A; } }
        public static uint ADD                   { get { return 0x6B; } }
        public static uint SEPARATOR             { get { return 0x6C; } }
        public static uint SUBTRACT              { get { return 0x6D; } }
        public static uint DECIMAL               { get { return 0x6E; } }
        public static uint DIVIDE                { get { return 0x6F; } }
        public static uint F1                    { get { return 0x70; } }
        public static uint F2                    { get { return 0x71; } }
        public static uint F3                    { get { return 0x72; } }
        public static uint F4                    { get { return 0x73; } }
        public static uint F5                    { get { return 0x74; } }
        public static uint F6                    { get { return 0x75; } }
        public static uint F7                    { get { return 0x76; } }
        public static uint F8                    { get { return 0x77; } }
        public static uint F9                    { get { return 0x78; } }
        public static uint F10                   { get { return 0x79; } }
        public static uint F11                   { get { return 0x7A; } }
        public static uint F12                   { get { return 0x7B; } }
        public static uint F13                   { get { return 0x7C; } }
        public static uint F14                   { get { return 0x7D; } }
        public static uint F15                   { get { return 0x7E; } }
        public static uint F16                   { get { return 0x7F; } }
        public static uint F17                   { get { return 0x80; } }
        public static uint F18                   { get { return 0x81; } }
        public static uint F19                   { get { return 0x82; } }
        public static uint F20                   { get { return 0x83; } }
        public static uint F21                   { get { return 0x84; } }
        public static uint F22                   { get { return 0x85; } }
        public static uint F23                   { get { return 0x86; } }
        public static uint F24                   { get { return 0x87; } }
        public static uint NUMLOCK               { get { return 0x90; } }
        public static uint SCROLL                { get { return 0x91; } }
        public static uint OEM_NEC_EQUAL         { get { return 0x92; } }   // '=' key on numpad
        public static uint OEM_FJ_JISHO          { get { return 0x92; } }   // 'Dictionary' key
        public static uint OEM_FJ_MASSHOU        { get { return 0x93; } }   // 'Unregister word' key
        public static uint OEM_FJ_TOUROKU        { get { return 0x94; } }   // 'Register word' key
        public static uint OEM_FJ_LOYA           { get { return 0x95; } }   // 'Left OYAYUBI' key
        public static uint OEM_FJ_ROYA           { get { return 0x96; } }   // 'Right OYAYUBI' key
        public static uint LSHIFT                { get { return 0xA0; } }
        public static uint RSHIFT                { get { return 0xA1; } }
        public static uint LCONTROL              { get { return 0xA2; } }
        public static uint RCONTROL              { get { return 0xA3; } }
        public static uint LMENU                 { get { return 0xA4; } }
        public static uint RMENU                 { get { return 0xA5; } }
        public static uint BROWSER_BACK          { get { return 0xA6; } }
        public static uint BROWSER_FORWARD       { get { return 0xA7; } }
        public static uint BROWSER_REFRESH       { get { return 0xA8; } }
        public static uint BROWSER_STOP          { get { return 0xA9; } }
        public static uint BROWSER_SEARCH        { get { return 0xAA; } }
        public static uint BROWSER_FAVORITES     { get { return 0xAB; } }
        public static uint BROWSER_HOME          { get { return 0xAC; } }
        public static uint VOLUME_MUTE           { get { return 0xAD; } }
        public static uint VOLUME_DOWN           { get { return 0xAE; } }
        public static uint VOLUME_UP             { get { return 0xAF; } }
        public static uint MEDIA_NEXT_TRACK      { get { return 0xB0; } }
        public static uint MEDIA_PREV_TRACK      { get { return 0xB1; } }
        public static uint MEDIA_STOP            { get { return 0xB2; } }
        public static uint MEDIA_PLAY_PAUSE      { get { return 0xB3; } }
        public static uint LAUNCH_MAIL           { get { return 0xB4; } }
        public static uint LAUNCH_MEDIA_SELECT   { get { return 0xB5; } }
        public static uint LAUNCH_APP1           { get { return 0xB6; } }
        public static uint LAUNCH_APP2           { get { return 0xB7; } }
        public static uint OEM_1                 { get { return 0xBA; } }   // ';:' for US
        public static uint OEM_PLUS              { get { return 0xBB; } }   // '+' any country
        public static uint OEM_COMMA             { get { return 0xBC; } }   // ',' any country
        public static uint OEM_MINUS             { get { return 0xBD; } }   // '-' any country
        public static uint OEM_PERIOD            { get { return 0xBE; } }   // '.' any country
        public static uint OEM_2                 { get { return 0xBF; } }   // '/?' for US
        public static uint OEM_3                 { get { return 0xC0; } }   // '`~' for US
        public static uint OEM_4                 { get { return 0xDB; } }  //  '[{' for US
        public static uint OEM_5                 { get { return 0xDC; } }  //  '\|' for US
        public static uint OEM_6                 { get { return 0xDD; } }  //  ']}' for US
        public static uint OEM_7                 { get { return 0xDE; } }  //  ''"' for US
        public static uint OEM_8                 { get { return 0xDF; } }
        public static uint OEM_AX                { get { return 0xE1; } }  //  'AX' key on Japanese AX kbd
        public static uint OEM_102               { get { return 0xE2; } }  //  "<>" or "\|" on RT 102-key kbd.
        public static uint ICO_HELP              { get { return 0xE3; } }  //  Help key on ICO
        public static uint ICO_00                { get { return 0xE4; } }  //  00 key on ICO
        public static uint PROCESSKEY            { get { return 0xE5; } }
        public static uint ICO_CLEAR             { get { return 0xE6; } }
        public static uint PACKET                { get { return 0xE7; } }
        public static uint OEM_RESET             { get { return 0xE9; } }
        public static uint OEM_JUMP              { get { return 0xEA; } }
        public static uint OEM_PA1               { get { return 0xEB; } }
        public static uint OEM_PA2               { get { return 0xEC; } }
        public static uint OEM_PA3               { get { return 0xED; } }
        public static uint OEM_WSCTRL            { get { return 0xEE; } }
        public static uint OEM_CUSEL             { get { return 0xEF; } }
        public static uint OEM_ATTN              { get { return 0xF0; } }
        public static uint OEM_FINISH            { get { return 0xF1; } }
        public static uint OEM_COPY              { get { return 0xF2; } }
        public static uint OEM_AUTO              { get { return 0xF3; } }
        public static uint OEM_ENLW              { get { return 0xF4; } }
        public static uint OEM_BACKTAB           { get { return 0xF5; } }
        public static uint ATTN                  { get { return 0xF6; } }
        public static uint CRSEL                 { get { return 0xF7; } }
        public static uint EXSEL                 { get { return 0xF8; } }
        public static uint EREOF                 { get { return 0xF9; } }
        public static uint PLAY                  { get { return 0xFA; } }
        public static uint ZOOM                  { get { return 0xFB; } }
        public static uint NONAME                { get { return 0xFC; } }
        public static uint PA1                   { get { return 0xFD; } }
        public static uint OEM_CLEAR             { get { return 0xFE; } }

        public static uint MAPVK_VK_TO_VSC       { get { return 0x00; } }
        public static uint MAPVK_VSC_TO_VK       { get { return 0x01; } }
        public static uint MAPVK_VK_TO_CHAR      { get { return 0x02; } }
        public static uint MAPVK_VSC_TO_VK_EX    { get { return 0x03; } }
        public static uint MAPVK_VK_TO_VSC_EX    { get { return 0x04; } }

        public static uint KEYEVENTF_KEYUP      { get { return 0x02; } }
        public static uint KEYEVENTF_UNICODE    { get { return 0x04; } }
        public static uint KEYEVENTF_SCANCODE   { get { return 0x08; } }

    }
}
