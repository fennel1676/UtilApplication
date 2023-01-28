using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Communication.Remoting.RemoteComLib
{
    [Serializable]
    public class ReceiveDataEventArgs : EventArgs
    {
        private string m_strData;

        public string Data { get { return m_strData; } set { m_strData = value; } }
    }
}
