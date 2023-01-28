using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace AvSimDllMotionExternCsharp
{
    public class AvSimDllCommSocket_AndroidSocket : IAvSimDllComm
    {
        private AndroidJavaClass JAVASocketCommunicationClass;
        private AndroidJavaObject JAVASocketCommunicationObject;

        private AndroidJavaObject g_sockfd;

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
                Console.WriteLine("WARNING: InitSocket(): already opened socket");
                ComClose();
            }

            JAVASocketCommunicationClass = new AndroidJavaClass("com.PNI.JAVASocketCommunication");
            JAVASocketCommunicationObject = JAVASocketCommunicationClass.CallStatic<AndroidJavaObject>("getInstance");

            try
            {
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
                JAVASocketCommunicationObject.Call("close", g_sockfd);

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

            int nonblocking = 1;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(remoteIp), nPort);
            
            try
            {
                g_sockfd = JAVASocketCommunicationObject.Call<AndroidJavaObject>("createSocket", remoteIp, nPort);
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

                }
                else
                {
                    goto CLOSE_RET;
                }
            }

            g_connected = true;

	        return true;

CLOSE_RET:
	        if (g_sockfd != null)
            {
                JAVASocketCommunicationObject.Call("close",g_sockfd);
                g_sockfd = null;
            }            

	        return false;
        }

        public bool ComClose()
        {
            if (g_sockfd != null)
            {
                JAVASocketCommunicationObject.Call("close", g_sockfd);
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
            byte[] readData = JAVASocketCommunicationObject.Call<byte[]>("ReadByte", g_sockfd, len);
            Debug.Log(Encoding.Default.GetString(readData));
            Array.Copy(readData, 0, msg, 0, readData.Length);

            return readData.Length;
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
            byte[] recievedData = JAVASocketCommunicationObject.Call<byte[]>("ReadByte", g_sockfd, nLength);
            Array.Copy(recievedData, 0,aMessage,0,recievedData.Length);
            int nRecvLength = aMessage.Length;
            socketError = SocketError.Success;

            if (SocketError.Success != socketError)
            {
                DateTime dt = DateTime.Now;
                Console.WriteLine("[{0}] SOCKET_ERROR(recv) : {1}\n", dt.ToString("HH:mm:ss.fff"), socketError.ToString());

                if (socketError == SocketError.TimedOut)
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

                    nRecv = ComRead(g_aMessage, nRecvPacketLength - nRecvLength);
                    Console.WriteLine("else if (nRecvLength < nRecvPacketLength) = {0} / {1} / {2} / {3}", i, nRecvLength, nRecvPacketLength, nRecv);

                    if (nRecv <= 0)
                        return nRecv;

                    Array.Copy(g_aMessage, 0, aMessage, nRecvLength, nRecv);
                    nRecvLength += nRecv;


                    if (nRecvLength == nRecvPacketLength)
                        return nRecvPacketLength;
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

            JAVASocketCommunicationObject.Call("WriteByte", g_sockfd, msg, len);

            return len;
        }
    }
}
