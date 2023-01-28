using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AvSimDll.MotionExternC
{
    public partial class MotionControl
    {
        private IntPtr m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis = IntPtr.Zero;
        private IntPtr m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm = IntPtr.Zero;
        private IntPtr m_ptrEQExtendData = IntPtr.Zero;

        public string RemoteIP { get; set; }
        public int Port { get; set; }
        public int Timeout { get; set; }

        private bool m_checkInitial = false;

        public bool Initial(int nPort = -1, int nTimeout = 300)
        {
            RemoteIP = string.Empty;
            Port = nPort;
            Timeout = nTimeout;

            bool isResult = true;
            if (!m_checkInitial)
            {
                if (-1 == nPort)
                    isResult = MotionControl__Initial();
                else
                    isResult = MotionControl__Initial_Port(Port, nTimeout);

                if (isResult)
                    m_checkInitial = true;
            }

            return isResult;
        }

        public bool Initial(string strRemoteIP, int nPort, int nTimeout = 300)
        {
            RemoteIP = strRemoteIP;
            Port = nPort;
            Timeout = nTimeout;

            bool isResult = true;
            if (!m_checkInitial)
            {
                isResult = MotionControl__Initial_Sock(strRemoteIP, nPort, nTimeout);

                if (isResult)
                    m_checkInitial = true;
            }

            return isResult;
        }

        public bool Destroy()
        {
            bool isResult = true;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = MotionControl__Destroy();
                    else
                        isResult = MotionControl__Destroy_Port(Port);
                }
                else
                {
                    isResult = MotionControl__Destroy_Sock();
                }

                if (isResult)
                {
                    m_checkInitial = false;
                    RemoteIP = string.Empty;
                    Port = -1;
                    Timeout = 300;

                    if (IntPtr.Zero != m_ptrEQExtendData)
                    {
                        Marshal.FreeHGlobal(m_ptrEQExtendData);
                        m_ptrEQExtendData = IntPtr.Zero;
                    }
                    
                    if (IntPtr.Zero != m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis)
                    {
                        Marshal.FreeHGlobal(m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis);
                        m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis = IntPtr.Zero;
                    }

                    if (IntPtr.Zero != m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm)
                    {
                        Marshal.FreeHGlobal(m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm);
                        m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm = IntPtr.Zero;
                    }
                }
            }
            return isResult;
        }

        public bool State()
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = MotionControl__State();
                    else
                        isResult = MotionControl__State_Port(Port);
                }
                else
                {
                    isResult = MotionControl__State_Sock();
                }
            }
            return isResult;
        }

        public bool State(int nPort)
        {
            return MotionControl__State_Port(Port);
        }

        public bool Ping(bool bResp = true)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = MotionControl__Ping();
                    else
                        isResult = MotionControl__Ping_Port(Port, bResp);
                }
                else
                {
                    isResult = MotionControl__Ping_Sock(bResp);
                }
            }
            return isResult;
        }

        public bool WriteDO(int nDO)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = MotionControl__DO(nDO);
                    else
                        isResult = MotionControl__DO_Port(Port, nDO);
                }
                else
                {
                    isResult = MotionControl__DO_Sock(nDO);
                }
            }
            return isResult;
        }

        public int ReadDO()
        {
            int nResult = 0;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        nResult = MotionControl__GetDO();
                    else
                        nResult = MotionControl__GetDO_Port(Port);
                }
                else
                {
                    nResult = MotionControl__GetDO_Sock();
                }
            }
            return nResult;
        }

        public int ReadDI()
        {
            int nResult = 0;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        nResult = MotionControl__GetDI();
                    else
                        nResult = MotionControl__GetDI_Port(Port);
                }
                else
                {
                    nResult = MotionControl__GetDI_Sock();
                }
            }
            return nResult;
        }

        public bool DOF_and_Blower(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = MotionControl__DOF_and_Blower(nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nBlower);
                    else
                        isResult = MotionControl__DOF_and_Blower_Port(Port, nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nBlower);
                }
                else
                {
                    isResult = MotionControl__DOF_and_Blower_Sock(nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nBlower);
                }
            }
            return isResult;
        }

        public bool DOF_and_Blower_and_DO(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower, int nDO)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = MotionControl__DOF_and_Blower_and_DO(nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nBlower, nDO);
                    else
                        isResult = MotionControl__DOF_and_Blower_and_DO_Port(Port, nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nBlower, nDO);
                }
                else
                {
                    isResult = MotionControl__DOF_and_Blower_and_DO_Sock(nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nBlower, nDO);
                }
            }
            return isResult;
        }

        public bool EQExtendData(out EQUIPMENT_EXTEND_DATA objEQData, bool bResp = true)
        {
            bool isResult = false;
            objEQData = null;
            if (m_checkInitial)
            {
                try
                {
                    if (IntPtr.Zero == m_ptrEQExtendData)
                        m_ptrEQExtendData = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(EQUIPMENT_EXTEND_DATA)));

                    if (string.Empty == RemoteIP)
                    {
                        if (-1 == Port)
                            isResult = MotionControl__EQExtendData(m_ptrEQExtendData, bResp);
                        else
                            isResult = MotionControl__EQExtendData_Port(Port, m_ptrEQExtendData, bResp);
                    }
                    else
                    {
                        isResult = MotionControl__EQExtendData_Sock(m_ptrEQExtendData, bResp);
                    }

                    if (isResult)
                        objEQData = (EQUIPMENT_EXTEND_DATA)Marshal.PtrToStructure(m_ptrEQExtendData, typeof(EQUIPMENT_EXTEND_DATA));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return isResult;
        }

        public bool V2_DOF_and_Blower(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                MOTION_DATA data = new MOTION_DATA();
                data.Roll = nRoll;
                data.Pitch = nPitch;
                data.Yaw = nYaw;
                data.Sway = nSway;
                data.Surge = nSurge;
                data.Heave = nHeave;
                data.MotionSpeed = nSpeed;
                data.Blower = nBlower;

                isResult = V2_DOF_and_Blower(data);
            }
            return isResult;
        }

        public bool V2_DOF_and_Blower(MOTION_DATA data)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = MotionControlV2__DOF_and_Blower(data);
                    else
                        isResult = MotionControlV2__DOF_and_Blower_Port(Port, data);
                }
                else
                {
                    isResult = MotionControlV2__DOF_and_Blower_Sock(data);
                }
            }
            return isResult;
        }

        public bool V2_DOF_and_Blower_and_DO(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower, int nDo)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                MOTION_DATA data = new MOTION_DATA();
                data.Roll = nRoll;
                data.Pitch = nPitch;
                data.Yaw = nYaw;
                data.Sway = nSway;
                data.Surge = nSurge;
                data.Heave = nHeave;
                data.MotionSpeed = nSpeed;
                data.Blower = nBlower;

                isResult = V2_DOF_and_Blower_and_DO(data, nDo);
            }
            return isResult;
        }

        public bool V2_DOF_and_Blower_and_DO(MOTION_DATA data, int nDo)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = MotionControlV2__DOF_and_Blower_and_DO(data, nDo);
                    else
                        isResult = MotionControlV2__DOF_and_Blower_and_DO_Port(Port, data, nDo);
                }
                else
                {
                    isResult = MotionControlV2__DOF_and_Blower_and_DO_Sock(data, nDo);
                }
            }
            return isResult;
        }

        public bool V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis(
            int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower,
            int nRolling, int nRollingSpeed, int nRollingMode,
            int nPitching, int nPitchingSpeed, int nPitchingMode,
            int nYawing, int nYawingSpeed, int nYawingMode,
            out EQUIPMENT_DATA eqData, bool bResp = true)
        {
            eqData = null;
            bool isResult = false;
            if (m_checkInitial)
            {
                MOTION_EXTEND_DATA data = new MOTION_EXTEND_DATA();
                data.Roll = nRoll;
                data.Pitch = nPitch;
                data.Yaw = nYaw;
                data.Sway = nSway;
                data.Surge = nSurge;
                data.Heave = nHeave;
                data.MotionSpeed = nSpeed;
                data.Blower = nBlower;
                data.Rolling = nRolling;
                data.RollingSpeed = nRollingSpeed;
                data.RollingMode = nRollingMode;
                data.Pitching = nPitching;
                data.PitchingSpeed = nPitchingSpeed;
                data.PitchingMode = nPitchingMode;
                data.Yawing = nYawing;
                data.YawingSpeed = nYawingSpeed;
                data.YawingMode = nYawingMode;

                isResult = V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis(data, out eqData, bResp);
            }
            return isResult;
        }

        public bool V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis(MOTION_EXTEND_DATA data, out EQUIPMENT_DATA eqData, bool bResp = true)
        {
            eqData = null;
            bool isResult = false;
            if (m_checkInitial)
            {   
                if (IntPtr.Zero == m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis)
                    m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(EQUIPMENT_DATA)));

                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis(data, m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis, bResp);
                    else
                        isResult = MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_Port(Port, data, m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis, bResp);
                }
                else
                {
                    isResult = MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_Sock(data, m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis, bResp);
                }

                if (isResult)
                    eqData = (EQUIPMENT_DATA)Marshal.PtrToStructure(m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis, typeof(EQUIPMENT_DATA));
            }
            return isResult;
        }

        public bool V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm(
            int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower,
            int nRolling, int nRollingSpeed, int nRollingMode,
            int nPitching, int nPitchingSpeed, int nPitchingMode,
            int nYawing, int nYawingSpeed, int nYawingMode,
            out EQUIPMENT_EXTEND_DATA eqData, bool bResp = true)
        {
            eqData = null;
            bool isResult = false;
            if (m_checkInitial)
            {
                MOTION_EXTEND_DATA data = new MOTION_EXTEND_DATA();
                data.Roll = nRoll;
                data.Pitch = nPitch;
                data.Yaw = nYaw;
                data.Sway = nSway;
                data.Surge = nSurge;
                data.Heave = nHeave;
                data.MotionSpeed = nSpeed;
                data.Blower = nBlower;
                data.Rolling = nRolling;
                data.RollingSpeed = nRollingSpeed;
                data.RollingMode = nRollingMode;
                data.Pitching = nPitching;
                data.PitchingSpeed = nPitchingSpeed;
                data.PitchingMode = nPitchingMode;
                data.Yawing = nYawing;
                data.YawingSpeed = nYawingSpeed;
                data.YawingMode = nYawingMode;

                isResult = V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm(data, out eqData, bResp);
            }
            return isResult;
        }

        public bool V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm(MOTION_EXTEND_DATA data, out EQUIPMENT_EXTEND_DATA eqData, bool bResp = true)
        {
            eqData = null;
            bool isResult = false;
            if (m_checkInitial)
            {
                if (IntPtr.Zero == m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm)
                    m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(EQUIPMENT_EXTEND_DATA)));

                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm(data, m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm, bResp);
                    else
                        isResult = MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm_Port(Port, data, m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm, bResp);
                }
                else
                {
                    isResult = MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm_Sock(data, m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm, bResp);
                }

                if (isResult)
                    eqData = (EQUIPMENT_EXTEND_DATA)Marshal.PtrToStructure(m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm, typeof(EQUIPMENT_EXTEND_DATA));
            }
            return isResult;
        }
        
        public bool Wifi_ReadTcpSocketMode(ref int nData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                IntPtr pResult = Marshal.AllocCoTaskMem(4);
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = ReadTcpSocketMode(pResult);
                    else
                        isResult = ReadTcpSocketMode_Port(Port, pResult);
                }
                else
                {
                    isResult = ReadTcpSocketMode_Sock(pResult);
                }

                if (isResult)
                {
                    int[] aResult = new int[1];
                    Marshal.Copy(pResult, aResult, 0, 1);
                    Marshal.FreeHGlobal(pResult);
                    nData = (int)aResult[0];
                }
            }
            return isResult;
        }

        public bool Wifi_WriteTcpSocketMode(int nData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteTcpSocketMode(nData);
                    else
                        isResult = WriteTcpSocketMode_Port(Port, nData);
                }
                else
                {
                    isResult = WriteTcpSocketMode_Sock(nData);
                }
            }
            return isResult;
        }

        public bool Wifi_ReadWifiMode(ref int nData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                IntPtr pResult = Marshal.AllocCoTaskMem(4);
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = ReadWifiMode(pResult);
                    else
                        isResult = ReadWifiMode_Port(Port, pResult);
                }
                else
                {
                    isResult = ReadWifiMode_Sock(pResult);
                }

                if (isResult)
                {
                    int[] aResult = new int[1];
                    Marshal.Copy(pResult, aResult, 0, 1);
                    Marshal.FreeHGlobal(pResult);
                    nData = (int)aResult[0];
                }
            }
            return isResult;
        }

        public bool Wifi_WriteWifiMode(int nData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteWifiMode(nData);
                    else
                        isResult = WriteWifiMode_Port(Port, nData);
                }
                else
                {
                    isResult = WriteWifiMode_Sock(nData);
                }
            }
            return isResult;
        }

        public bool Wifi_ReadWifiChannel(ref int nData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                IntPtr pResult = Marshal.AllocCoTaskMem(4);
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = ReadWifiChannel(pResult);
                    else
                        isResult = ReadWifiChannel_Port(Port, pResult);
                }
                else
                {
                    isResult = ReadWifiChannel_Sock(pResult);
                }

                if (isResult)
                {
                    int[] aResult = new int[1];
                    Marshal.Copy(pResult, aResult, 0, 1);
                    Marshal.FreeHGlobal(pResult);
                    nData = (int)aResult[0];
                }
            }
            return isResult;
        }

        public bool Wifi_WriteWifiChannel(int nData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteWifiChannel(nData);
                    else
                        isResult = WriteWifiChannel_Port(Port, nData);
                }
                else
                {
                    isResult = WriteWifiChannel_Sock(nData);
                }
            }
            return isResult;
        }

        public bool Wifi_ReadBluetoothDeviceId(ref string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                StringBuilder sb = new StringBuilder();
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = ReadBluetoothDeviceId(sb);
                    else
                        isResult = ReadBluetoothDeviceId_Port(Port, sb);
                }
                else
                {
                    isResult = ReadBluetoothDeviceId_Sock(sb);
                }

                if (isResult)
                    szData = sb.ToString();
            }
            return isResult;
        }

        public bool Wifi_WriteBluetoothDeviceId(string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteBluetoothDeviceId(szData);
                    else
                        isResult = WriteBluetoothDeviceId_Port(Port, szData);
                }
                else
                {
                    isResult = WriteBluetoothDeviceId_Sock(szData);
                }
            }
            return isResult;
        }

        public bool Wifi_ReadBluetoothDevicePassword(ref string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                StringBuilder sb = new StringBuilder();
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = ReadBluetoothDevicePassword(sb);
                    else
                        isResult = ReadBluetoothDevicePassword_Port(Port, sb);
                }
                else
                {
                    isResult = ReadBluetoothDevicePassword_Sock(sb);
                }
                if (isResult)
                    szData = sb.ToString();
            }
            return isResult;
        }

        public bool Wifi_WriteBluetoothDevicePassword(string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteBluetoothDevicePassword(szData);
                    else
                        isResult = WriteBluetoothDevicePassword_Port(Port, szData);
                }
                else
                {
                    isResult = WriteBluetoothDevicePassword_Sock(szData);
                }
            }
            return isResult;
        }

        public bool Wifi_ReadSocketServerIP(ref string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                IntPtr pResult = Marshal.AllocCoTaskMem(4);
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = ReadSocketServerIP(pResult);
                    else
                        isResult = ReadSocketServerIP_Port(Port, pResult);
                }
                else
                {
                    isResult = ReadSocketServerIP_Sock(pResult);
                }
                if (isResult)
                {
                    byte[] aResult = new byte[4];
                    Marshal.Copy(pResult, aResult, 0, 4);
                    Marshal.FreeHGlobal(pResult);
                    szData = string.Format("{0}.{1}.{2}.{3}",(int)aResult[0],(int)aResult[1],(int)aResult[2],(int)aResult[3]);
                }
            }
            return isResult;
        }

        public bool Wifi_WriteSocketServerIP(string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                string[] aData = szData.Split('.');
                byte[] aIP = new byte[4];
                for(int i = 0; i < 4; i++)
                {
                    aIP[i] = byte.Parse(aData[i]);
                }

                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteSocketServerIP(aIP);
                    else
                        isResult = WriteSocketServerIP_Port(Port, aIP);
                }
                else
                {
                    isResult = WriteSocketServerIP_Sock(aIP);
                }
            }
            return isResult;
        }

        public bool Wifi_WriteSocketServerIP(byte[] aData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteSocketServerIP(aData);
                    else
                        isResult = WriteSocketServerIP_Port(Port, aData);
                }
                else
                {
                    isResult = WriteSocketServerIP_Sock(aData);
                }
            }
            return isResult;
        }

        public bool Wifi_ReadSocketServerSubnetMask(ref string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                IntPtr pResult = Marshal.AllocCoTaskMem(4);
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = ReadSocketServerSubnetMask(pResult);
                    else
                        isResult = ReadSocketServerSubnetMask_Port(Port, pResult);
                }
                else
                {
                    isResult = ReadSocketServerSubnetMask_Sock(pResult);
                }
                if (isResult)
                {
                    byte[] aResult = new byte[4];
                    Marshal.Copy(pResult, aResult, 0, 4);
                    Marshal.FreeHGlobal(pResult);
                    szData = string.Format("{0}.{1}.{2}.{3}",(int)aResult[0],(int)aResult[1],(int)aResult[2],(int)aResult[3]);
                }
            }
            return isResult;
        }

        public bool Wifi_WriteSocketServerSubnetMask(string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                string[] aData = szData.Split('.');
                byte[] aIP = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    aIP[i] = byte.Parse(aData[i]);
                }

                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteSocketServerSubnetMask(aIP);
                    else
                        isResult = WriteSocketServerSubnetMask_Port(Port, aIP);
                }
                else
                {
                    isResult = WriteSocketServerSubnetMask_Sock(aIP);
                }
            }
            return isResult;
        }

        public bool Wifi_WriteSocketServerSubnetMask(byte[] aData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteSocketServerSubnetMask(aData);
                    else
                        isResult = WriteSocketServerSubnetMask_Port(Port, aData);
                }
                else
                {
                    isResult = WriteSocketServerSubnetMask_Sock(aData);
                }
            }
            return isResult;
        }

        public bool Wifi_ReadSocketServerGateway(ref string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                IntPtr pResult = Marshal.AllocCoTaskMem(4);
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = ReadSocketServerGateway(pResult);
                    else
                        isResult = ReadSocketServerGateway_Port(Port, pResult);
                }
                else
                {
                    isResult = ReadSocketServerGateway_Sock(pResult);
                }
                if (isResult)
                {
                    byte[] aResult = new byte[4];
                    Marshal.Copy(pResult, aResult, 0, 4);
                    Marshal.FreeHGlobal(pResult);
                    szData = string.Format("{0}.{1}.{2}.{3}",(int)aResult[0],(int)aResult[1],(int)aResult[2],(int)aResult[3]);
                }
            }
            return isResult;
        }

        public bool Wifi_WriteSocketServerGateway(string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                string[] aData = szData.Split('.');
                byte[] aIP = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    aIP[i] = byte.Parse(aData[i]);
                }

                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteSocketServerGateway(aIP);
                    else
                        isResult = WriteSocketServerGateway_Port(Port, aIP);
                }
                else
                {
                    isResult = WriteSocketServerGateway_Sock(aIP);
                }
            }
            return isResult;
        }

        public bool Wifi_WriteSocketServerGateway(byte[] aData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteSocketServerGateway(aData);
                    else
                        isResult = WriteSocketServerGateway_Port(Port, aData);
                }
                else
                {
                    isResult = WriteSocketServerGateway_Sock(aData);
                }
            }
            return isResult;
        }

        public bool Wifi_ReadSocketServerDNS(ref string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                IntPtr pResult = Marshal.AllocCoTaskMem(4);
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = ReadSocketServerDNS(pResult);
                    else
                        isResult = ReadSocketServerDNS_Port(Port, pResult);
                }
                else
                {
                    isResult = ReadSocketServerDNS_Sock(pResult);
                }
                if (isResult)
                {
                    byte[] aResult = new byte[4];
                    Marshal.Copy(pResult, aResult, 0, 4);
                    Marshal.FreeHGlobal(pResult);
                    szData = string.Format("{0}.{1}.{2}.{3}",(int)aResult[0],(int)aResult[1],(int)aResult[2],(int)aResult[3]);
                }
            }
            return isResult;
        }

        public bool Wifi_WriteSocketServerDNS(string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                string[] aData = szData.Split('.');
                byte[] aIP = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    aIP[i] = byte.Parse(aData[i]);
                }

                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteSocketServerDNS(aIP);
                    else
                        isResult = WriteSocketServerDNS_Port(Port, aIP);
                }
                else
                {
                    isResult = WriteSocketServerDNS_Sock(aIP);
                }
            }
            return isResult;
        }

        public bool Wifi_WriteSocketServerDNS(byte[] aData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteSocketServerDNS(aData);
                    else
                        isResult = WriteSocketServerDNS_Port(Port, aData);
                }
                else
                {
                    isResult = WriteSocketServerDNS_Sock(aData);
                }
            }
            return isResult;
        }

        public bool Wifi_ReadSocketServerPort(ref int nData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                IntPtr pResult = Marshal.AllocCoTaskMem(4);
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = ReadSocketServerPort(pResult);
                    else
                        isResult = ReadSocketServerPort_Port(Port, pResult);
                }
                else
                {
                    isResult = ReadSocketServerPort_Sock(pResult);
                }

                if (isResult)
                {
                    int[] aResult = new int[1];
                    Marshal.Copy(pResult, aResult, 0, 1);
                    Marshal.FreeHGlobal(pResult);
                    nData = (int)aResult[0];
                }
            }
            return isResult;
        }

        public bool Wifi_WriteSocketServerPort(int nData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteSocketServerPort(nData);
                    else
                        isResult = WriteSocketServerPort_Port(Port, nData);
                }
                else
                {
                    isResult = WriteSocketServerPort_Sock(nData);
                }
            }
            return isResult;
        }

        public bool Wifi_ReadSocketClientIP(ref string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                IntPtr pResult = Marshal.AllocCoTaskMem(4);
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = ReadSocketClientIP(pResult);
                    else
                        isResult = ReadSocketClientIP_Port(Port, pResult);
                }
                else
                {
                    isResult = ReadSocketClientIP_Sock(pResult);
                }
                if (isResult)
                {
                    byte[] aResult = new byte[4];
                    Marshal.Copy(pResult, aResult, 0, 4);
                    Marshal.FreeHGlobal(pResult);
                    szData = string.Format("{0}.{1}.{2}.{3}",(int)aResult[0],(int)aResult[1],(int)aResult[2],(int)aResult[3]);
                }
            }
            return isResult;
        }

        public bool Wifi_WriteSocketClientIP(string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                string[] aData = szData.Split('.');
                byte[] aIP = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    aIP[i] = byte.Parse(aData[i]);
                }

                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteSocketClientIP(aIP);
                    else
                        isResult = WriteSocketClientIP_Port(Port, aIP);
                }
                else
                {
                    isResult = WriteSocketClientIP_Sock(aIP);
                }
            }
            return isResult;
        }

        public bool Wifi_WriteSocketClientIP(byte[] aData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteSocketClientIP(aData);
                    else
                        isResult = WriteSocketClientIP_Port(Port, aData);
                }
                else
                {
                    isResult = WriteSocketClientIP_Sock(aData);
                }
            }
            return isResult;
        }

        public bool Wifi_ReadSocketClientSubnetMask(ref string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                IntPtr pResult = Marshal.AllocCoTaskMem(4);
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = ReadSocketClientSubnetMask(pResult);
                    else
                        isResult = ReadSocketClientSubnetMask_Port(Port, pResult);
                }
                else
                {
                    isResult = ReadSocketClientSubnetMask_Sock(pResult);
                }
                if (isResult)
                {
                    byte[] aResult = new byte[4];
                    Marshal.Copy(pResult, aResult, 0, 4);
                    Marshal.FreeHGlobal(pResult);
                    szData = string.Format("{0}.{1}.{2}.{3}",(int)aResult[0],(int)aResult[1],(int)aResult[2],(int)aResult[3]);
                }
            }
            return isResult;
        }

        public bool Wifi_WriteSocketClientSubnetMask(string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                string[] aData = szData.Split('.');
                byte[] aIP = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    aIP[i] = byte.Parse(aData[i]);
                }

                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteSocketClientSubnetMask(aIP);
                    else
                        isResult = WriteSocketClientSubnetMask_Port(Port, aIP);
                }
                else
                {
                    isResult = WriteSocketClientSubnetMask_Sock(aIP);
                }
            }
            return isResult;
        }

        public bool Wifi_WriteSocketClientSubnetMask(byte[] aData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteSocketClientSubnetMask(aData);
                    else
                        isResult = WriteSocketClientSubnetMask_Port(Port, aData);
                }
                else
                {
                    isResult = WriteSocketClientSubnetMask_Sock(aData);
                }
            }
            return isResult;
        }

        public bool Wifi_ReadSocketClientGateway(ref string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                IntPtr pResult = Marshal.AllocCoTaskMem(4);
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = ReadSocketClientGateway(pResult);
                    else
                        isResult = ReadSocketClientGateway_Port(Port, pResult);
                }
                else
                {
                    isResult = ReadSocketClientGateway_Sock(pResult);
                }
                if (isResult)
                {
                    byte[] aResult = new byte[4];
                    Marshal.Copy(pResult, aResult, 0, 4);
                    Marshal.FreeHGlobal(pResult);
                    szData = string.Format("{0}.{1}.{2}.{3}",(int)aResult[0],(int)aResult[1],(int)aResult[2],(int)aResult[3]);
                }
            }
            return isResult;
        }

        public bool Wifi_WriteSocketClientGateway(string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                string[] aData = szData.Split('.');
                byte[] aIP = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    aIP[i] = byte.Parse(aData[i]);
                }

                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteSocketServerGateway(aIP);
                    else
                        isResult = WriteSocketServerGateway_Port(Port, aIP);
                }
                else
                {
                    isResult = WriteSocketServerGateway_Sock(aIP);
                }
            }
            return isResult;
        }

        public bool Wifi_WriteSocketClientGateway(byte[] aData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteSocketServerGateway(aData);
                    else
                        isResult = WriteSocketServerGateway_Port(Port, aData);
                }
                else
                {
                    isResult = WriteSocketServerGateway_Sock(aData);
                }
            }
            return isResult;
        }

        public bool Wifi_ReadSocketClientDNS(ref string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                IntPtr pResult = Marshal.AllocCoTaskMem(4);
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = ReadSocketClientDNS(pResult);
                    else
                        isResult = ReadSocketClientDNS_Port(Port, pResult);
                }
                else
                {
                    isResult = ReadSocketClientDNS_Sock(pResult);
                }
                if (isResult)
                {
                    byte[] aResult = new byte[4];
                    Marshal.Copy(pResult, aResult, 0, 4);
                    Marshal.FreeHGlobal(pResult);
                    szData = string.Format("{0}.{1}.{2}.{3}",(int)aResult[0],(int)aResult[1],(int)aResult[2],(int)aResult[3]);
                }
            }
            return isResult;
        }

        public bool Wifi_WriteSocketClientDNS(string szData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                string[] aData = szData.Split('.');
                byte[] aIP = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    aIP[i] = byte.Parse(aData[i]);
                }

                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteSocketClientDNS(aIP);
                    else
                        isResult = WriteSocketClientDNS_Port(Port, aIP);
                }
                else
                {
                    isResult = WriteSocketClientDNS_Sock(aIP);
                }

            }
            return isResult;
        }

        public bool Wifi_WriteSocketClientDNS(byte[] aData)
        {
            bool isResult = false;
            if (m_checkInitial)
            {
                if (string.Empty == RemoteIP)
                {
                    if (-1 == Port)
                        isResult = WriteSocketClientDNS(aData);
                    else
                        isResult = WriteSocketClientDNS_Port(Port, aData);
                }
                else
                {
                    isResult = WriteSocketClientDNS_Sock(aData);
                }

            }
            return isResult;
        }

    }
}
