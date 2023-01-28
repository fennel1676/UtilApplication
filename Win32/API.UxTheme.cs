using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AKI.Win32.API
{
    public class UxTheme
    {
        [DllImport("UxTheme.dll", CharSet = CharSet.Unicode)]
        public static extern int GetCurrentThemeName(StringBuilder pszThemeFileName, int dwMaxNameChars, StringBuilder pszColorBuff, int cchMaxColorChars, StringBuilder pszSizeBuff, int cchMaxSizeChars);

        [DllImport("UxTheme.dll")]
        public static extern bool IsAppThemed();
    }
}
