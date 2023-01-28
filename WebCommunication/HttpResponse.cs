using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AKI.Communication.WebCommunication
{
    public class HttpResponse<T>
    {
        public int Code { get; private set; }
        public Dictionary<String, String> Headers { get; private set; }
        public string Body { get; set; }

        public HttpResponse(string response)
        {
            //StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            //string fullResponse = responseReader.ReadToEnd();
            //webResponse.Close();
            //Headers = new Dictionary<string, string>();
            //Code = (int)response.StatusCode;

            //if (response.Content != null)
            //{
            //    var streamTask = response.Content.ReadAsStreamAsync();
            //    Task.WaitAll(streamTask);
            //    Raw = streamTask.Result;

            //    if (typeof(T) == typeof(String))
            //    {
            //        var stringTask = response.Content.ReadAsStringAsync();
            //        Task.WaitAll(stringTask);
            //        Body = (T)(object)stringTask.Result;
            //    }
            //    else if (typeof(Stream).IsAssignableFrom(typeof(T)))
            //    {
            //        Body = (T)(object)Raw;
            //    }
            //    else
            //    {
            //        var stringTask = response.Content.ReadAsStringAsync();
            //        Task.WaitAll(stringTask);
            //        Body = JsonConvert.DeserializeObject<T>(stringTask.Result);
            //    }
            //}

            //foreach (var header in response.Headers)
            //{
            //    Headers.Add(header.Key, header.Value.First());
            //}
        }
    }
}
