using System;
using System.Net;
using System.Net.Sockets;

namespace AvSimDllMotionExternCsharp
{
    public class AvSimDllCommSocket_PcSocket : IAvSimDllComm
    {
        private Socket g_sockfd = null;
        private bool g_connected = false;
        private int g_sockOptTimeout = 1000;
        private byte[] g_aMessage = new byte[1024 * 10];

        public const int FIONREAD = 0x4004667F;

        public const int WSAEINVAL = 0x2726;
        public const int WSAEWOULDBLOCK = 0x2733;
        public const int WSAEALREADY = 0x2735;
        public const int WSAEISCONN = 0x2748;

        private bool InitSocket()
        {
            if (g_sockfd != null)
            {
                Console.WriteLine("WARNING: CTcpClient::Open (): already opened socket");
                ComClose();
            }

            try
            {
                g_sockfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Set the socket to non-blocking mode.
                g_sockfd.Blocking = true;

                g_connected = false;
                return true;

            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
                goto CLOSE_RET;
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
            if (g_sockfd != null)
                g_sockfd.Close();

            g_sockfd = null;
            return false;
        }

        public bool ComOpen(int nPort)
        {
            return false;
        }

        public bool ComOpen(string remoteIp, int nPort)
        {
            if (g_sockfd == null)
                InitSocket();

            if (g_connected)
                return true;

            // Set the socket to non-blocking mode.
            int nonblocking = 1;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(remoteIp), nPort);
            
            try
            {
                g_sockfd.Connect(ipep);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Unable to connect to server.");
                Console.WriteLine(e.ToString());

                int error = e.NativeErrorCode;
                if (1 == nonblocking && (error == WSAEINVAL || error == WSAEWOULDBLOCK))
                {
                    return false;
                }
                else if (1 == nonblocking && (error == WSAEALREADY || error == WSAEISCONN))
                {
                    // 이미 연결된 소켓																	 // Connected
                }
                else
                {
                    goto CLOSE_RET;
                }
            }

            int size = sizeof(UInt32);
            UInt32 on = 1;
            UInt32 keepAliveInterval = 5000;   // Send a packet once every 10 seconds.
            UInt32 retryInterval = 1000;        // If no response, resend every second.
            byte[] inArray = new byte[size * 3];
            Array.Copy(BitConverter.GetBytes(on), 0, inArray, 0, size);
            Array.Copy(BitConverter.GetBytes(keepAliveInterval), 0, inArray, size, size);
            Array.Copy(BitConverter.GetBytes(retryInterval), 0, inArray, size * 2, size);

            g_sockfd.IOControl(IOControlCode.KeepAliveValues, inArray, null);

            // 연결되면 blocking 모드로 설정하고 send/recv 시에 timeout 값을 설정하여 무한히 대기하는것을 방지한다.
            g_sockfd.Blocking = true;

        	// Set the status for the keepalive option 
            g_sockfd.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            // socket의 opetion (sending/receiving timeout 값) 설정
            g_sockfd.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, g_sockOptTimeout);
            g_sockfd.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, g_sockOptTimeout);

	        g_connected = true;

	        return true;

CLOSE_RET:
	        //g_connected = FALSE;

	        if (g_sockfd != null)
            {
                g_sockfd.Shutdown(SocketShutdown.Both);
                g_sockfd.Close();
                g_sockfd = null;
            }            

	        return false;
        }

        public bool ComClose()
        {
            if (g_sockfd != null)
            {
                g_sockfd.Shutdown(SocketShutdown.Both);
                g_sockfd.Close();
                g_sockfd = null;
            }

            g_connected = false;
            return true;
        }

        public bool ComState()
        {
            return (g_sockfd != null) && g_connected;
        }

        public int ComRead(byte[] msg, int len)
        {
            if (g_sockfd == null)
                return -1;

            if (!g_connected)
                return -1;

            SocketError socketError;
            int ret = g_sockfd.Receive(msg, 0, len, SocketFlags.None, out socketError);
            if (ret == 0)
            {
                ComClose();
                return 0;
            }
            else
            {   
                if (SocketError.Success != socketError)
                {
                    DateTime dt = DateTime.Now;
                    Console.WriteLine("[{0}] SOCKET_ERROR(recv) : {1}\n", dt.ToString("HH:mm:ss.fff"), socketError.ToString());

                    if (socketError == SocketError.TimedOut)
                        return 0;

                    ComClose();
                    return -1;
                }
            }
            return ret;
        }

        public int ComRead(byte[] aMessage, int nLength, int nRecvPacketLength)
        {
            if (g_sockfd == null)
                return -1;

            if (!g_connected)
                return -1;

            if (nLength < nRecvPacketLength)
                return -2;

            SocketError socketError;
            int nRecvLength = g_sockfd.Receive(aMessage, 0, nLength, SocketFlags.None, out socketError);
            if (SocketError.Success != socketError)
            {
                DateTime dt = DateTime.Now;
                Console.WriteLine("[{0}] SOCKET_ERROR(recv) : {1}\n", dt.ToString("HH:mm:ss.fff"), socketError.ToString());

                if (socketError == SocketError.TimedOut || socketError == SocketError.WouldBlock)
                    return 0;

                ComClose();
                return -1;
            }
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
	        if (g_sockfd == null)
                return -1;

	        if (!g_connected)
		        return -1;

            SocketError socketError;
            int ret = g_sockfd.Send(msg, 0, len, SocketFlags.None, out socketError);
	        if (ret == 0)
            {
                ComClose();
		        return 0;
	        }
	        else 
	        {
                if (SocketError.Success != socketError)
                {
                    DateTime dt = DateTime.Now;
                    Console.WriteLine("[{0}] SOCKET_ERROR(send) : {1}\n", dt.ToString("HH:mm:ss.fff"), socketError.ToString());

                    if (socketError == SocketError.TimedOut)
                        return 0;

                    ComClose();
			        return -1;
		        }
	        }
	        return ret;
        }
    }
}
