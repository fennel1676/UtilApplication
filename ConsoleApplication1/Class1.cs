using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ConsoleApplication1
{
    #region Response Command Xmls

    public class ResponseStatus_Head
    {
        [XmlAttribute(AttributeName = "ID")]
        public int ID;


        [XmlAttribute(AttributeName = "CommandType")]
        public int CommandType = -1;
    }

    public class ResponseContentsStatus_Body
    {
        [XmlAttribute(AttributeName = "RespStatus")]
        public int RespStatus;

        [XmlAttribute(AttributeName = "VrMode")]
        public int VrMode;

        [XmlAttribute(AttributeName = "ApplicationName")]
        public string ApplicationName;

        [XmlAttribute(AttributeName = "ContentsName")]
        public string ContentsName;
    }

    public class ResponseSimulatorStatus_Body
    {
        [XmlAttribute(AttributeName = "MotionSpeed")]
        public int MotionSpeed;

        [XmlAttribute(AttributeName = "Di")]
        public int Di;

        [XmlAttribute(AttributeName = "Do")]
        public int Do;

        [XmlAttribute(AttributeName = "BoardingStatus")]
        public int BoardingStatus;

        [XmlAttribute(AttributeName = "CheckingStatus")]
        public int CheckingStatus;
    }

    public class ResponseStatus_Alarm
    {
        [XmlIgnore]
        public bool CheckAlarm = true;

        [XmlAttribute(AttributeName = "AlarmType")]
        public int AlarmType;

        [XmlAttribute(AttributeName = "AlarmCode")]
        public string AlarmCode;

        public ResponseStatus_Alarm() { }

        public ResponseStatus_Alarm(ResponseStatus_Alarm alram)
        {
            this.AlarmType = alram.AlarmType;
            this.AlarmCode = alram.AlarmCode;
        }
        public ResponseStatus_Alarm(int nAlarmType, string strAlarmCode)
        {
            this.AlarmType = nAlarmType;
            this.AlarmCode = strAlarmCode;
        }

    }

    public class AlarmStatus : ResponseStatus_Alarm
    {
        public bool CheckAlarm = true;

        public AlarmStatus() { }

        public AlarmStatus(ResponseStatus_Alarm alram, bool checkAlarm)
        {
            this.AlarmType = alram.AlarmType;
            this.AlarmCode = alram.AlarmCode;
            this.CheckAlarm = checkAlarm;
        }
    }

    public class ResponseStatus_Alarms
    {
        [XmlElement(ElementName = "Head")]
        public ResponseStatus_Head Head;

        [XmlElement(ElementName = "Alarm")]
        public List<ResponseStatus_Alarm> Alarm = new List<ResponseStatus_Alarm>();
    }

    [XmlRootAttribute("ResponseContentsStatus")]
    public class ResponseContentsStatus_SW : ResponseStatus_Alarms
    {
        [XmlElement(ElementName = "Body")]
        public ResponseContentsStatus_Body Body;
    }

    [XmlRootAttribute("ResponseContentsStatus")]
    public class ResponseSimulatorStatus_HW : ResponseStatus_Alarms
    {
        [XmlElement(ElementName = "Body")]
        public ResponseSimulatorStatus_Body Body;
    }

    #endregion
}
