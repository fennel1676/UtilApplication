using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace AKI.Util
{
    public class XmlUtil
    {
        public static int Load<T>(out T tObject, string strFilePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            if (null == serializer)
            {
                tObject = default(T);
                return 1;
            }
            XmlReader reader = XmlReader.Create(strFilePath);
            if (null == reader)
            {
                tObject = default(T);
                return 2;
            }
            tObject = (T)serializer.Deserialize(reader);
            if (null == tObject)
            {
                tObject = default(T);
                return 3;
            }
            reader.Close();
            return 0;
        }

        public static int LoadXml<T>(out T tObject, string strXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            if (null == serializer)
            {
                tObject = default(T);
                return 1;
            }

            XmlReader reader = XmlReader.Create(new StringReader(strXml));
            if (null == reader)
            {
                tObject = default(T);
                return 2;
            }
            tObject = (T)serializer.Deserialize(reader);
            if (null == tObject)
            {
                tObject = default(T);
                return 3;
            }
            reader.Close();
            return 0;
        }

        public static int Save<T>(T tObject, string strFilePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            if (null == serializer)
            {
                tObject = default(T);
                return 1;
            }
            StringBuilder resutl = new StringBuilder();
            if (null == resutl)
            {
                tObject = default(T);
                return 2;
            }
            XmlWriter writer = XmlWriter.Create(strFilePath);
            if (null == writer)
            {
                tObject = default(T);
                return 3;
            }
            serializer.Serialize(writer, tObject);
            writer.Close();
            return 0;
        }

        public static int SaveXml<T>(T tObject, out string strOutXml)
        {
            XmlSerializer serializer = null;
            if (typeof(T) == tObject.GetType())
                serializer = new XmlSerializer(typeof(T));
            else
                serializer = new XmlSerializer(tObject.GetType());

            strOutXml = string.Empty;

            if (null == serializer)
            {
                tObject = default(T);
                return 1;
            }

            StringBuilder resutl = new StringBuilder();
            if (null == resutl)
            {
                tObject = default(T);
                return 2;
            }
            StringBuilder strbXml = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(strbXml);
            if (null == writer)
            {
                tObject = default(T);
                return 3;
            }
            serializer.Serialize(writer, tObject);
            writer.Close();
            strOutXml = strbXml.ToString();
            return 0;
        }
    }
}
