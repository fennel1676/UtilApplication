using AKI.Communication.Remoting.RemoteComLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Communication.Remoting.DataComServer
{
    public class DataServer : MarshalByRefObject, IDataReceiver
    {
        public event DataReceiveHandler ReceiveData;

        private static DataEventRepeators g_dataEventRepeators;

        public static DataEventRepeators Reapters
        {
            get
            {
                if (g_dataEventRepeators == null)
                    g_dataEventRepeators = new DataEventRepeators();

                return DataServer.g_dataEventRepeators;
            }
        }

        public void SendData(string strData)
        {
            ReceiveDataEventArgs e = new ReceiveDataEventArgs();
            e.Data = strData;
            try
            {
                OnReceiveData(e);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void OnReceiveData(ReceiveDataEventArgs e)
        {
            if (ReceiveData != null)
            {
                ReceiveData(null, e);
            }
        }

        public void AddEventRepeater(DataEventRepeator dataEventRepeator)
        {
            Reapters.Add(dataEventRepeator);
            this.ReceiveData += new DataReceiveHandler(dataEventRepeator.OnReceiveData);
        }
    }
}
