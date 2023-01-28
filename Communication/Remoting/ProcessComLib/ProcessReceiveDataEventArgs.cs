using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Communication.Remoting.ProcessComLib
{
    public class ProcessReceiveDataEventArgs : EventArgs
    {
        public string received_data;

        public ProcessReceiveDataEventArgs(string strData)
        {
            this.received_data = strData;
        }
    }
}
