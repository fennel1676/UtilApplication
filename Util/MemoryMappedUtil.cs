using System;
using System.Runtime.InteropServices;

namespace AKI.Util
{
    public class MemoryMappedUtil
    {
        /// file name
        private string m_strFileName;
        /// Windows handle to memory mapping of _file
        private IntPtr m_pMappedFile = IntPtr.Zero;

        MemoryMappedUtil()
        {
            m_pMappedFile = IntPtr.Zero;
        }

        // get OS page size(for remap)
        private static uint GetPageSize()
        {
            AKI.Win32.API.Kernel32.SYSTEM_INFO sysInfo = new Win32.API.Kernel32.SYSTEM_INFO();
            AKI.Win32.API.Kernel32.GetSystemInfo(out sysInfo);
            return sysInfo.allocationGranularity;
        }

        public bool CreateSharedMemory(string szName, int nSize)
        {
            m_pMappedFile = AKI.Win32.API.Coredll.CreateFileMapping(AKI.Win32.HandleApi.INVALID_HANDLE_VALUE, null, AKI.Win32.PAGE.READWRITE, 0, (uint)nSize, szName);
            return (IntPtr.Zero != m_pMappedFile);
        }

        public bool OpenSharedMemory(string szName)
        {
            if (IntPtr.Zero == m_pMappedFile)
            {
                m_pMappedFile = AKI.Win32.API.Coredll.OpenFileMapping(AKI.Win32.FILE_MAP.ALL_ACCESS, true, szName);
            }

            return IntPtr.Zero != m_pMappedFile;
        }

        /// close file
        public void CloseSharedMemrory()
        {
            if (IntPtr.Zero != m_pMappedFile)
            {
                AKI.Win32.API.Coredll.CloseHandle(m_pMappedFile);
                m_pMappedFile = IntPtr.Zero;
            }
        }

        public bool ReadData(ref byte[] szData, int nLen, int nOffset = 0)
        {
            if (IntPtr.Zero == m_pMappedFile)
                return false;

            if (null == szData || szData.Length < nLen)
                szData = new byte[nLen];

            IntPtr lpRet = AKI.Win32.API.Coredll.MapViewOfFile(m_pMappedFile, AKI.Win32.FILE_MAP.READ, 0, (uint)nOffset, (uint)nLen);
            if (IntPtr.Zero != lpRet)
            {
                Marshal.Copy(lpRet, szData, 0, nLen);
                AKI.Win32.API.Coredll.UnmapViewOfFile(lpRet);
                return true;
            }

            return false;
        }

        public bool WriteData(byte[] szData, int nLen, int nOffset)
        {
            if (IntPtr.Zero == m_pMappedFile)
                return false;

            IntPtr lpRet = AKI.Win32.API.Coredll.MapViewOfFile(m_pMappedFile, AKI.Win32.FILE_MAP.READ | AKI.Win32.FILE_MAP.WRITE, 0, (uint)nOffset, (uint)nLen);
            if (IntPtr.Zero != lpRet)
            {
                Marshal.Copy(szData, 0, lpRet, nLen);
                AKI.Win32.API.Coredll.UnmapViewOfFile(lpRet);
                return true;
            }
            return false;
        }
    }
}
