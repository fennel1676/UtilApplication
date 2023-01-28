using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AvSimDll.MotionExternC
{
    public partial class MotionControl
    {
        #region Initial
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__Initial();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__Initial_Port(int nPort, int nTimeout);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__Initial_Sock(string szRemoteIP, int nPort, int nTimeout);
        #endregion

        #region Destroy
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__Destroy();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__Destroy_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__Destroy_Sock();
        #endregion

        #region State
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__State();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__State_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__State_Sock();
        #endregion

        #region Ping
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__Ping();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__Ping_Port(int nPort, bool bResp);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__Ping_Sock(bool bResp);
        #endregion

        #region DO
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__DO(int nDo);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__DO_Port(int nPort, int nDo);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__DO_Sock(int nDo);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int MotionControl__GetDO();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int MotionControl__GetDO_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int MotionControl__GetDO_Sock();
        #endregion

        #region DI
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int MotionControl__GetDI();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int MotionControl__GetDI_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int MotionControl__GetDI_Sock();
        #endregion

        #region DOF_and_Blower
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__DOF_and_Blower(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__DOF_and_Blower_Port(int nPort, int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__DOF_and_Blower_Sock(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower);
        #endregion

        #region DOF_and_Blower_and_DO
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__DOF_and_Blower_and_DO(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower, int nDO);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__DOF_and_Blower_and_DO_Port(int nPort, int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower, int nDO);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__DOF_and_Blower_and_DO_Sock(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower, int nDO);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__EQExtendData(IntPtr pEQData, bool bResp);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__EQExtendData_Port(int nPort, IntPtr pEQData, bool bResp);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControl__EQExtendData_Sock(IntPtr pEQData, bool bResp);
        #endregion

        #region V2__DOF_and_Blower
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControlV2__DOF_and_Blower(MOTION_DATA stMotionData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControlV2__DOF_and_Blower_Port(int nPort, MOTION_DATA stMotionData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControlV2__DOF_and_Blower_Sock(MOTION_DATA stMotionData);
        #endregion

        #region V2__DOF_and_Blower_and_DO
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControlV2__DOF_and_Blower_and_DO(MOTION_DATA stMotionData, int nDO);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControlV2__DOF_and_Blower_and_DO_Port(int nPort, MOTION_DATA stMotionData, int nDO);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControlV2__DOF_and_Blower_and_DO_Sock(MOTION_DATA stMotionData, int nDO);
        #endregion

        #region V2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis(MOTION_EXTEND_DATA stMotionData, IntPtr pEQData, bool bResp);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_Port(int nPort, MOTION_EXTEND_DATA stMotionData, IntPtr pEQData, bool bResp);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_Sock(MOTION_EXTEND_DATA stMotionData, IntPtr pEQData, bool bResp);

        #endregion

        #region V2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm(MOTION_EXTEND_DATA stMotionData, IntPtr pEQData, bool bResp);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm_Port(int nPort, MOTION_EXTEND_DATA stMotionData, IntPtr pEQData, bool bResp);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm_Sock(MOTION_EXTEND_DATA stMotionData, IntPtr pEQData, bool bResp);
        #endregion


        #region Socket Parameter Setting Function
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadTcpSocketMode(IntPtr pData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadTcpSocketMode_Port(int nPort, IntPtr pData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadTcpSocketMode_Sock(IntPtr pData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteTcpSocketMode(int nData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteTcpSocketMode_Port(int nPort, int nData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteTcpSocketMode_Sock(int nData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadWifiMode(IntPtr pData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadWifiMode_Port(int nPort, IntPtr pData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadWifiMode_Sock(IntPtr pData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteWifiMode(int nData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteWifiMode_Port(int nPort, int nData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteWifiMode_Sock(int nData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadWifiChannel(IntPtr pData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadWifiChannel_Port(int nPort, IntPtr pData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadWifiChannel_Sock(IntPtr pData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteWifiChannel(int nData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteWifiChannel_Port(int nPort, int nData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteWifiChannel_Sock(int nData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadBluetoothDeviceId(StringBuilder szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadBluetoothDeviceId_Port(int nPort, StringBuilder szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadBluetoothDeviceId_Sock(StringBuilder szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteBluetoothDeviceId(string szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteBluetoothDeviceId_Port(int nPort, string szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteBluetoothDeviceId_Sock(string szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadBluetoothDevicePassword(StringBuilder szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadBluetoothDevicePassword_Port(int nPort, StringBuilder szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadBluetoothDevicePassword_Sock(StringBuilder szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteBluetoothDevicePassword(string szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteBluetoothDevicePassword_Port(int nPort, string szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteBluetoothDevicePassword_Sock(string szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketServerIP(IntPtr szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketServerIP_Port(int nPort, IntPtr szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketServerIP_Sock(IntPtr szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketServerIP(byte[] szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketServerIP_Port(int nPort, byte[] szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketServerIP_Sock(byte[] szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketServerSubnetMask(IntPtr szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketServerSubnetMask_Port(int nPort, IntPtr szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketServerSubnetMask_Sock(IntPtr szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketServerSubnetMask(byte[] szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketServerSubnetMask_Port(int nPort, byte[] szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketServerSubnetMask_Sock(byte[] szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketServerGateway(IntPtr szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketServerGateway_Port(int nPort, IntPtr szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketServerGateway_Sock(IntPtr szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketServerGateway(byte[] szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketServerGateway_Port(int nPort, byte[] szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketServerGateway_Sock(byte[] szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketServerDNS(IntPtr szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketServerDNS_Port(int nPort, IntPtr szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketServerDNS_Sock(IntPtr szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketServerDNS(byte[] szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketServerDNS_Port(int nPort, byte[] szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketServerDNS_Sock(byte[] szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketServerPort(IntPtr pData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketServerPort_Port(int nPort, IntPtr pData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketServerPort_Sock(IntPtr pData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketServerPort(int nData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketServerPort_Port(int nPort, int nData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketServerPort_Sock(int nData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketClientIP(IntPtr szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketClientIP_Port(int nPort, IntPtr szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketClientIP_Sock(IntPtr szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketClientIP(byte[] szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketClientIP_Port(int nPort, byte[] szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketClientIP_Sock(byte[] szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketClientSubnetMask(IntPtr szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketClientSubnetMask_Port(int nPort, IntPtr szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketClientSubnetMask_Sock(IntPtr szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketClientSubnetMask(byte[] szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketClientSubnetMask_Port(int nPort, byte[] szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketClientSubnetMask_Sock(byte[] szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketClientGateway(IntPtr szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketClientGateway_Port(int nPort, IntPtr szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketClientGateway_Sock(IntPtr szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketClientGateway(byte[] szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketClientGateway_Port(int nPort, byte[] szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketClientGateway_Sock(byte[] szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketClientDNS(IntPtr szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketClientDNS_Port(int nPort, IntPtr szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool ReadSocketClientDNS_Sock(IntPtr szData);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketClientDNS(byte[] szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketClientDNS_Port(int nPort, byte[] szData);
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static bool WriteSocketClientDNS_Sock(byte[] szData);
        #endregion
    }
}
