using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AvSimDll.Communication
{
    public class Serial
    {
        public static int BUFFER_SIZE = 1024;

        private int m_nPort = 1;
        private int m_nBaudrate = 115200;
        private byte[] m_aBuf = null;
        private IntPtr m_ptrBuf = IntPtr.Zero;

        public int Port { get { return m_nPort; } }
        public int Baudrate { get { return m_nBaudrate; } }

        [DllImport("AvSimDllCommunication.dll")]
        private extern static int Communication_Open(int nPort, int nBaud);

        [DllImport("AvSimDllCommunication.dll")]
        private extern static int Communication_Close(int nPort);

        [DllImport("AvSimDllCommunication.dll")]
        private extern static int Communication_State(int nPort);

        [DllImport("AvSimDllCommunication.dll")]
        private extern static int Communication_Getc(int nPort, ref byte pBuf);

        [DllImport("AvSimDllCommunication.dll")]
        private extern static int Communication_Putc(int nPort, byte chBuf);

        [DllImport("AvSimDllCommunication.dll")]
        private extern static int Communication_Read(int nPort, IntPtr pBuf, int nSize);

        [DllImport("AvSimDllCommunication.dll")]
        private extern static int Communication_Write(int nPort, IntPtr pBuf, int nSize);

        public bool Open(int nPort, int nBaudrate = 115200)
        {
            m_nPort = nPort;
            m_nBaudrate = nBaudrate;
            int nResult = Communication_Open(m_nPort, m_nBaudrate);
            if (0 != nResult)
            {
                m_aBuf = new byte[BUFFER_SIZE];
                m_ptrBuf = Marshal.AllocHGlobal(BUFFER_SIZE);
                return true;
            }
            else
            {
                if (IntPtr.Zero != m_ptrBuf)
                {
                    Marshal.FreeHGlobal(m_ptrBuf);
                    m_ptrBuf = IntPtr.Zero;
                }
                return false;
            }
        }

        public bool Close()
        {
            if (IntPtr.Zero != m_ptrBuf)
            {
                Marshal.FreeHGlobal(m_ptrBuf);
                m_ptrBuf = IntPtr.Zero;
            }

            return (0 == Communication_Close(m_nPort)) ? false : true;
        }

        public int State()
        {
            if (IntPtr.Zero == m_ptrBuf)
                return -1;

            return Communication_State(m_nPort);
        }

        public int Getc(ref byte pBuf)
        {
            if (IntPtr.Zero == m_ptrBuf)
                return -1;

            return Communication_Getc(m_nPort, ref pBuf);
        }

        public int Putc(byte chBuf)
        {
            if (IntPtr.Zero == m_ptrBuf)
                return -1;

            return Communication_Putc(m_nPort, chBuf);
        }

        public int Read(IntPtr pBuf, int nSize)
        {
            if (IntPtr.Zero == m_ptrBuf)
                return -1;

            return Communication_Read(m_nPort, pBuf, nSize);
        }

        public int Read(out byte[] aBuf)
        {
            aBuf = null;
            if (IntPtr.Zero == m_ptrBuf)
                return -1;

            int nResult = Communication_Read(m_nPort, m_ptrBuf, BUFFER_SIZE);
            if (0 < nResult)
            {
                aBuf = new byte[nResult];
                Marshal.Copy(m_ptrBuf, aBuf, 0, nResult);
            }
            return nResult;
        }

        public int Read(out string strBuf)
        {
            strBuf = string.Empty;
            if (IntPtr.Zero == m_ptrBuf)
                return -1;

            byte[] aBuf = null;
            int nResult = Read(out aBuf);
            if (0 < nResult)
                strBuf = Encoding.ASCII.GetString(aBuf, 0, nResult);

            return nResult;
        }

        public int Write(IntPtr pBuf, int nSize)
        {
            if (IntPtr.Zero == m_ptrBuf)
                return -1;

            return Communication_Write(m_nPort, pBuf, nSize);
        }

        public int Write(byte[] aBuf, int nSize)
        {
            if (IntPtr.Zero == m_ptrBuf)
                return -1;

            IntPtr ptr = IntPtr.Zero;
            if (BUFFER_SIZE >= nSize)
                ptr = m_ptrBuf;
            else
                ptr = Marshal.AllocHGlobal(nSize);
            Marshal.Copy(aBuf, 0, ptr, nSize);
            int nResult = Communication_Write(m_nPort, ptr, nSize);
            if (BUFFER_SIZE < nSize)
                Marshal.FreeHGlobal(ptr);
            return nResult;
        }

        public int Write(string strBuf)
        {
            if (IntPtr.Zero == m_ptrBuf)
                return -1;

            return Write(Encoding.ASCII.GetBytes(strBuf), strBuf.Length);
        }
    }
}
