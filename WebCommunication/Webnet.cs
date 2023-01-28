using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Communication.WebCommunication
{
    public class Webnet
    {
        public static HttpRequest post(string url)
        {
            return new HttpRequest("POST", url);
        }
    }
}
