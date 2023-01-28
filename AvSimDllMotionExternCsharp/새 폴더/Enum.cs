using System;

namespace AvSimDllMotionExternCsharp
{
    //  OS
    public enum eOS : int
    {
        PC_Soket = 0,
        PC_Serial = 4,
        Android_Socket = 1,
        Android_Serial = 3,
        IOS = 2,        
    }

    //  ComPort
    public enum eComPort : int
    {
        COM1 = 0,
        COM2 = 1,
        COM3 = 2,
        COM4 = 3,
        COM5 = 4,
        COM6 = 5,
        COM7 = 6,
        COM8 = 7,
        COM9 = 8,
        COM10 = 9,
    }

    // 통신 데이터 플래그들(시작, 끝, 실패, 요청등의 플래그값)
    public enum eCommFlag : int
    {
        STX = 0x02,
        ETX = 0x03,
        EOT = 0x04,
        ENQ = 0x05,
        ACK = 0x06,
        LF = 0x0a,
        CL = 0x0c,
        CR = 0x0d,
        DLE = 0x10,
        NAK = 0x15,
    }

    // 통신시 에러 상태 값 디파인
    public enum eState : int
    {
        ACK = 2,
        SUCCESS = 1,
        NONE = 0,
        ERROR = -1,
        TIMEOUT = -2,
        CHECKSUM_ERR = -4,
        CRC_ERR = -8,
        NAK = -16,
        PORT_OPEN_ERR = 0xC000,			// COM Port를 오픈하지 못했을경우의 에러값
    }

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

    public enum eDefine : int
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        // AvSim DOF Scale Data Define
        AV_SIM__ACTION_VALUE_MIN = 0,                      // AvSim Scale 최소치
        AV_SIM__ACTION_VALUE_CENTER = 10000,               // AvSim Scale 중간치
        AV_SIM__ACTION_VALUE_MAX = 20000,                  // AvSim Scale 최대치
        /////////////////////////////////////////////////////////////////////////////////////////////


        /////////////////////////////////////////////////////////////////////////////////////////////
        // AvSim Circling Scale Data Define
        AV_SIM__ACTION_CIRCLING_VALUE_MIN = 0,             // AvSim Scale 최소치
        AV_SIM__ACTION_CIRCLING_VALUE_CENTER = 18000,      // AvSim Scale 중간치
        AV_SIM__ACTION_CIRCLING_VALUE_MAX = 36000,         // AvSim Scale 최대치
        /////////////////////////////////////////////////////////////////////////////////////////////

        MOTOR_DRIVE_POLING_CHANNEL_MAX = 9,

        AV_SIM__BLOW_SCALE_CALCULATE_ZERO = 0,
        AV_SIM__BLOW_SCALE_CALCULATE_SPAN = 100,

        AV_SIM__MOTION_SPEED_SCALE_CALCULATE_ZERO = 1,
        AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SPAN = 255,
        AV_SIM__MOTION_SPEED_SCALE_CALCULATE_SAFE = 30,

        AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_ZERO = 3000,
        AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SPAN = 0,
        AV_SIM__MOTION_RPM_SPEED_SCALE_CALCULATE_SAFE = 200,

        AV_SIM__CIRCLING_MODE_DEFAULT = 0,
        AV_SIM__CIRCLING_MODE_STOP = 1,
        AV_SIM__CIRCLING_MODE_CW = 2,
        AV_SIM__CIRCLING_MODE_CCW = 3,

        AV_SIM__RESPONSE_DISABLE = 0,
        AV_SIM__RESPONSE_ENABLE = 1,

    }
}
