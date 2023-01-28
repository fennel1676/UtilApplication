using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AKI.Communication.Ethernet
{
    public class ServerAsyncTcp
    {
        public delegate void delegateServerTcpConnectionChanged(ClientTcp clientTcp);
        public delegate void delegateServerTcpError(ServerAsyncTcp server, Exception e);

        public event delegateServerTcpConnectionChanged Event_Connect = null;
        public event delegateServerTcpConnectionChanged Event_Disconnect = null;
        public event delegateServerTcpConnectionChanged Event_DataAvailable = null;
        public event delegateServerTcpError Event_Error = null;

        private List<ClientTcp> m_listConnections;
        private TcpListener m_tcpListener;

        private Thread m_threadListen;
        private Thread m_threadSend;

        private bool m_isOpen;

        private int m_nPort;
        private int m_nMaxSendAttempts;
        private int m_nIdleTime;
        private int m_nMaxCallbackThreads;
        private int m_nVerifyConnectionInterval;
        private Encoding m_encoding;

        private SemaphoreSlim m_semaphoreSlim;
        private bool m_isWaiting;

        private int m_nActiveThreads;
        private object m_objActiveThreadsLock = new object();
        
        public ServerAsyncTcp()
        {
            initialize();
        }

        private void initialize()
        {
            m_listConnections = new List<ClientTcp>();
            m_tcpListener = null;

            m_threadListen = null;
            m_threadSend = null;

            m_nPort = -1;
            m_nMaxSendAttempts = 3;
            m_isOpen = false;
            m_nIdleTime = 50;
            m_nMaxCallbackThreads = 100;
            m_nVerifyConnectionInterval = 100;
            m_encoding = Encoding.ASCII;

            m_semaphoreSlim = new SemaphoreSlim(0);
            m_isWaiting = false;

            m_nActiveThreads = 0;
        }

        public int Port
        {
            get
            {
                return m_nPort;
            }
            set
            {
                if (value < 0)
                    return;

                if (m_nPort == value)
                    return;

                if (m_isOpen)
                    throw new Exception("Invalid attempt to change port while still open.\nPlease close port before changing.");

                m_nPort = value;
                if (m_tcpListener == null)
                    m_tcpListener = new TcpListener(IPAddress.Any, m_nPort);
                else
                    m_tcpListener.Server.Bind(new IPEndPoint(IPAddress.Any, m_nPort));
            }
        }

        public int MaxSendAttempts { get { return m_nMaxSendAttempts; } set { m_nMaxSendAttempts = value; } }

        public bool IsOpen
        {
            get
            {
                return m_isOpen;
            }
            set
            {
                if (m_isOpen == value)
                    return;

                if (value)
                    Open();
                else
                    Close();
            }
        }

        public List<ClientTcp> Connections
        {
            get
            {
                List<ClientTcp> listConnections = new List<ClientTcp>();
                listConnections.AddRange(m_listConnections);
                return listConnections;
            }
        }

        public int IdleTime { get { return m_nIdleTime; } set { m_nIdleTime = value; } }

        public int MaxCallbackThreads { get { return m_nMaxCallbackThreads; } set { m_nMaxCallbackThreads = value; } }

        public int VerifyConnectionInterval { get { return m_nVerifyConnectionInterval; } set { m_nVerifyConnectionInterval = value; } }

        public Encoding Encoding
        {
            get
            {
                return m_encoding;
            }
            set
            {
                Encoding oldEncoding = m_encoding;
                m_encoding = value;
                foreach (ClientTcp client in m_listConnections)
                {
                    if (client.Encoding == oldEncoding)
                        client.Encoding = m_encoding;
                }
            }
        }

        public void SetEncoding(Encoding encoding, bool changeAllClients)
        {
            Encoding oldEncoding = m_encoding;
            m_encoding = encoding;
            if (changeAllClients)
            {
                foreach (ClientTcp client in m_listConnections)
                {
                    client.Encoding = m_encoding;
                }
            }
        }

        private void runListener()
        {
            while (m_isOpen && m_nPort >= 0)
            {
                try
                {
                    if (m_tcpListener.Pending())
                    {
                        TcpClient socket = m_tcpListener.AcceptTcpClient();
                        ClientTcp conn = new ClientTcp(socket, m_encoding);

                        if (Event_Connect != null)
                        {
                            lock (m_objActiveThreadsLock)
                            {
                                m_nActiveThreads++;
                            }
                            conn.CallbackThread = new Thread(() => { Event_Connect(conn); });
                            conn.CallbackThread.Start();
                        }

                        lock (m_listConnections)
                        {
                            m_listConnections.Add(conn);
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(m_nIdleTime);
                    }
                }
                catch (ThreadInterruptedException)
                {
                }
                catch (Exception e)
                {
                    if (m_isOpen && Event_Error != null)
                    {
                        Event_Error(this, e);
                    }
                }
            }
        }

        private void runSender()
        {
            while (m_isOpen && m_nPort >= 0)
            {
                try
                {
                    bool moreWork = false;
                    for (int i = 0; i < m_listConnections.Count; i++)
                    {
                        if (m_listConnections[i].CallbackThread != null)
                        {
                            try
                            {
                                m_listConnections[i].CallbackThread = null;
                                lock (m_objActiveThreadsLock)
                                {
                                    m_nActiveThreads--;
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }

                        if (m_listConnections[i].CallbackThread != null)
                        {
                        }
                        else if (m_listConnections[i].Connected() &&
                                (m_listConnections[i].LastVerifyTime.AddMilliseconds(m_nVerifyConnectionInterval) > DateTime.UtcNow ||
                                m_listConnections[i].VerifyConnected()))
                        {
                            moreWork = moreWork || ProcessConnection(m_listConnections[i]);
                        }
                        else
                        {
                            lock (m_listConnections)
                            {
                                if (null != Event_Disconnect)
                                    Event_Disconnect(m_listConnections[i]);
                                m_listConnections.RemoveAt(i);
                                i--;
                            }
                        }
                    }

                    if (!moreWork)
                    {
                        System.Threading.Thread.Yield();
                        lock (m_semaphoreSlim)
                        {
                            foreach (ClientTcp conn in m_listConnections)
                            {
                                if (conn.HasMoreWork())
                                {
                                    moreWork = true;
                                    break;
                                }
                            }
                        }
                        if (!moreWork)
                        {
                            m_isWaiting = true;
                            m_semaphoreSlim.Wait(m_nIdleTime);
                            m_isWaiting = false;
                        }
                    }
                }
                catch (ThreadInterruptedException)
                {
                }
                catch (Exception e)
                {
                    if (m_isOpen && Event_Error != null)
                        Event_Error(this, e);
                }
            }
        }

        private bool ProcessConnection(ClientTcp conn)
        {
            bool moreWork = false;
            if (conn.ProcessOutgoing(m_nMaxSendAttempts))
            {
                moreWork = true;
            }

            if (null != Event_DataAvailable && m_nActiveThreads < m_nMaxCallbackThreads && conn.Socket.Available > 0)
            {
                lock (m_objActiveThreadsLock)
                {
                    m_nActiveThreads++;
                }

                conn.CallbackThread = new Thread(() => { Event_DataAvailable(conn); });
                conn.CallbackThread.Start();
                Thread.Yield();
            }
            return moreWork;
        }

        public void Open()
        {
            lock (this)
            {
                if (m_isOpen)
                    return;

                if (m_nPort < 0)
                    throw new Exception("Invalid port");

                try
                {
                    m_tcpListener.Start(5);
                }
                catch (Exception)
                {
                    m_tcpListener.Stop();
                    m_tcpListener = new TcpListener(IPAddress.Any, m_nPort);
                    m_tcpListener.Start(5);
                }

                m_isOpen = true;

                m_threadListen = new Thread(new ThreadStart(runListener));
                m_threadListen.Start();

                m_threadSend = new Thread(new ThreadStart(runSender));
                m_threadSend.Start();
            }
        }

        public void Close()
        {
            if (!m_isOpen)
            {
                return;
            }

            lock (this)
            {
                m_isOpen = false;
                foreach (ClientTcp conn in m_listConnections)
                {
                    conn.ForceDisconnect();
                }

                try
                {
                    if (m_threadListen.IsAlive)
                    {
                        m_threadListen.Interrupt();

                        Thread.Yield();
                        if (m_threadListen.IsAlive)
                            m_threadListen.Abort();
                    }
                }
                catch (System.Security.SecurityException)
                {
                }

                try
                {
                    if (m_threadSend.IsAlive)
                    {
                        m_threadSend.Interrupt();

                        Thread.Yield();
                        if (m_threadSend.IsAlive)
                            m_threadSend.Abort();
                    }
                }
                catch (System.Security.SecurityException) { }
            }

            m_tcpListener.Stop();

            lock (m_listConnections)
            {
                m_listConnections.Clear();
            }

            m_threadListen = null;
            m_threadSend = null;
            GC.Collect();
        }

        public void Send(string data)
        {
            lock (m_semaphoreSlim)
            {
                foreach (ClientTcp conn in m_listConnections)
                {
                    conn.SendData(data);
                }
                Thread.Yield();
                if (m_isWaiting)
                {
                    m_semaphoreSlim.Release();
                    m_isWaiting = false;
                }
            }
        }

        public void Send(byte[] data)
        {
            lock (m_semaphoreSlim)
            {
                foreach (ClientTcp conn in m_listConnections)
                {

                    conn.SendData(data);
                }
                Thread.Yield();
                if (m_isWaiting)
                {
                    m_semaphoreSlim.Release();
                    m_isWaiting = false;
                }
            }
        }

        public void Send(string ip, byte[] data)
        {
            lock (m_semaphoreSlim)
            {
                foreach (ClientTcp conn in m_listConnections)
                {
                    string connetClient = IPAddress.Parse(((IPEndPoint)conn.Socket.Client.RemoteEndPoint).Address.ToString()).ToString();
                    if (connetClient.Equals(ip))
                        conn.SendData(data);
                }

                Thread.Yield();

                if (m_isWaiting)
                {
                    m_semaphoreSlim.Release();
                    m_isWaiting = false;
                }
            }
        }

    }
}
