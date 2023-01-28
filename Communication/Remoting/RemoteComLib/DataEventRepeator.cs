using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Communication.Remoting.RemoteComLib
{
    public delegate void DataReceiveHandler(object sender, ReceiveDataEventArgs e);

    public class DataEventRepeator : MarshalByRefObject
    {
        public event DataReceiveHandler ReceiveData;
        public void OnReceiveData(object sender, ReceiveDataEventArgs e)
        {
            if (this.ReceiveData != null)
            {
                ReceiveData(this, e);
            }
        }

    }
    public class DataEventRepeators : List<DataEventRepeator>
    {
    }
}
