using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace AKI.Communication.WebCommunication
{
    public class HttpRequest
    {
        private bool hasFields;

        private bool hasExplicitBody;

        private string m_strMethod = "POST";

        public Uri URL { get; protected set; }

        public string Method { get { return m_strMethod; } }

        public Dictionary<String, String> Headers { get; protected set; }

        public Dictionary<string, object> Body { get; private set; }

        // Should add overload that takes URL object
        public HttpRequest(string strMethod, string url)
        {
            Uri locurl;
            
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out locurl))
            {
                if (
                    !(locurl.IsAbsoluteUri &&
                      (locurl.Scheme == "http" || locurl.Scheme == "https")) ||
                    !locurl.IsAbsoluteUri)
                {
                    throw new ArgumentException("The url passed to the HttpMethod constructor is not a valid HTTP/S URL");
                }
            }
            else
            {
                throw new ArgumentException("The url passed to the HttpMethod constructor is not a valid HTTP/S URL");
            }

            m_strMethod = strMethod;
            URL = locurl;
            Headers = new Dictionary<string, string>();
            Body = new Dictionary<string, object>();
        }

        public HttpRequest header(string name, string value)
        {
            Headers.Add(name, value);
            return this;
        }

        public HttpRequest headers(Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    Headers.Add(header.Key, header.Value);
                }
            }

            return this;
        }

        public HttpRequest field(string name, string value)
        {
            if (hasExplicitBody)
            {
                throw new InvalidOperationException("Can't add fields to a request with an explicit body");
            }

            Body.Add(name, value);

            hasFields = true;
            return this;
        }

        public HttpRequest fields(Dictionary<string, object> parameters)
        {
            if (hasExplicitBody)
            {
                throw new InvalidOperationException("Can't add fields to a request with an explicit body");
            }

            foreach (KeyValuePair<string, object> pair in parameters)
            {
                if (pair.Value is String)
                    Body.Add(pair.Key, pair.Value);
            }

            hasFields = true;
            return this;
        }

        public HttpWebResponse asJson()
        {
            return HttpClientHelper.Request(this);
        }
    }
}
