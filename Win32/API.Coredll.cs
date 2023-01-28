using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AKI.Win32.API
{
    public class Coredll
    {
        [DllImport("Coredll.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFile(	string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes,
												uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("Coredll.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFileMapping(	IntPtr hFile, object lpFileMappingAttributes, uint flProtect, uint dwMaximumSizeHigh,
												uint dwMaximumSizeLow, string lpName);

        [DllImport("Coredll.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFileForMapping(	string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, // set null
												uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);


        [DllImport("Coredll.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr OpenFileMapping(uint dwDesiredAccess, bool bInheritHandle, string lpName);

        [DllImport("Coredll.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("Coredll.dll", SetLastError = true)]
        public static extern IntPtr MapViewOfFile(	IntPtr hFileMappingObject, uint dwDesiredAccess, uint dwFileOffsetHigh,
												uint dwFileOffsetLow, uint dwNumberOfBytesToMap);

        [DllImport("Coredll.dll", SetLastError = true)]
        public static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);


    }
}
