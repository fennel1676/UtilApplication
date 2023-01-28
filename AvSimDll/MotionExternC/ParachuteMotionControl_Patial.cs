using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AvSimDll.MotionExternC
{
    public partial class ParachuteMotionControl
    {
        [DllImport("AvSimDllMotionExternC.dll")]
        // Initial
        private extern static int Parachute__MotionControl__Initial();

        [DllImport("AvSimDllMotionExternC.dll")]
        // Initial
        private extern static int Parachute__MotionControl__Initial_Port(int nPort1, int nPort2);

        [DllImport("AvSimDllMotionExternC.dll")]
        // Destroy
        private extern static int Parachute__MotionControl__Destroy();

        [DllImport("AvSimDllMotionExternC.dll")]
        // Motion Control DOF
        //	nRoll ~ nHeave : 0~20000
        //	nSpeed : 0(max) ~ 255(min)
        private extern static void Parachute__MotionControl__DOF(int nComPort, int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed);

        [DllImport("AvSimDllMotionExternC.dll")]
        // Motion Control DOF
        //	nRoll ~ nHeave : 0~20000
        //	nSpeed : 0(max) ~ 255(min)
        private extern static IntPtr Parachute__MotionControl__GetCommand_DOF(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, ref int refComandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        // Motion Control DOF and Blower
        //	nRoll ~ nHeave : 0~20000
        //	nSpeed : 0(max) ~ 255(min)
        //	nBlower : 0(min) ~ 100(max)
        private extern static void Parachute__MotionControl__DOF_and_Blower(int nComPort, int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower);

        [DllImport("AvSimDllMotionExternC.dll")]
        // Motion Control DOF and Blower
        //	nRoll ~ nHeave : 0~20000
        //	nSpeed : 0(max) ~ 255(min)
        //	nBlower : 0(min) ~ 100(max)
        private extern static IntPtr Parachute__MotionControl__GetCommand_DOF_and_Blower(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower, ref int refComandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        // Motion Control DOF and Blower and DO
        //	nRoll ~ nHeave : 0~20000
        //	nSpeed : 0(max) ~ 255(min)
        //	nBlower : 0(min) ~ 100(max)
        //	nDO : bit flag
        private extern static void Parachute__MotionControl__DOF_and_Blower_and_DO(int nComPort, int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower, int nDO);

        [DllImport("AvSimDllMotionExternC.dll")]
        // Motion Control DOF and Blower and DO
        //	nRoll ~ nHeave : 0~20000
        //	nSpeed : 0(max) ~ 255(min)
        //	nBlower : 0(min) ~ 100(max)
        //	nDO : bit flag
        private extern static IntPtr Parachute__MotionControl__GetCommand_DOF_and_Blower_and_DO(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower, int nDO, ref int refComandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        // Motion Control DO
        //	nDO : bit flag
        private extern static void Parachute__MotionControl__DO(int nComPort, int nDO);

        [DllImport("AvSimDllMotionExternC.dll")]
        // Motion Control DO
        //	nDO : bit flag
        private extern static IntPtr Parachute__MotionControl__GetCommand_DO(int nDO, ref int refComandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        // Motion Control DI
        //	return : nDI bit flag
        private extern static int Parachute__MotionControl__DI(int nComPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        // Motion Control DI
        //	return : nDI bit flag
        private extern static IntPtr Parachute__MotionControl__GetCommand_DI(ref int refComandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        //
        // Motion Control Axis
        //	nAxisPos1 ~ nAxisPos8 : 0~20000
        //	nAxisSpeed1 ~ nAxisSpeed8 : 0(max) ~ 255(min)
        private extern static void Parachute__MotionControl__Axis(int nComPort, int nAxisPos1, int nAxisSpeed1, int nAxisPos2, int nAxisSpeed2, int nAxisPos3, int nAxisSpeed3, int nAxisPos4, int nAxisSpeed4, int nAxisPos5, int nAxisSpeed5, int nAxisPos6, int nAxisSpeed6);

        [DllImport("AvSimDllMotionExternC.dll")]
        //
        // Motion Control Axis
        //	nAxisPos1 ~ nAxisPos8 : 0~20000
        //	nAxisSpeed1 ~ nAxisSpeed8 : 0(max) ~ 255(min)
        private extern static IntPtr Parachute__MotionControl__GetCommand_Axis(int nAxisPos1, int nAxisSpeed1, int nAxisPos2, int nAxisSpeed2, int nAxisPos3, int nAxisSpeed3, int nAxisPos4, int nAxisSpeed4, int nAxisPos5, int nAxisSpeed5, int nAxisPos6, int nAxisSpeed6, ref int refComandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        // Motion Control Axis and Blower
        //	nAxisPos1 ~ nAxisPos8 : 0~20000
        //	nAxisSpeed1 ~ nAxisSpeed8 : 0(max) ~ 255(min)
        //	nBlower : 0(min) ~ 100(max)
        private extern static void Parachute__MotionControl__Axis_and_Blower(int nComPort, int nAxisPos1, int nAxisSpeed1, int nAxisPos2, int nAxisSpeed2, int nAxisPos3, int nAxisSpeed3, int nAxisPos4, int nAxisSpeed4, int nAxisPos5, int nAxisSpeed5, int nAxisPos6, int nAxisSpeed6, int nBlower);

        [DllImport("AvSimDllMotionExternC.dll")]
        // Motion Control Axis and Blower
        //	nAxisPos1 ~ nAxisPos8 : 0~20000
        //	nAxisSpeed1 ~ nAxisSpeed8 : 0(max) ~ 255(min)
        //	nBlower : 0(min) ~ 100(max)
        private extern static IntPtr Parachute__MotionControl__GetCommand_Axis_and_Blower(int nAxisPos1, int nAxisSpeed1, int nAxisPos2, int nAxisSpeed2, int nAxisPos3, int nAxisSpeed3, int nAxisPos4, int nAxisSpeed4, int nAxisPos5, int nAxisSpeed5, int nAxisPos6, int nAxisSpeed6, int nBlower, ref int refComandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        // Motion Control Axis and Blower and DO
        //	nAxisPos1 ~ nAxisPos8 : 0~20000
        //	nAxisSpeed1 ~ nAxisSpeed8 : 0(max) ~ 255(min)
        //	nBlower : 0(min) ~ 100(max)
        //	nDO : bit flag
        private extern static void Parachute__MotionControl__Axis_and_Blower_and_DO(int nComPort, int nAxisPos1, int nAxisSpeed1, int nAxisPos2, int nAxisSpeed2, int nAxisPos3, int nAxisSpeed3, int nAxisPos4, int nAxisSpeed4, int nAxisPos5, int nAxisSpeed5, int nAxisPos6, int nAxisSpeed6, int nBlower, int nDO);

        [DllImport("AvSimDllMotionExternC.dll")]
        // Motion Control Axis and Blower and DO
        //	nAxisPos1 ~ nAxisPos8 : 0~20000
        //	nAxisSpeed1 ~ nAxisSpeed8 : 0(max) ~ 255(min)
        //	nBlower : 0(min) ~ 100(max)
        //	nDO : bit flag
        private extern static IntPtr Parachute__MotionControl__GetCommand_Axis_and_Blower_and_DO(int nAxisPos1, int nAxisSpeed1, int nAxisPos2, int nAxisSpeed2, int nAxisPos3, int nAxisSpeed3, int nAxisPos4, int nAxisSpeed4, int nAxisPos5, int nAxisSpeed5, int nAxisPos6, int nAxisSpeed6, int nBlower, int nDO, ref int refComandSize);

        /////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Parachute__MotionControl__DOF_and_Blower_and_GripStrength_and_GripPos(
                                             int nComPort,
                                             int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed,
                                             int nBlower1, int nBlower2, int nBlower3, int nBlower4,
                                             int nGripStrengthLeft, int nGripStrengthRight,
                                             ref int pnGripPosLeft, ref int pnGripPosRight);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Parachute__MotionControl__GetCommand_DOF_and_Blower_and_GripStrength_and_GripPos(
                                             int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed,
                                             int nBlower1, int nBlower2, int nBlower3, int nBlower4,
                                             int nGripStrengthLeft, int nGripStrengthRight,
                                             int nGripPosLeft, int nGripPosRight,
                                             ref int refComandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Parachute__MotionControl__DOF_and_Blower_and_GripStrength_and_GripPos__Yaw_Direction(
                                             int nComPort,
                                             int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nYawMode,
                                             int nBlower1, int nBlower2, int nBlower3, int nBlower4,
                                             int nGripStrengthLeft, int nGripStrengthRight,
                                             ref int pnGripPosLeft, ref int pnGripPosRight);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Parachute__MotionControl__GetCommand_DOF_and_Blower_and_GripStrength_and_GripPos__Yaw_Direction(
                                             int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nYawMode,
                                             int nBlower1, int nBlower2, int nBlower3, int nBlower4,
                                             int nGripStrengthLeft, int nGripStrengthRight,
                                             int nGripPosLeft, int nGripPosRight,
                                             ref int refComandSize);

        /////////////////////////////////////////////////////////////////////////////////////////////
        // 컨텐츠에서 호출 해서 데이터를 적재 및 얻어 가는 함수이다.
        [DllImport("AvSimDllMotionExternC.dll")]
        // Motion rage(Roll, Pitch, _____, Sway, Surge, Heave) : 0~20000, Motion range(yaw) : 0 ~ 36000, Speed : 0-max ~ 255:min
        // Blower rate : 0~100%
        // 줄 당기는 강도 조절 : 0~100%
        // 줄 당긴 위치 : 0~20000
        private extern static void Parachute__MotionControl__Action(
                                             int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nSpeedYaw, 
                                             int nBlower1, int nBlower2, int nBlower3, int nBlower4,                                                    
                                             int nGripStrengthLeft, int nGripStrengthRight,
                                             ref int pnGripPosLeft, ref int pnGripPosRight);

        [DllImport("AvSimDllMotionExternC.dll")]
        // Motion rage(Roll, Pitch, _____, Sway, Surge, Heave) : 0~20000
        // Motion range(yaw) : 0 ~ 36000
        // Speed : 0-max ~ 255:min
        // Blower rate : 0~100%
        // 줄 당기는 강도 조절 : 0~100%
        // 줄 당긴 위치 : 0~20000
        private extern static void Parachute__MotionControl__Action__Yaw_Direction(
                                             int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nSpeedYaw, int nYawActionMode, ref int pnYawCurrentPosition,      
                                             int nBlower1, int nBlower2, int nBlower3, int nBlower4,                                                    
                                             int nGripStrengthLeft, int nGripStrengthRight,
                                             ref int pnGripPosLeft, ref int pnGripPosRight);

        [DllImport("AvSimDllMotionExternC.dll")]
        // Motion rage(Roll, Pitch, _____, Sway, Surge, Heave) : 0~20000
        // Motion range(yaw) : 0 ~ 36000
        // Speed : 0-max ~ 255:min
        private extern static void Parachute__MotionControl__Action__Yaw_Direction__Action_DOF_Motion(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Parachute__MotionControl__Action__Yaw_Direction__Action_Yaw_Motion(int nSpeedYaw, int nYawActionMode, ref int pnYawCurrentPosition);

        [DllImport("AvSimDllMotionExternC.dll")]
        // Blower rate : 0~100%
        private extern static void Parachute__MotionControl__Action__Yaw_Direction__Action_Blower(int nBlower1, int nBlower2, int nBlower3, int nBlower4);

        [DllImport("AvSimDllMotionExternC.dll")]
        // 줄 당기는 강도 조절 : 0~100%
        private extern static void Parachute__MotionControl__Action__Yaw_Direction__Action_GripStrength(int nGripStrengthLeft, int nGripStrengthRight);

        [DllImport("AvSimDllMotionExternC.dll")]
        // 줄 당긴 위치 : 0~20000
        private extern static void Parachute__MotionControl__Action__Yaw_Direction__Obtain_GripPos(ref int pnGripPosLeft, ref int pnGripPosRight);
    }
}
