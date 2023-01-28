using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace AKI.Communication.Util
{
    public class GeneralPacketWrapper
    {
        public const int HEADSIZE = 8;
        public const int BODYSIZE = (1024 * 4) - HEADSIZE;

        #region Struct

        public struct GeneralPacket
        {
            public int CommandType;

            public int BodySize;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GeneralPacketWrapper.BODYSIZE)]
            public byte[] Body;
        }

        #endregion

        #region Member Variable

        private bool m_bNetworkOrderConversion = false;

        #endregion

        #region Properties

        public bool NetworkOrderConversion { get { return m_bNetworkOrderConversion; } set { m_bNetworkOrderConversion = value; } }

        public int CommandType { get { return m_stGeneralPacket.CommandType; } set { m_stGeneralPacket.CommandType = value; } }

        public int FullSize { get { return m_stGeneralPacket.BodySize + HeadSize; } }

        public int HeadSize { get { return HEADSIZE; } }

        public int BodySize { get { return m_stGeneralPacket.BodySize; } set { m_stGeneralPacket.BodySize = value; } }

        public int BufferSize { get { return m_stGeneralPacket.Body.Length + HeadSize; } }

        public string BodyString
        {
            get
            {
                return Encoding.Unicode.GetString(m_stGeneralPacket.Body, 0, m_stGeneralPacket.BodySize);
            }
            set
            {
                byte[] aData = Encoding.Unicode.GetBytes(value);
                Array.Copy(aData, m_stGeneralPacket.Body, aData.Length);
            }
        }

        public byte[] BodyByte { get { return m_stGeneralPacket.Body; } set { Array.Copy(value, m_stGeneralPacket.Body, value.Length); } }

        #endregion

        private GeneralPacket m_stGeneralPacket;

        #region Construct

        public GeneralPacketWrapper(bool bNetworkOrderConversion = false)
        {
            m_stGeneralPacket = new GeneralPacket();
            m_stGeneralPacket.Body = new byte[BODYSIZE];
            m_bNetworkOrderConversion = bNetworkOrderConversion;
        }

        public GeneralPacketWrapper(int nCommandType, int nBodySize, string strBody, bool bNetworkOrderConversion = false)
        {
            m_stGeneralPacket = new GeneralPacket();
            m_stGeneralPacket.Body = new byte[BODYSIZE];
            m_stGeneralPacket.BodySize = nBodySize;
            m_bNetworkOrderConversion = bNetworkOrderConversion;

            m_stGeneralPacket.CommandType = nCommandType;
            byte[] aData = Encoding.Unicode.GetBytes(strBody);
            Array.Copy(aData, m_stGeneralPacket.Body, nBodySize);
        }

        public GeneralPacketWrapper(int nCommandType, string strBody, bool bNetworkOrderConversion = false)
        {
            m_stGeneralPacket = new GeneralPacket();
            m_stGeneralPacket.Body = new byte[BODYSIZE];
            m_bNetworkOrderConversion = bNetworkOrderConversion;

            m_stGeneralPacket.CommandType = nCommandType;
            byte[] aData = Encoding.Unicode.GetBytes(strBody);
            m_stGeneralPacket.BodySize = aData.Length;
            Array.Copy(aData, m_stGeneralPacket.Body, aData.Length);
        }

        public GeneralPacketWrapper(byte[] aData, bool bNetworkOrderConversion = false)
        {
            if (aData.Length < HeadSize)
                throw new Exception(string.Format("데이터 사이즈가 최소한의 헤더 사이즈 보다 데이터가 적습니다. (헤더 사이즈: {0} / {1})", HeadSize, aData.Length));

            if (aData.Length > BODYSIZE + HeadSize)
                throw new Exception(string.Format("입력된 데이터 사이즈 값이 최대 버퍼 사이즈 보다 큽니다. (버퍼 사이즈 : {0} / {1})", BODYSIZE + HeadSize, aData.Length));

            m_stGeneralPacket = new GeneralPacket();
            m_stGeneralPacket.Body = new byte[BODYSIZE];

            m_stGeneralPacket.CommandType = BitConverter.ToInt32(aData, 0);
            m_stGeneralPacket.BodySize = BitConverter.ToInt32(aData, 4);

            m_bNetworkOrderConversion = bNetworkOrderConversion;
            if (m_bNetworkOrderConversion)
            {
                m_stGeneralPacket.CommandType = IPAddress.NetworkToHostOrder(m_stGeneralPacket.CommandType);
                m_stGeneralPacket.BodySize = IPAddress.NetworkToHostOrder(m_stGeneralPacket.BodySize);
            }

            Array.Copy(aData, 8, m_stGeneralPacket.Body, 0, m_stGeneralPacket.BodySize);
        }

        public GeneralPacketWrapper(byte[] aData, int nSize, bool bNetworkOrderConversion = false)
        {
            if (aData.Length < HeadSize)
                throw new Exception(string.Format("데이터 사이즈가 최소한의 헤더 사이즈 보다 데이터가 적습니다. (헤더 사이즈: {0} / {1})", HeadSize, aData.Length));

            if (aData.Length < nSize)
                throw new Exception(string.Format("데이터 사이즈와 입력된 사이즈 값보다 적습니다. (데이터 사이즈: {0} / {1})", aData.Length, nSize));

            if (nSize < HeadSize)
                throw new Exception(string.Format("입력된 데이터 사이즈 값이 최소 버퍼 사이즈 보다 큽니다. (헤더 사이즈 : {0} / {1})", HeadSize, nSize));

            if (nSize > BODYSIZE + HeadSize)
                throw new Exception(string.Format("입력된 데이터 사이즈 값이 최대 버퍼 사이즈 보다 큽니다. (버퍼 사이즈 : {0} / {1})", BODYSIZE + HeadSize, nSize));

            m_stGeneralPacket = new GeneralPacket();
            m_stGeneralPacket.Body = new byte[BODYSIZE];

            m_stGeneralPacket.CommandType = BitConverter.ToInt32(aData, 0);
            m_stGeneralPacket.BodySize = BitConverter.ToInt32(aData, 4);

            m_bNetworkOrderConversion = bNetworkOrderConversion;
            if (m_bNetworkOrderConversion)
            {
                m_stGeneralPacket.CommandType = IPAddress.NetworkToHostOrder(m_stGeneralPacket.CommandType);
                m_stGeneralPacket.BodySize = IPAddress.NetworkToHostOrder(m_stGeneralPacket.BodySize);
            }

            if (nSize != m_stGeneralPacket.BodySize + HeadSize)
                throw new Exception(string.Format("입력된 데이터 사이즈 값과 실제 바디 사이즈가 다릅니다. 큽니다. (사이즈 : {0} / {1})", m_stGeneralPacket.BodySize + HeadSize, nSize));

            Array.Copy(aData, 8, m_stGeneralPacket.Body, 0, m_stGeneralPacket.BodySize);
        }

        #endregion

        #region Method

        public byte[] Struct2Bytes()
        {
            if (null == m_stGeneralPacket.Body)
                return null;

            byte[] aData = null;

            if (m_bNetworkOrderConversion)
            {
                int nCommandType = m_stGeneralPacket.CommandType;
                int nBodySize = m_stGeneralPacket.BodySize;

                m_stGeneralPacket.CommandType = IPAddress.HostToNetworkOrder(m_stGeneralPacket.CommandType);
                m_stGeneralPacket.BodySize = IPAddress.HostToNetworkOrder(m_stGeneralPacket.BodySize);

                aData = AKI.Util.ConvertUtil.Object2Byte(m_stGeneralPacket);

                m_stGeneralPacket.CommandType = nCommandType;
                m_stGeneralPacket.BodySize = nBodySize;
            }
            else
                aData = AKI.Util.ConvertUtil.Object2Byte(m_stGeneralPacket);

            return aData;
        }

        public byte[] Struct2ValidBytes()
        {
            byte[] aData = Struct2Bytes();

            if (null == aData)
                return null;

            byte[] aData2 = new byte[FullSize];

            Array.Copy(aData, aData2, aData2.Length);
            return AKI.Util.ConvertUtil.Object2Byte(m_stGeneralPacket);
        }

        public T Xml2Object<T>()
        {
            T xml = default(T);
            try
            {
                AKI.Util.XmlUtil.LoadXml<T>(out xml, this.BodyString);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return xml;
        }

        #endregion
    }
}
