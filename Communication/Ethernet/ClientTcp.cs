using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AKI.Communication.Ethernet
{
    public class ClientTcp
    {
        private TcpClient m_socket;
        private List<byte[]> m_messagesToSend;
        private List<int> m_messagesSizeToSend;
        private int m_attemptCount;
        private Thread m_thread = null;
        private DateTime m_dtlastVerifyTime;
        private Encoding m_encoding;

        public TcpClient Socket { get { return m_socket; } set { m_socket = value; } }

        public Thread CallbackThread
        {
            get
            {
                return m_thread;
            }
            set
            {
                if (!CanStartNewThread())
                {
                    throw new Exception("Cannot override TcpServerConnection Callback Thread. The old thread is still running.");
                }
                m_thread = value;
            }
        }

        public DateTime LastVerifyTime { get { return m_dtlastVerifyTime; } }

        public Encoding Encoding { get { return m_encoding; } set { m_encoding = value; } }

        public ClientTcp(TcpClient sock, Encoding encoding)
        {
            m_socket = sock;
            m_messagesToSend = new List<byte[]>();
            m_messagesSizeToSend = new List<int>();
            m_attemptCount = 0;

            m_dtlastVerifyTime = DateTime.UtcNow;
            m_encoding = encoding;
        }

        public bool Connected()
        {
            try
            {
                return m_socket.Connected;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool VerifyConnected()
        {
            bool connected =    0 != m_socket.Client.Available ||
                                !m_socket.Client.Poll(1, SelectMode.SelectRead) ||
                                m_socket.Client.Available != 0;

            m_dtlastVerifyTime = DateTime.UtcNow;
            return connected;
        }

        public bool ProcessOutgoing(int maxSendAttempts)
        {
            lock (m_socket)
            {
                if (!m_socket.Connected)
                {
                    m_messagesToSend.Clear();
                    m_messagesSizeToSend.Clear();
                    return false;
                }

                if (m_messagesToSend.Count == 0)
                {
                    return false;
                }

                NetworkStream stream = m_socket.GetStream();
                try
                {
                    if (-1 == m_messagesSizeToSend[0])
                        stream.Write(m_messagesToSend[0], 0, m_messagesToSend[0].Length);
                    else
                        stream.Write(m_messagesToSend[0], 0, m_messagesSizeToSend[0]);
                    
                    lock (m_messagesToSend)
                    {
                        m_messagesToSend.RemoveAt(0);
                    }

                    lock (m_messagesSizeToSend)
                    {
                        m_messagesSizeToSend.RemoveAt(0);
                    }
                    m_attemptCount = 0;
                }
                catch (System.IO.IOException)
                {
                    m_attemptCount++;
                    if (m_attemptCount >= maxSendAttempts)
                    {
                        lock (m_messagesToSend)
                        {
                            m_messagesToSend.RemoveAt(0);
                        }

                        lock (m_messagesSizeToSend)
                        {
                            m_messagesSizeToSend.RemoveAt(0);
                        }
                        m_attemptCount = 0;
                    }
                }
                catch (ObjectDisposedException)
                {
                    m_socket.Close();
                    return false;
                }
            }
            return m_messagesToSend.Count != 0;
        }

        public void SendData(string data)
        {
            byte[] array = m_encoding.GetBytes(data);
            lock (m_messagesToSend)
            {
                m_messagesToSend.Add(array);
            }

            lock (m_messagesSizeToSend)
            {
                m_messagesSizeToSend.Add(-1);
            }
        }

        public void SendData(byte[] data)
        {
            byte[] array = data;
            lock (m_messagesToSend)
            {
                m_messagesToSend.Add(array);
            }

            lock (m_messagesSizeToSend)
            {
                m_messagesSizeToSend.Add(-1);
            }
        }

        public void SendData(byte[] data, int size)
        {
            byte[] array = data;
            lock (m_messagesToSend)
            {
                m_messagesToSend.Add(array);
            }

            lock (m_messagesSizeToSend)
            {
                m_messagesSizeToSend.Add(size);
            }
        }

        public void ForceDisconnect()
        {
            lock (m_socket)
            {
                m_socket.Close();
            }
        }

        public bool HasMoreWork()
        {
            return m_messagesToSend.Count > 0 || (Socket.Available > 0 && CanStartNewThread());
        }

        private bool CanStartNewThread()
        {
            if (m_thread == null)
            {
                return true;
            }
            return (m_thread.ThreadState & (ThreadState.Aborted | ThreadState.Stopped)) != 0 &&
                   (m_thread.ThreadState & ThreadState.Unstarted) == 0;
        }

    }
}
