using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace AKI.Communication.Remoting.RemoteComLib
{
    public interface IDataReceiver
    {
        [OneWay]
        void SendData(string strData);

        void AddEventRepeater(DataEventRepeator dataEventRepeator);
    }
}
