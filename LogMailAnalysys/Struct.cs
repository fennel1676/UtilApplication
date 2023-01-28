using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogMailAnalysys
{

    public class sStart
    {
        public TimeSpan StartTime;
        public TimeSpan EndTime;
        public string CountryCode;
        public string ZoneName;

        public List<sContents> Contents = new List<sContents>();

        public bool Analysys(ref int nIndex, string[] aLog, string[] aLine)
        {
            string line = aLog[nIndex];
            string[] aLineLast = null;

            StartTime = TimeSpan.Parse(aLine[0]);

            #region sting Parsing
            for (int n = 3; n < aLine.Length; n++)
            {
                if ("Country" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        if ("Zone" == aLine[m] && "=" == aLine[m + 1])
                            break;

                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    CountryCode = sb.ToString().TrimEnd(' ');
                }
                else if ("Zone" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    ZoneName = sb.ToString().TrimEnd(' ');
                }
            }
            #endregion

            aLineLast = aLine;
            nIndex++;

            string strCountryCode = string.Empty;
            string strZoneName = string.Empty;
            string strLine = string.Empty;
            for (int i = nIndex; i < aLog.Length; i++, nIndex++)
            {
                line = aLog[i];
                strLine = line.Replace("[", "");
                strLine = strLine.Replace("]", "");
                strLine = strLine.Replace(",", "");
                aLine = strLine.Split(' ');

                #region sting Parsing
                for (int n = 3; n < aLine.Length; n++)
                {
                    if ("Country" == aLine[n])
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int m = n + 2; m < aLine.Length; m++)
                        {
                            if ("Zone" == aLine[m] && "=" == aLine[m + 1])
                                break;

                            sb.AppendFormat("{0} ", aLine[m]);
                        }
                        strCountryCode = sb.ToString().TrimEnd(' ');
                    }
                    else if ("Zone" == aLine[n])
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int m = n + 2; m < aLine.Length; m++)
                        {
                            if ("Content" == aLine[m] && "=" == aLine[m + 1])
                                break;

                            sb.AppendFormat("{0} ", aLine[m]);
                        }
                        strZoneName = sb.ToString().TrimEnd(' ');
                    }
                }
                #endregion

                if ("PNI-Select" == aLine[2])
                {
                    if (CountryCode == strCountryCode && ZoneName == strZoneName)
                    {
                        sContents content = new sContents();
                        Contents.Add(content);
                        if (content.Analysys(ref nIndex, aLog, aLine))
                        {
                            i = nIndex;
                            continue;
                        }
                        else
                            break;
                    }
                    else
                    {
                        nIndex--;
                        return true;
                    }
                }
                else if ("PNI-Exit" == aLine[2])
                {
                    EndTime = TimeSpan.Parse(aLine[0]);
                    nIndex--;
                    return true;
                }
                else
                {
                    EndTime = TimeSpan.Parse(aLineLast[0]);
                    nIndex--;
                    return true;
                }
            }
            return false;
        }
    }

    public class sContents
    {
        public string CountryCode;
        public string ZoneName;
        public TimeSpan SelectTime;
        public string SelectContentName;
        public List<sPlay> Play = new List<sPlay>();

        public bool Analysys(ref int nIndex, string[] aLog, string[] aLine)
        {
            string[] aLineLast = null;

            SelectTime = TimeSpan.Parse(aLine[0]);

            #region string parsing
            for (int n = 3; n < aLine.Length; n++)
            {
                if ("Country" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        if ("Zone" == aLine[m] && "=" == aLine[m + 1])
                            break;

                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    CountryCode = sb.ToString().TrimEnd(' ');
                }
                else if ("Zone" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        if ("Content" == aLine[m] && "=" == aLine[m + 1])
                            break;

                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    ZoneName = sb.ToString().TrimEnd(' ');
                }
                else if ("Content" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    SelectContentName = sb.ToString().TrimEnd(' ');
                }
            }
            #endregion

            aLineLast = aLine;
            nIndex++;

            string strLine = string.Empty;
            string strCountryCode = string.Empty;
            string strZoneName = string.Empty;
            string strSelectContentName = string.Empty;
            string strPlayCode = string.Empty;
            for (int i = nIndex; i < aLog.Length; i++, nIndex++)
            {
                strLine = aLog[i].Replace("[", "");
                strLine = strLine.Replace("]", "");
                strLine = strLine.Replace(",", "");
                aLine = strLine.Split(' ');

                #region string parsing
                for (int n = 3; n < aLine.Length; n++)
                {
                    if ("Country" == aLine[n])
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int m = n + 2; m < aLine.Length; m++)
                        {
                            if ("Zone" == aLine[m] && "=" == aLine[m + 1])
                                break;

                            sb.AppendFormat("{0} ", aLine[m]);
                        }
                        strCountryCode = sb.ToString().TrimEnd(' ');
                    }
                    else if ("Zone" == aLine[n])
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int m = n + 2; m < aLine.Length; m++)
                        {
                            if (("Contents" == aLine[m] || "PlayCode" == aLine[m]) && "=" == aLine[m + 1])
                                break;

                            sb.AppendFormat("{0} ", aLine[m]);
                        }
                        strZoneName = sb.ToString().TrimEnd(' ');
                    }
                    else if ("Content" == aLine[n] || "Contents" == aLine[n])
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int m = n + 2; m < aLine.Length; m++)
                        {
                            if ("PlayCode" == aLine[m] && "=" == aLine[m + 1])
                                break;

                            sb.AppendFormat("{0} ", aLine[m]);
                        }
                        strSelectContentName = sb.ToString().TrimEnd(' ');
                    }
                    else if ("PlayCode" == aLine[n])
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int m = n + 2; m < aLine.Length; m++)
                        {
                            if ("PlayResult" == aLine[m] && "=" == aLine[m + 1])
                                break;

                            sb.AppendFormat("{0} ", aLine[m]);
                        }
                        strPlayCode = sb.ToString().TrimEnd(' ');
                    }
                }
                #endregion

                if ("PNI-GetPlay" == aLine[2])
                {
                    if (CountryCode == strCountryCode && ZoneName == strZoneName && SelectContentName == strSelectContentName)
                    {
                        sPlay play = new sPlay();
                        Play.Add(play);
                        if (play.AnalysysPlay(ref nIndex, aLog, aLine))
                        {
                            i = nIndex;
                            continue;
                        }
                        else
                            break;
                    }
                    else
                    {
                        nIndex--;
                        return true;
                    }
                }
                else if ("PNI-GetStop" == aLine[2])
                {
                    if (CountryCode == strCountryCode && ZoneName == strZoneName)
                    {
                        int nPlayIndex = -1;
                        for (int j = Play.Count - 1; j >= 0; j--)
                        {
                            if (Play[j].PlayCode == strPlayCode)
                            {
                                nPlayIndex = j;
                                break;
                            }
                        }
                        if (-1 == nPlayIndex)
                            return true;

                        sPlay play = Play[nPlayIndex];
                        if (play.AnalysysStop(ref nIndex, aLog, aLine))
                        {
                            i = nIndex;
                            continue;
                        }
                        else
                            break;
                    }
                    else
                    {
                        nIndex--;
                        return true;
                    }
                }
                else
                {
                    nIndex--;
                    return true;
                }
            }
            return false;
        }
    }
    
    public class sPlay
    {
        public string CountryCode;
        public string ZoneName;
        public TimeSpan PlayTime;
        public TimeSpan StopTime;
        public string PlayCode;
        public string Result;
        public List<sSimulator> Simulator = new List<sSimulator>();

        public bool AnalysysPlay(ref int nIndex, string[] aLog, string[] aLine)
        {
            string[] aLineLast = null;

            PlayTime = TimeSpan.Parse(aLine[0]);

            #region string parsing
            for (int n = 3; n < aLine.Length; n++)
            {
                if ("Country" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        if ("Zone" == aLine[m] && "=" == aLine[m + 1])
                            break;

                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    CountryCode = sb.ToString().TrimEnd(' ');
                }
                else if ("Zone" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        if (("Contents" == aLine[m] || "Content" == aLine[m]) && "=" == aLine[m + 1])
                            break;

                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    ZoneName = sb.ToString().TrimEnd(' ');
                }
                else if ("PlayCode" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    PlayCode = sb.ToString().TrimEnd(' ');
                }
            }
            #endregion

            aLineLast = aLine;
            nIndex++;

            string strLine = string.Empty;
            string strCountryCode = string.Empty;
            string strZoneName = string.Empty;
            string strPlayCode = string.Empty;
            string strSimulator = string.Empty;
            string strPort = string.Empty;
            for (int i = nIndex; i < aLog.Length; i++, nIndex++)
            {
                strLine = aLog[i].Replace("[", "");
                strLine = strLine.Replace("]", "");
                strLine = strLine.Replace(",", "");
                aLine = strLine.Split(' ');

                #region string parsing
                for (int n = 3; n < aLine.Length; n++)
                {
                    if ("Country" == aLine[n])
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int m = n + 2; m < aLine.Length; m++)
                        {
                            if ("Zone" == aLine[m] && "=" == aLine[m + 1])
                                break;

                            sb.AppendFormat("{0} ", aLine[m]);
                        }
                        strCountryCode = sb.ToString().TrimEnd(' ');
                    }
                    else if ("Zone" == aLine[n])
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int m = n + 2; m < aLine.Length; m++)
                        {
                            if ("Simulator" == aLine[m] && "=" == aLine[m + 1])
                                break;

                            sb.AppendFormat("{0} ", aLine[m]);
                        }
                        strZoneName = sb.ToString().TrimEnd(' ');
                    }
                    else if ("PlayCode" == aLine[n])
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int m = n + 2; m < aLine.Length; m++)
                        {
                            sb.AppendFormat("{0} ", aLine[m]);
                        }
                        strPlayCode = sb.ToString().TrimEnd(' ');
                    }
                    else if ("Simulator" == aLine[n])
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int m = n + 2; m < aLine.Length; m++)
                        {
                            if ("DeviceID" == aLine[m] && "=" == aLine[m + 1])
                                break;

                            sb.AppendFormat("{0} ", aLine[m]);
                        }
                        strSimulator = sb.ToString().TrimEnd(' ');
                    }
                    else if ("Port" == aLine[n])
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int m = n + 2; m < aLine.Length; m++)
                        {
                            if ("Status" == aLine[m] && "=" == aLine[m + 1])
                                break;

                            sb.AppendFormat("{0} ", aLine[m]);
                        }
                        strPort = sb.ToString().TrimEnd(' ');
                    }
                }
                #endregion

                if ("PNI-GetOn" == aLine[2])
                {
                    if (CountryCode == strCountryCode && ZoneName == strZoneName && PlayCode == strPlayCode)
                    {
                        int nSimulatorIndex = -1;
                        for (int j = Simulator.Count - 1; j >= 0; j--)
                        {
                            if (Simulator[j].Name == strSimulator && Simulator[j].Port == strPort)
                            {
                                nSimulatorIndex = j;
                                break;
                            }
                        }
                        sSimulator simulator = null;
                        if (-1 == nSimulatorIndex)
                        {
                            simulator = new sSimulator();
                            Simulator.Add(simulator);
                        }
                        else
                            simulator = Simulator[nSimulatorIndex];
                        
                        simulator.AnalysysGetOn(ref nIndex, aLog, aLine);
                    }
                    else
                    {
                        nIndex--;
                        return true;
                    }
                }
                else if ("PNI-GetOff" == aLine[2])
                {
                    if (CountryCode == strCountryCode && ZoneName == strZoneName && PlayCode == strPlayCode)
                    {
                        int nSimulatorIndex = -1;
                        for (int j = Simulator.Count - 1; j >= 0; j--)
                        {
                            if (Simulator[j].Name == strSimulator && Simulator[j].Port == strPort)
                            {
                                nSimulatorIndex = j;
                                break;
                            }
                        }
                        if (-1 == nSimulatorIndex)
                            return true;

                        sSimulator simulator = Simulator[nSimulatorIndex];
                        simulator.AnalysysGetOff(ref nIndex, aLog, aLine);
                    }
                    else
                    {
                        nIndex--;
                        return true;
                    }
                }
                else if ("PNI-GetCompulsionOff" == aLine[2])
                {
                    if (CountryCode == strCountryCode && ZoneName == strZoneName && PlayCode == strPlayCode)
                    {
                        int nSimulatorIndex = -1;
                        for (int j = Simulator.Count - 1; j >= 0; j--)
                        {
                            if (Simulator[j].Name == strSimulator && Simulator[j].Port == strPort)
                            {
                                nSimulatorIndex = j;
                                break;
                            }
                        }
                        if (-1 == nSimulatorIndex)
                            return true;

                        sSimulator simulator = Simulator[nSimulatorIndex];
                        simulator.AnalysysGetCompulsionOff(ref nIndex, aLog, aLine);
                        i = nIndex;
                    }
                    else
                    {
                        nIndex--;
                        return true;
                    }
                }
                else
                {
                    nIndex--;
                    return true;
                }
            }
            return false;
        }

        public bool AnalysysStop(ref int nIndex, string[] aLog, string[] aLine)
        {
            string strLine = string.Empty;
            string strCountryCode = string.Empty;
            string strZoneName = string.Empty;
            string strPlayCode = string.Empty;
            string strResult = string.Empty;

            #region string Pasrsing
            for (int n = 3; n < aLine.Length; n++)
            {
                if ("Country" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        if ("Zone" == aLine[m] && "=" == aLine[m + 1])
                            break;

                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    strCountryCode = sb.ToString().TrimEnd(' ');
                }
                else if ("Zone" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        if ("PlayCode" == aLine[m] && "=" == aLine[m + 1])
                            break;

                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    strZoneName = sb.ToString().TrimEnd(' ');
                }
                else if ("PlayCode" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        if ("PlayResult" == aLine[m] && "=" == aLine[m + 1])
                            break;

                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    strPlayCode = sb.ToString().TrimEnd(' ');
                }
                else if ("PlayResult" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    strResult = sb.ToString().TrimEnd(' ');
                }
            }
            #endregion

            if (CountryCode == strCountryCode && ZoneName == strZoneName && PlayCode == strPlayCode)
            {
                StopTime = TimeSpan.Parse(aLine[0]);
                Result = strResult;
            }
            else
            {
                strLine = aLog[nIndex - 1].Replace("[", "");
                strLine = strLine.Replace("]", "");
                strLine = strLine.Replace(",", "");
                aLine = strLine.Split(' ');

                StopTime = TimeSpan.Parse(aLine[0]);
                Result = "-1";
            }

            return true;
        }
    }

    public class sSimulator
    {
        public TimeSpan GetOnTime;
        public TimeSpan GetOffTime;
        public TimeSpan GetCompulsionOffTime;
        public string Name;
        public string Port;
        
        public List<sDevice> Devices = new List<sDevice>();

        public bool AnalysysGetOn(ref int nIndex, string[] aLog, string[] aLine)
        {
            GetOnTime = TimeSpan.Parse(aLine[0]);

            sDevice device = new sDevice();
            Devices.Add(device);

            for (int n = 3; n < aLine.Length; n++)
            {
                if ("Simulator" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        if ("DeviceID" == aLine[m] && "=" == aLine[m + 1])
                            break;

                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    Name = sb.ToString().TrimEnd(' ');
                }
                else if ("Port" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        if ("Status" == aLine[m] && "=" == aLine[m + 1])
                            break;

                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    Port = sb.ToString().TrimEnd(' ');
                }
                else if ("DeviceID" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        if ("Port" == aLine[m] && "=" == aLine[m + 1])
                            break;

                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    device.ID = sb.ToString().TrimEnd(' ');
                }
                else if ("Status" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        if ("PlayCode" == aLine[m] && "=" == aLine[m + 1])
                            break;

                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    device.GetOnStatus = sb.ToString().TrimEnd(' ');
                }
            }

            return true;
        }

        public bool AnalysysGetOff(ref int nIndex, string[] aLog, string[] aLine)
        {
            GetOffTime = TimeSpan.Parse(aLine[0]);

            string strDeviceID = string.Empty;
            string strStatus = string.Empty;
            for (int n = 3; n < aLine.Length; n++)
            {
                if ("DeviceID" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        if ("Port" == aLine[m] && "=" == aLine[m + 1])
                            break;

                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    strDeviceID = sb.ToString().TrimEnd(' ');
                }
                else if ("Status" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        if ("PlayCode" == aLine[m] && "=" == aLine[m + 1])
                            break;

                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    strStatus = sb.ToString().TrimEnd(' ');
                }
            }

            for (int i = 0; i < Devices.Count; i++)
            {
                if (Devices[i].ID == strDeviceID)
                {
                    Devices[i].GetOffStatus = strStatus;
                }
            }
            return true;
        }

        public bool AnalysysGetCompulsionOff(ref int nIndex, string[] aLog, string[] aLine)
        {
            GetCompulsionOffTime = TimeSpan.Parse(aLine[0]);

            string strDeviceID = string.Empty;
            string strStatus = string.Empty;
            for (int n = 3; n < aLine.Length; n++)
            {
                if ("DeviceID" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        if ("Port" == aLine[m] && "=" == aLine[m + 1])
                            break;

                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    strDeviceID = sb.ToString().TrimEnd(' ');
                }
                else if ("Status" == aLine[n])
                {
                    StringBuilder sb = new StringBuilder();
                    for (int m = n + 2; m < aLine.Length; m++)
                    {
                        if ("PlayCode" == aLine[m] && "=" == aLine[m + 1])
                            break;

                        sb.AppendFormat("{0} ", aLine[m]);
                    }
                    strStatus = sb.ToString().TrimEnd(' ');
                }
            }

            for (int i = 0; i < Devices.Count; i++)
            {
                if (Devices[i].ID == strDeviceID)
                {
                    Devices[i].GetCompulsionOffStatus = strStatus;
                }
            }

            return true;
        }
    }


    public class sDevice
    {
        public string ID;
        public string GetOnStatus;
        public string GetOffStatus;
        public string GetCompulsionOffStatus;
    }
}


