
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;

namespace AvSimDllMotionExternCsharp
{
    #region Communication
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MOTOR_ERROR_DATA
    {
        [MarshalAs(UnmanagedType.U1)]
        public byte MainErrCode;

        [MarshalAs(UnmanagedType.U1)]
        public byte SubErrCode;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MOTION_DATA
    {
        [MarshalAs(UnmanagedType.I4)]
        public int Roll = (int)eDefine.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int Pitch = (int)eDefine.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int Yaw = (int)eDefine.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int Sway = (int)eDefine.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int Surge = (int)eDefine.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int Heave = (int)eDefine.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int MotionSpeed = (int)eDefine.AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SAFE;

        [MarshalAs(UnmanagedType.I4)]
        public int Blower = (int)eDefine.AV_SIM__BLOW_SCALE_CALCULATE_ZERO;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MOTION_EXTEND_DATA : MOTION_DATA
    {
        [MarshalAs(UnmanagedType.I4)]
        public int Rolling = (int)eDefine.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int RollingSpeed = (int)eDefine.AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SAFE;

        [MarshalAs(UnmanagedType.I4)]
        public int RollingMode = (int)eDefine.AV_SIM__CIRCLING_MODE_DEFAULT;

        [MarshalAs(UnmanagedType.I4)]
        public int Pitching = (int)eDefine.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int PitchingSpeed = (int)eDefine.AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SAFE;

        [MarshalAs(UnmanagedType.I4)]
        public int PitchingMode = (int)eDefine.AV_SIM__CIRCLING_MODE_DEFAULT;

        [MarshalAs(UnmanagedType.I4)]
        public int Yawing = (int)eDefine.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int YawingSpeed = (int)eDefine.AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SAFE;

        [MarshalAs(UnmanagedType.I4)]
        public int YawingMode = (int)eDefine.AV_SIM__CIRCLING_MODE_DEFAULT;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class EQUIPMENT_DATA
    {
        [MarshalAs(UnmanagedType.I4)]
        public int DO = 0;

        [MarshalAs(UnmanagedType.I4)]
        public int DI = 0;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)eAxisOutput.eAxisMax)]
        public int[] SrcAxisPos = new int[(int)eAxisOutput.eAxisMax];

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)eAxisOutput.eAxisMax)]
        public int[] DstAxisPos = new int[(int)eAxisOutput.eAxisMax];

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)eAxisOutput.eAxisMax)]
        public int[] EcdAxisPos = new int[(int)eAxisOutput.eAxisMax];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class EQUIPMENT_EXTEND_DATA
    {
        [MarshalAs(UnmanagedType.I4)]
        public int DO = 0;

        [MarshalAs(UnmanagedType.I4)]
        public int DI = 0;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)eAxisOutput.eAxisMax)]
        public int[] AxisPos = new int[(int)eAxisOutput.eAxisMax];

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)(eAxisOutput.eAxisMax + 1))]
        public MOTOR_ERROR_DATA[] mAxisAlarm = new MOTOR_ERROR_DATA[(int)(eAxisOutput.eAxisMax + 1)];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AXIS_DATA
    {
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 mAxis;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 mAxisSpeed;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MOTION_AXIS_DATA
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=((int)(eAxisOutput.eAxisEx9) * 8))]
        public AXIS_DATA[] mAxisData = new AXIS_DATA[(int)eAxisOutput.eAxisEx9];

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 mBlower = (UInt32)eDefine.AV_SIM__BLOW_SCALE_CALCULATE_ZERO;

        public MOTION_AXIS_DATA()
        {
            mAxisData[(int)eAxisOutput.eAxis1].mAxis = (UInt32)eDefine.AV_SIM__ACTION_VALUE_CENTER;
            mAxisData[(int)eAxisOutput.eAxis1].mAxisSpeed = (UInt32)eDefine.AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE;
            mAxisData[(int)eAxisOutput.eAxis2].mAxis = (UInt32)eDefine.AV_SIM__ACTION_VALUE_CENTER;
            mAxisData[(int)eAxisOutput.eAxis2].mAxisSpeed = (UInt32)eDefine.AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE;
            mAxisData[(int)eAxisOutput.eAxis3].mAxis = (UInt32)eDefine.AV_SIM__ACTION_VALUE_CENTER;
            mAxisData[(int)eAxisOutput.eAxis3].mAxisSpeed = (UInt32)eDefine.AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE;
            mAxisData[(int)eAxisOutput.eAxis4].mAxis = (UInt32)eDefine.AV_SIM__ACTION_VALUE_CENTER;
            mAxisData[(int)eAxisOutput.eAxis4].mAxisSpeed = (UInt32)eDefine.AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE;
            mAxisData[(int)eAxisOutput.eAxis5].mAxis = (UInt32)eDefine.AV_SIM__ACTION_VALUE_CENTER;
            mAxisData[(int)eAxisOutput.eAxis5].mAxisSpeed = (UInt32)eDefine.AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE;
            mAxisData[(int)eAxisOutput.eAxis6].mAxis = (UInt32)eDefine.AV_SIM__ACTION_VALUE_CENTER;
            mAxisData[(int)eAxisOutput.eAxis6].mAxisSpeed = (UInt32)eDefine.AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE;
            mAxisData[(int)eAxisOutput.eAxisEx7].mAxis = (UInt32)eDefine.AV_SIM__ACTION_CIRCLING_VALUE_CENTER;
            mAxisData[(int)eAxisOutput.eAxisEx7].mAxisSpeed = (UInt32)eDefine.AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE;
            mAxisData[(int)eAxisOutput.eAxisEx8].mAxis = (UInt32)eDefine.AV_SIM__ACTION_CIRCLING_VALUE_CENTER;
            mAxisData[(int)eAxisOutput.eAxisEx8].mAxisSpeed = (UInt32)eDefine.AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE;
            mBlower = (UInt32)eDefine.AV_SIM__BLOW_SCALE_CALCULATE_ZERO;
        }
    }

    #endregion

    #region Xml

    public class CommunicationType
    {
        [XmlAttribute(AttributeName = "TYPE")]
        public int Type = 0;
    }

    public class ResponseTimeout
    {
        [XmlAttribute(AttributeName = "LIMIT_TIME")]
        public int LimitTime = 0;
    }

    public class ComPort
    {
        [XmlAttribute(AttributeName = "COM_PORT")]
        public int Port = 0;
    }

    public class UdpParameter
    {
        [XmlAttribute(AttributeName = "IP")]
        public string IP = "127.0.0.1";

        [XmlAttribute(AttributeName = "PORT")]
        public int Port = 4000;

        [XmlAttribute(AttributeName = "KEEPALIVE")]
        public bool KeepAlive = false;
    }

    public class OsType
    {
        [XmlAttribute(AttributeName = "Type")]
        public eOS Type = eOS.Android_Socket;
    }

    [XmlRootAttribute("ENVIRONMENT_PARAMETER")]
    public class EnvironmentParameterXml
    {
        [XmlElement(ElementName = "COMMUNICATION_TYPE")]
        public CommunicationType CommunicationType;

        [XmlElement(ElementName = "RESPONSE_TIMEOUT")]
        public ResponseTimeout ResponseTimeout;

        [XmlElement(ElementName = "COM_PORT")]
        public ComPort ComPort;

        [XmlElement(ElementName = "UDP_PARAMETER")]
        public UdpParameter UdpParameter;

        [XmlElement(ElementName = "OS_TYPE")]
        public OsType OsType;
    }

    #endregion
}
