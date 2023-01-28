
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LogAnalyzer
{
    #region Xml

    public class Runs
    {
        [XmlElement(ElementName = "Run")]
        public List<Run> Run = new List<LogAnalyzer.Run>();
    }

    public class Run
    {
        [XmlAttribute(AttributeName = "StartTime")]
        public string StartTime;

        [XmlAttribute(AttributeName = "ExitTime")]
        public string ExitTime;

        [XmlAttribute(AttributeName = "Country")]
        public string Country;

        [XmlAttribute(AttributeName = "Zone")]
        public string Zone;

        [XmlElement(ElementName = "Selects")]
        public Selects Selects;
    }

    public class Selects
    {
        [XmlElement(ElementName = "Select")]
        public List<Select> Select = new List<LogAnalyzer.Select>();
    }

    public class Select
    {
        [XmlAttribute(AttributeName = "Time")]
        public string Time;

        [XmlAttribute(AttributeName = "Content")]
        public string Content;

        [XmlElement(ElementName = "Plays")]
        public Plays Plays;
    }

    public class Plays
    {
        [XmlElement(ElementName = "Play")]
        public List<Play> Play = new List<LogAnalyzer.Play>();
    }

    public class Play
    {
        [XmlAttribute(AttributeName = "Time")]
        public string Time;

        [XmlAttribute(AttributeName = "Content")]
        public string Content;

        [XmlAttribute(AttributeName = "PlayCode")]
        public string PlayCode;

        [XmlAttribute(AttributeName = "PlayResult")]
        public string PlayResult;

        [XmlElement(ElementName = "Simulators")]
        public Simulators Simulators;
    }

    public class Simulators
    {
        [XmlElement(ElementName = "Simulator")]
        public List<Simulator> Simulator = new List<LogAnalyzer.Simulator>();
    }

    public class Simulator
    {
        [XmlAttribute(AttributeName = "ID")]
        public string SimulatorID;


        [XmlElement(ElementName = "Devices")]
        public Devices Devices;
    }

    public class Devices
    {
        [XmlElement(ElementName = "Device")]
        public List<Device> Device = new List<LogAnalyzer.Device>();
    }

    public class Device
    {
        [XmlAttribute(AttributeName = "ID")]
        public string SimulatorID;

        [XmlAttribute(AttributeName = "GetOnTime")]
        public string GetOnTime;

        [XmlAttribute(AttributeName = "GetOffTime")]
        public string GetOffTime;

        [XmlAttribute(AttributeName = "Status")]
        public string Status;
    }


    [XmlRootAttribute("LogAnalyzer")]
    public class LogAnalyzerXml
    {
        [XmlElement(ElementName = "Runs")]
        public Runs Runs;
    }

    #endregion
}
