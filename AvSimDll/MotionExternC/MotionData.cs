using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AvSimDll.MotionExternC
{
    public enum eOutput : Int32
    {
        eRoll = 0,
        ePitch = 1,
        eYaw = 2,
        eSway = 3,
        eSurge = 4,
        eHeave = 5,
        eRollEx = 6,
        ePitchEx = 7,
        eYawEx = 8,
        eMax = 9,
    };

    public enum eAxisOutput : Int32
    {
        eAxis1 = 0,
        eAxis2 = 1,
        eAxis3 = 2,
        eAxis4 = 3,
        eAxis5 = 4,
        eAxis6 = 5,
        eAxisEx7 = 6,
        eAxisEx8 = 7,
        eAxisEx9 = 8,
        eAxisMax = 9,
    };

    public class Define
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        // AvSim DOF Scale Data Define
        public static int AV_SIM__ACTION_VALUE_MIN = 0;                      // AvSim Scale 최소치
        public static int AV_SIM__ACTION_VALUE_CENTER = 10000;               // AvSim Scale 중간치
        public static int AV_SIM__ACTION_VALUE_MAX = 20000;                  // AvSim Scale 최대치
        /////////////////////////////////////////////////////////////////////////////////////////////


        /////////////////////////////////////////////////////////////////////////////////////////////
        // AvSim Circling Scale Data Define
        public static int AV_SIM__ACTION_CIRCLING_VALUE_MIN = 0;             // AvSim Scale 최소치
        public static int AV_SIM__ACTION_CIRCLING_VALUE_CENTER = 18000;      // AvSim Scale 중간치
        public static int AV_SIM__ACTION_CIRCLING_VALUE_MAX = 36000;         // AvSim Scale 최대치
        /////////////////////////////////////////////////////////////////////////////////////////////

        public static int MOTOR_DRIVE_POLING_CHANNEL_MAX = 9;

        public static int AV_SIM__VELOCITY_SCALE_CALCULATE_ZERO = 0;
        public static int AV_SIM__VELOCITY_SCALE_CALCULATE_SPAN = 100;

        public static int AV_SIM__MOTION_SPEED_SCALE_CALCULATE_ZERO = 1;
        public static int AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SPAN = 255;
        public static int AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE = 30;

        public static int AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_ZERO = 3000;
        public static int AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SPAN = 0;
        public static int AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SAFE = 200;

        public static int AV_SIM__CIRCLING_MODE_DEFAULT = 0;
        public static int AV_SIM__CIRCLING_MODE_STOP = 1;
        public static int AV_SIM__CIRCLING_MODE_CW = 2;
        public static int AV_SIM__CIRCLING_MODE_CCW = 3;

        public static int AV_SIM__RESPONSE_DISABLE = 0;
        public static int AV_SIM__RESPONSE_ENABLE = 1;

    }

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
        public int Roll = Define.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int Pitch = Define.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int Yaw = Define.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int Sway = Define.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int Surge = Define.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int Heave = Define.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int MotionSpeed = Define.AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SAFE;

        [MarshalAs(UnmanagedType.I4)]
        public int Blower = Define.AV_SIM__VELOCITY_SCALE_CALCULATE_ZERO;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MOTION_EXTEND_DATA : MOTION_DATA
    {
        [MarshalAs(UnmanagedType.I4)]
        public int Rolling = Define.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int RollingSpeed = Define.AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SAFE;

        [MarshalAs(UnmanagedType.I4)]
        public int RollingMode = Define.AV_SIM__CIRCLING_MODE_DEFAULT;

        [MarshalAs(UnmanagedType.I4)]
        public int Pitching = Define.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int PitchingSpeed = Define.AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SAFE;

        [MarshalAs(UnmanagedType.I4)]
        public int PitchingMode = Define.AV_SIM__CIRCLING_MODE_DEFAULT;

        [MarshalAs(UnmanagedType.I4)]
        public int Yawing = Define.AV_SIM__ACTION_VALUE_CENTER;

        [MarshalAs(UnmanagedType.I4)]
        public int YawingSpeed = Define.AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SAFE;

        [MarshalAs(UnmanagedType.I4)]
        public int YawingMode = Define.AV_SIM__CIRCLING_MODE_DEFAULT;
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

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)(eAxisOutput.eAxisMax + 1) * 2)]
        //public byte[,] AxisAlarm = new byte[(int)(eAxisOutput.eAxisMax + 1), 2];
    }

}
