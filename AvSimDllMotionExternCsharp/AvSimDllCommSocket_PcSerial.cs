using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.IO.Ports;

namespace AvSimDllMotionExternCsharp
{
    public class AvSimDllCommSocket_PcSerial : IAvSimDllComm
    {
        private SerialPort g_serial = null;

        private Socket g_sockfd = null;
        private bool g_connected = false;
        private int g_sockOptTimeout = 1000;
        private byte[] g_aMessage = new byte[1024 * 10];

        public const int FIONREAD = 0x4004667F;

        public const int WSAEINVAL = 0x2726;
        public const int WSAEWOULDBLOCK = 0x2733;
        public const int WSAEALREADY = 0x2735;
        public const int WSAEISCONN = 0x2748;

        private bool InitSerial()
        {
            if (g_serial != null)
            {
                Console.WriteLine("WARNING: Open (): already opened serial");
                ComClose();
            }

            try
            {
                g_serial = new SerialPort();
                return true;

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
                goto CLOSE_RET;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("NullReferenceException caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
                goto CLOSE_RET;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
                goto CLOSE_RET;
            }
            
            CLOSE_RET:
            if (g_serial != null)
                g_serial.Close();

            g_serial = null;
            return false;
        }

        public bool ComOpen(string remoteIp, int nPort)
        {
            return false;
        }

        public bool ComOpen(int nPort)
        {
            if (g_serial == null)
                InitSerial();

            if (null != g_serial && g_serial.IsOpen)
                return true;

            try
            {
                // Allow the user to set the appropriate properties.
                g_serial.PortName = string.Format("COM{0}", nPort);
                g_serial.BaudRate = 115200;
                g_serial.Parity = Parity.None;
                g_serial.DataBits = 8;
                g_serial.StopBits = StopBits.One;
                g_serial.ReadBufferSize = 4096;
                g_serial.WriteBufferSize = 4096;

                g_serial.ReadTimeout = 500;
                g_serial.WriteTimeout = 500;
                
                g_serial.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to open to port.");
                Console.WriteLine(e.ToString());

                goto CLOSE_RET;
            }

	        return true;

CLOSE_RET:

	        if (g_serial != null)
            {
                g_serial.Close();
                g_serial = null;
            }            

	        return false;
        }

        public bool ComClose()
        {
            if (g_serial != null)
            {
                g_serial.Close();
                g_serial = null;
            }
            return true;
        }

        public bool ComState()
        {
            return null == g_serial ? false : g_serial.IsOpen;
        }

        public int ComRead(byte[] msg, int len)
        {
            if (g_serial == null)
                return -1;

            if (!g_serial.IsOpen)
                return -1;
            
            int dwRead = g_serial.BytesToRead;
            if (dwRead > 0)
            {
                if (dwRead > len)
                    dwRead = len;
            }

            return g_serial.Read(msg, 0, dwRead);
        }

        public int ComRead(byte[] aMessage, int nLength, int nRecvPacketLength)
        {
            if (g_serial == null)
                return -1;

            if (!g_serial.IsOpen)
                return -1;

            if (nLength < nRecvPacketLength)
                return -2;

            int nRecvLength = g_serial.Read(aMessage, 0, nLength);
            if (nRecvLength == 0)
            {
                ComClose();
                return 0;
            }
            else if (nRecvLength < nRecvPacketLength)
            {
                Console.WriteLine("else if (nRecvLength < nRecvPacketLength) = {0} / {1} / {2}", -1, nRecvLength, nRecvPacketLength);

                int nRecv = 0;

                for (int i = 0; i < nRecvPacketLength; i++)
                {
                    System.Threading.Thread.Sleep(1);

                    nRecv = ComRead(g_aMessage, nLength);
                    Console.WriteLine("else if (nRecvLength < nRecvPacketLength) = {0} / {1} / {2} / {3}", i, nRecvLength, nRecvPacketLength, nRecv);

                    if (nRecv <= 0)
                        return nRecv;

                    Array.Copy(g_aMessage, 0, aMessage, nRecvLength, nRecv);                    

                    if (nRecvLength + nRecv >= nRecvPacketLength)
                        return nRecvPacketLength + nRecv;
                    else
                        nRecvLength += nRecv;

                }
            }
            else
            {
            }
            return nRecvLength;
        }

        public int  ComWrite(byte[] msg, int len)
        {
            if (g_serial == null)
                return -1;

            if (!g_serial.IsOpen)
                return -1;

            SocketError socketError;
            g_serial.Write(msg, 0, len);
	        return g_serial.BytesToWrite;
        }
    }
}
