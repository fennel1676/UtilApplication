using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;

namespace WindowsFormsApplication1
{
    #region GeneralPacketXml
    //public class GeneralPacketWrapper
    //{
    //    public const int BODYSIZE = 2040;

    //    #region Struct

    //    public struct GeneralPacket
    //    {
    //        public int CommandType;

    //        public int BodySize;

    //        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GeneralPacketWrapper.BODYSIZE)]
    //        public byte[] Body;
    //    }

    //    #endregion

    //    #region Member Variable

    //    private bool m_bNetworkOrderConversion = false;

    //    #endregion

    //    #region Properties

    //    public bool NetworkOrderConversion { get { return m_bNetworkOrderConversion; } set { m_bNetworkOrderConversion = value; } }

    //    public int CommandType { get { return m_stGeneralPacket.CommandType; } set { m_stGeneralPacket.CommandType = value; } }

    //    public int FullSize { get { return m_stGeneralPacket.BodySize + HeadSize; } }

    //    public int HeadSize { get { return 8; } }

    //    public int BodySize { get { return m_stGeneralPacket.BodySize; } set { m_stGeneralPacket.BodySize = value; } }

    //    public int BufferSize { get { return m_stGeneralPacket.Body.Length + HeadSize; } }

    //    public string BodyString
    //    {
    //        get
    //        {
    //            return Encoding.Unicode.GetString(m_stGeneralPacket.Body, 0, m_stGeneralPacket.BodySize);
    //        }
    //        set
    //        {
    //            byte[] aData = Encoding.Unicode.GetBytes(value);
    //            Array.Copy(aData, m_stGeneralPacket.Body, aData.Length);
    //        }
    //    }

    //    public byte[] BodyByte { get { return m_stGeneralPacket.Body; } set { Array.Copy(value, m_stGeneralPacket.Body, value.Length); } }

    //    #endregion

    //    private GeneralPacket m_stGeneralPacket;

    //    #region Construct

    //    public GeneralPacketWrapper(bool bNetworkOrderConversion = false)
    //    {
    //        m_stGeneralPacket = new GeneralPacket();
    //        m_stGeneralPacket.Body = new byte[BODYSIZE];
    //        m_bNetworkOrderConversion = bNetworkOrderConversion;
    //    }

    //    public GeneralPacketWrapper(int nCommandType, int nBodySize, string strBody, bool bNetworkOrderConversion = false)
    //    {
    //        m_stGeneralPacket = new GeneralPacket();
    //        m_stGeneralPacket.Body = new byte[BODYSIZE];
    //        m_stGeneralPacket.BodySize = nBodySize;
    //        m_bNetworkOrderConversion = bNetworkOrderConversion;

    //        m_stGeneralPacket.CommandType = nCommandType;
    //        byte[] aData = Encoding.Unicode.GetBytes(strBody);
    //        Array.Copy(aData, m_stGeneralPacket.Body, nBodySize);
    //    }

    //    public GeneralPacketWrapper(int nCommandType, string strBody, bool bNetworkOrderConversion = false)
    //    {
    //        m_stGeneralPacket = new GeneralPacket();
    //        m_stGeneralPacket.Body = new byte[BODYSIZE];
    //        m_bNetworkOrderConversion = bNetworkOrderConversion;

    //        m_stGeneralPacket.CommandType = nCommandType;
    //        byte[] aData = Encoding.Unicode.GetBytes(strBody);
    //        m_stGeneralPacket.BodySize = aData.Length;
    //        Array.Copy(aData, m_stGeneralPacket.Body, aData.Length);
    //    }

    //    public GeneralPacketWrapper(byte[] aData, bool bNetworkOrderConversion = false)
    //    {
    //        if (aData.Length < HeadSize)
    //            throw new Exception(string.Format("데이터 사이즈가 최소한의 헤더 사이즈 보다 데이터가 적습니다. (헤더 사이즈: {0} / {1})", HeadSize, aData.Length));

    //        if (aData.Length > BODYSIZE + HeadSize)
    //            throw new Exception(string.Format("입력된 데이터 사이즈 값이 최대 버퍼 사이즈 보다 큽니다. (버퍼 사이즈 : {0} / {1})", BODYSIZE + HeadSize, aData.Length));

    //        m_stGeneralPacket = new GeneralPacket();
    //        m_stGeneralPacket.Body = new byte[BODYSIZE];

    //        m_stGeneralPacket.CommandType = BitConverter.ToInt32(aData, 0);
    //        m_stGeneralPacket.BodySize = BitConverter.ToInt32(aData, 4);

    //        m_bNetworkOrderConversion = bNetworkOrderConversion;
    //        if (m_bNetworkOrderConversion)
    //        {
    //            m_stGeneralPacket.CommandType = IPAddress.NetworkToHostOrder(m_stGeneralPacket.CommandType);
    //            m_stGeneralPacket.BodySize = IPAddress.NetworkToHostOrder(m_stGeneralPacket.BodySize);
    //        }

    //        Array.Copy(aData, 8, m_stGeneralPacket.Body, 0, m_stGeneralPacket.BodySize);
    //    }

    //    public GeneralPacketWrapper(byte[] aData, int nSize, bool bNetworkOrderConversion = false)
    //    {
    //        if (aData.Length < HeadSize)
    //            throw new Exception(string.Format("데이터 사이즈가 최소한의 헤더 사이즈 보다 데이터가 적습니다. (헤더 사이즈: {0} / {1})", HeadSize, aData.Length));

    //        if (aData.Length < nSize)
    //            throw new Exception(string.Format("데이터 사이즈와 입력된 사이즈 값보다 적습니다. (데이터 사이즈: {0} / {1})", aData.Length, nSize));

    //        if (nSize < HeadSize)
    //            throw new Exception(string.Format("입력된 데이터 사이즈 값이 최소 버퍼 사이즈 보다 큽니다. (헤더 사이즈 : {0} / {1})", HeadSize, nSize));

    //        if (nSize > BODYSIZE + HeadSize)
    //            throw new Exception(string.Format("입력된 데이터 사이즈 값이 최대 버퍼 사이즈 보다 큽니다. (버퍼 사이즈 : {0} / {1})", BODYSIZE + HeadSize, nSize));

    //        m_stGeneralPacket = new GeneralPacket();
    //        m_stGeneralPacket.Body = new byte[BODYSIZE];

    //        m_stGeneralPacket.CommandType = BitConverter.ToInt32(aData, 0);
    //        m_stGeneralPacket.BodySize = BitConverter.ToInt32(aData, 4);

    //        m_bNetworkOrderConversion = bNetworkOrderConversion;
    //        if (m_bNetworkOrderConversion)
    //        {
    //            m_stGeneralPacket.CommandType = IPAddress.NetworkToHostOrder(m_stGeneralPacket.CommandType);
    //            m_stGeneralPacket.BodySize = IPAddress.NetworkToHostOrder(m_stGeneralPacket.BodySize);
    //        }

    //        if (nSize != m_stGeneralPacket.BodySize + HeadSize)
    //            throw new Exception(string.Format("입력된 데이터 사이즈 값과 실제 바디 사이즈가 다릅니다. 큽니다. (사이즈 : {0} / {1})", m_stGeneralPacket.BodySize + HeadSize, nSize));

    //        Array.Copy(aData, 8, m_stGeneralPacket.Body, 0, m_stGeneralPacket.BodySize);
    //    }

    //    #endregion

    //    #region Method

    //    public byte[] Struct2Bytes()
    //    {
    //        if (null == m_stGeneralPacket.Body)
    //            return null;

    //        byte[] aData = null;

    //        if (m_bNetworkOrderConversion)
    //        {
    //            int nCommandType = m_stGeneralPacket.CommandType;
    //            int nBodySize = m_stGeneralPacket.BodySize;

    //            m_stGeneralPacket.CommandType = IPAddress.HostToNetworkOrder(m_stGeneralPacket.CommandType);
    //            m_stGeneralPacket.BodySize = IPAddress.HostToNetworkOrder(m_stGeneralPacket.BodySize);

    //            aData = AKI.Util.ConvertUtil.Struct2Byte(m_stGeneralPacket);

    //            m_stGeneralPacket.CommandType = nCommandType;
    //            m_stGeneralPacket.BodySize = nBodySize;
    //        }
    //        else
    //            aData = AKI.Util.ConvertUtil.Struct2Byte(m_stGeneralPacket);

    //        return aData;
    //    }

    //    public byte[] Struct2ValidBytes()
    //    {
    //        byte[] aData = Struct2Bytes();

    //        if (null == aData)
    //            return null;

    //        byte[] aData2 = new byte[FullSize];

    //        Array.Copy(aData, aData2, aData2.Length);
    //        return AKI.Util.ConvertUtil.Struct2Byte(m_stGeneralPacket);
    //    }

    //    #endregion
    //}

    //public class PacketMaker
    //{
    //    private GeneralPacketWrapper.GeneralPacket m_objPacket = new GeneralPacketWrapper.GeneralPacket();


    //}

    public class PacketFactory
    {

    }

    //public class BodyInfo
    //{
    //    [XmlText]
    //    public string Xml;
    //}

    //public class HeadInfo
    //{
    //    [XmlAttribute(AttributeName = "CommandType")]
    //    public int CommandType;

    //    [XmlAttribute(AttributeName = "BodySize")]
    //    public int BodySize;
    //}

    //[XmlRootAttribute("GeneralPacket")]
    //public class GeneralPacketXml
    //{
    //    [XmlElement(ElementName = "HeadInfo")]
    //    public HeadInfo Head = new HeadInfo();
    //    [XmlElement(ElementName = "BodyInfo")]
    //    public BodyInfo Body = new BodyInfo();
    //}

    public class MagicPacketInfo
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name;

        [XmlAttribute(AttributeName = "IP")]
        public string IP;

        [XmlAttribute(AttributeName = "Mac")]
        public string Mac;
    }

    [XmlRootAttribute("MagicPackets")]
    public class MagicPacketSettingXml
    {
        [XmlElement(ElementName = "MagicPacket")]
        public List<MagicPacketInfo> MagicPacket = new List<MagicPacketInfo>();
    }


    [XmlRootAttribute("MagicPackets")]
    public class MagicPacketSettingXml2 : MagicPacketSettingXml
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name;

        [XmlAttribute(AttributeName = "IP")]
        public string IP;

        [XmlAttribute(AttributeName = "Mac")]
        public string Mac;
    }
    #endregion


    public class SettingData
    {
        private static string pathForAttractionControlProgram;
        public static string PATH_FOR_ATTRACTION_CONTROL_PROGRAM { get { return pathForAttractionControlProgram; } set { pathForAttractionControlProgram = value; } }

        private static VideoDataList videoDataList;
        public static VideoDataList VideoDataList { get { return videoDataList; } set { videoDataList = value; } }

        private static BoughtDataList boughtDataList;
        public static BoughtDataList BoughtDataList { get { return boughtDataList; } set { boughtDataList = value; } }

        private static string memberCount = "1";
        public static string MEMBER_COUNT { get { return memberCount; } set { memberCount = value; } }

        private static string memberCount_online = "1";
        public static string MEMBER_COUNT_ONLINE { get { return memberCount_online; } set { memberCount_online = value; } }

        private static bool isActivedStartButton = false;
        public static bool IsActivedStartButton { get { return isActivedStartButton; } set { isActivedStartButton = value; } }

        private static int delayTimeForDeviceLiving;
        public static int DELAY_TIME_FOR_DEVICE_LIVING { get { return delayTimeForDeviceLiving; } set { delayTimeForDeviceLiving = value; } }

        private static string delayTimeForStartVideoAboutDevice;
        public static string DELAY_TIME_FOR_START_VIDEO_ABOUT_DEVICE { get { return "3000"; } set { delayTimeForStartVideoAboutDevice = value; } }
    }

    [System.Serializable]
    public class VideoDataList
    {
        public int totalCnt;
        public List<VideoData> lists;

        public VideoDataList() : base()
        {
            this.lists = new List<VideoData>();
            this.totalCnt = -1;
        }

        [System.Serializable]
        public class VideoData
        {
            public string index;
            public string uid;
            public string owner_uid;
            public string video_name;
            public string video_title;
            public string video_desc;
            public string video_category;
            public string video_level;
            public string video_playtime;
            public string video_path;
            public string video_thumbnail;
            public string video_type;
            public string motion_name;
            public string motion_accept;
            public string price;
            public string price_default;
            public string tax_price;
            public string is_vrsound;
            public string is_active;
            public string create_at;
            public string update_at;
            public string delete_at;
            public string machine_uid;
            public string playPrice;
            public string isUpdate;
            public string cpTax;
            public string apTax;
            public string resellerTax;
            public string adminTax;
            public bool isBuy;

            public VideoData() : base()
            {
                index = "";
                uid = "";
                owner_uid = "";
                video_name = "";
                video_title = "";
                video_desc = "";
                video_category = "";
                video_level = "";
                video_playtime = "";
                video_path = "";
                video_thumbnail = "";
                video_type = "";
                motion_name = "";
                motion_accept = "";
                price = "";
                price_default = "";
                tax_price = "";
                is_active = "";
                create_at = "";
                update_at = "";
                delete_at = "";
                machine_uid = "";
                playPrice = "";
                isUpdate = "";
                cpTax = "";
                apTax = "";
                resellerTax = "";
                adminTax = "";
                isBuy = false;
            }
        }
    }

    public class BoughtDataList
    {
        public int totalCnt;
        public List<BoughtData> lists;

        public BoughtDataList() : base()
        {
            this.lists = new List<BoughtData>();
            this.totalCnt = -1;
        }

        [System.Serializable]
        public class BoughtData
        {
            public string index;
            public string uid;
            public string owner_uid;
            public string video_name;
            public string video_title;
            public string video_desc;
            public string video_category;
            public string video_level;
            public string video_playtime;
            public string video_path;
            public string video_thumbnail;
            public string video_type;
            public string motion_name;
            public string motion_accept;
            public string price;
            public string price_default;
            public string tax_price;
            public string is_vrsound;
            public string is_active;
            public string create_at;
            public string update_at;
            public string delete_at;
            public string machine_uid;
            public string playPrice;

            public string isHoliday;
            public string is_holiday_dc;
            public string holiday_price;
            public string none_holiday_price;

            public string isUpdate;
            public string cpTax;
            public string apTax;
            public string resellerTax;
            public string adminTax;

            public BoughtData() : base()
            {
                index = "";
                uid = "";
                owner_uid = "";
                video_name = "";
                video_title = "";
                video_desc = "";
                video_category = "";
                video_level = "";
                video_playtime = "";
                video_path = "";
                video_thumbnail = "";
                video_type = "";
                motion_name = "";
                motion_accept = "";
                price = "";
                price_default = "";
                tax_price = "";
                is_vrsound = "";
                is_active = "";
                create_at = "";
                update_at = "";
                delete_at = "";
                machine_uid = "";
                playPrice = "";

                isHoliday = "";
                is_holiday_dc = "";
                holiday_price = "";
                none_holiday_price = "";

                isUpdate = "";
                cpTax = "";
                apTax = "";
                resellerTax = "";
                adminTax = "";
            }
        }
    }

    public class PostInfo
    {
        [XmlElement(ElementName = "Key")]
        public string Key;

        [XmlElement(ElementName = "Url")]
        public string Url;
    }

    [XmlRootAttribute("DataServerCommunication")]
    public class DataServerCommunicationXml
    {
        [XmlElement(ElementName = "PostInfo")]
        public List<PostInfo> PostInfos = new List<PostInfo>();
    }

    public class ContentDto
    {
        public string resultCode { get; set; }
        public string contentsCount { get; set; }
        public string processResultCode { get; set; }
        public string processResultMsg { get; set; }
        public Content[] contents { get; set; }
    }

    public class Content
    {
        public string contentId { get; set; }
        public string ftpPath { get; set; }
        public string contentFileName { get; set; }
        public string contentName { get; set; }
        public string deviceSerial { get; set; }
        public string contentVersion { get; set; }
        public string contentVersionName { get; set; }
        public string contentVersionDate { get; set; }
    }

    public class TicketInfoDto
    {
        public TicketInfo[] resultList { get; set; }
    }

    public class TicketInfo
    {
        public string tbBandCode { get; set; }
        public int tbChageCnt { get; set; }
        public int tbUsesCnt { get; set; }
        public string tbTrsdate { get; set; }
        public string m2Cd { get; set; }
        public string m2Name { get; set; }
        public string taAreaCode { get; set; }
        public string taAreaName { get; set; }
    }

    public class TicketInfoDto2
    {
        public TicketInfo2[] result { get; set; }
        public TicketLog2[] log { get; set; }
        public string resultCode;
        public string resultMsg;
        public TicketStatus2[] status { get; set; }
    }

    public class TicketInfo2
    {
        public string nptId { get; set; }
        public string bandNo { get; set; }
        public string trsNo { get; set; }
        public string itemCd { get; set; }
        public string nptType { get; set; }
        public string etc { get; set; }
        public string regDt { get; set; }
        public string refundDt { get; set; }
        public string itemName { get; set; }
    }

    public class TicketLog2
    {
        public string BARCODE_NUMBER { get; set; }
        public string RESULT_CODE { get; set; }
        public string REQUEST_TYPE_CODE { get; set; }
        public string REQUEST_DATE { get; set; }
        public string VRD_SERIAL { get; set; }
    }

    public class TicketStatus2
    {
        public string availableCount { get; set; }
        public string vrdCategoryName { get; set; }
        public string state { get; set; }
        public string vrdCategoryId { get; set; }
    }

    public class GetOnOffHistoryDto
    {
        public GetOnOffHistory[] resultList { get; set; }
    }

    public class GetOnOffHistory
    {
        public string requestDate { get; set; }
        public string kskName { get; set; }
    }

    public class ㅁㅊ_Loginout
    {
        public ㅁㅊ_Response_Loginout response { get; set; }
    }

    public class ㅁㅊ_ValidateTicket
    {
        public ㅁㅊ_Response response { get; set; }
    }

    public class ㅁㅊ_Log
    {
        public ㅁㅊ_Response_Log response { get; set; }
    }

    public class ㅁㅊ_TicketInfo
    {
        public ㅁㅊ_Response_TicketInfo response { get; set; }
    }

    public class ㅁㅊ_GetOnOff
    {
        public ㅁㅊ_Response response { get; set; }
    }

    public class ㅁㅊ_GetContentsOnOffStop
    {
        public ㅁㅊ_Response_GetContentsOnOffStop response { get; set; }
    }

    public class ㅁㅊ_Response
    {
        public ㅁㅊ_Header header { get; set; }

        public ㅁㅊ_Body body { get; set; }
    }

    public class ㅁㅊ_Response_Loginout
    {
        public ㅁㅊ_Header header { get; set; }

        public ㅁㅊ_Body_Response body { get; set; }
    }

    public class ㅁㅊ_Response_Log
    {
        public ㅁㅊ_Header header { get; set; }

        public ㅁㅊ_Body_Response body { get; set; }
    }

    public class ㅁㅊ_Response_GetContentsOnOffStop
    {
        public ㅁㅊ_Header header { get; set; }

        public ㅁㅊ_Body_Response body { get; set; }
    }

    public class ㅁㅊ_Header
    {
        public string statusCode { get; set; }
        public string statusMessage { get; set; }
    }

    public class ㅁㅊ_Body
    {
        public ㅁㅊ_Body_Response response { get; set; }
    }

    public class ㅁㅊ_Body_Response
    {
        public string resultCode { get; set; }
        public string resultMsg { get; set; }
    }

    public class ㅁㅊ_Body_Response_TicketInfo
    {
        public ㅁㅊ_Body_BandInfo bandInfo { get; set; }

        public ㅁㅊ_Body_ChargeList[] chargeList { get; set; }

        public ㅁㅊ_Body_BandOnOffList[] bandOnOffList { get; set; }

        public string amount { get; set; }

        public string refund { get; set; }

        public ㅁㅊ_Body_PosTradeItemList[] posTradeItemList { get; set; }
    }

    public class ㅁㅊ_Response_TicketInfo
    {
        public ㅁㅊ_Header header { get; set; }

        public TicketInfoDto3 body { get; set; }
    }

    public class TicketInfoDto3
    {
        public ㅁㅊ_Body_Response_TicketInfo response { get; set; }
    }

    public class ㅁㅊ_Body_BandInfo
    {
        public string bandId { get; set; }
        public string barcodeNumber { get; set; }
        public string createTime { get; set; }
        public string updateTime { get; set; }
        public string itemCount { get; set; }
        public string posTradeId { get; set; }
        public string bandCategoryCode { get; set; }
        public string[] bandCharges { get; set; }
        public string refundYn { get; set; }
    }

    public class ㅁㅊ_Body_ChargeList
    {
        public string posTradeId { get; set; }
        public string bandId { get; set; }
        public string vrdCategoryId { get; set; }
        public string chargeCount { get; set; }
        public string availableCount { get; set; }
        public string createTime { get; set; }
        public string barcodeNumber { get; set; }
        public string vrdCategoryName { get; set; }
        public string itemType { get; set; }
        public string availableTime { get; set; }
        public string totalCnt { get; set; }
        public string totalTime { get; set; }
        public string usedCount { get; set; }
        public string itemcategoryList { get; set; }
    }

    public class ㅁㅊ_Body_BandOnOffList
    {
        public string id { get; set; }
        public string vrdSerial { get; set; }
        public string barcodeNumber { get; set; }
        public string requestDate { get; set; }
        public string resultCode { get; set; }
        public string requestTypeCode { get; set; }
        public string itemType { get; set; }
    }

    public class ㅁㅊ_Body_PosTradeItemList
    {
        public string bandId { get; set; }
        public string posTradeId { get; set; }
        public string itemId { get; set; }
        public string itemCount { get; set; }
        public string itemPrice { get; set; }
        public string createTime { get; set; }
        public string itemName { get; set; }
    }

}
