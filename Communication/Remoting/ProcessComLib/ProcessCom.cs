using AKI.Communication.Remoting.RemoteComLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace AKI.Communication.Remoting.ProcessComLib
{
    public class ProcessCom
    {
        public delegate void ProcessReceivedDataHandle(object sender, ProcessReceiveDataEventArgs e);
        public event ProcessReceivedDataHandle ProcessReceivedData;

        //EventSinkCollections
        private DataEventRepeator repeater = new DataEventRepeator();
        //Server Local Port（Proxy）
        private IDataReceiver m_dataServer;

        protected void OnProcessReceivedData(object sender, ProcessReceiveDataEventArgs e)
        {
            if (ProcessReceivedData != null)
            {
                ProcessReceivedData(sender, e);
            }
        }
        public void InitCommunication()
        {
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary dicProperties = new Hashtable();
            dicProperties["port"] = 0;

            TcpChannel tcpChannel = new TcpChannel(dicProperties, null, provider);
            ChannelServices.RegisterChannel(tcpChannel, false);
            repeater.ReceiveData += new DataReceiveHandler(repeater_ReceiveData);
            try
            {
                IDictionary prop = new Hashtable();
                dicProperties["port"] = 2007;
                TcpChannel chnn = new TcpChannel(dicProperties, null, provider);
                ChannelServices.UnregisterChannel(tcpChannel);
                ChannelServices.RegisterChannel(chnn, false);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(DataComServer.DataServer), "DataServer.rem", WellKnownObjectMode.Singleton);
            }
            catch
            {

            }

            Connection();
        }

        private void Connection()
        {
            //connect server
            m_dataServer = (IDataReceiver)Activator.GetObject(typeof(IDataReceiver), "tcp://localhost:2007/DataServer.rem");
            //add repter to EventRepeaterList
            m_dataServer.AddEventRepeater(repeater);
        }

        public void SendData(string strData)
        {
            m_dataServer.SendData(strData);
        }

        void repeater_ReceiveData(object sender, ReceiveDataEventArgs e)
        {
            ProcessReceiveDataEventArgs ev = new ProcessReceiveDataEventArgs(e.Data);
            OnProcessReceivedData(sender, ev);
        }
    }
}
