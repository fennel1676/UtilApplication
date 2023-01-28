using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AvSimDll.MotionExternC
{
    public partial class ParachuteMotionControl
    {   
        public int Initial()
        {
            return Parachute__MotionControl__Initial();
        }

        public int Initial(int nPort1 = 1, int nPort2 = 2)
        {
            if (nPort1 == nPort2 || 0 >= nPort1 || 0 >= nPort2)
                return Parachute__MotionControl__Initial();
            else
                return Parachute__MotionControl__Initial_Port(nPort1, nPort2);
        }

        public int Destroy()
        {
            return Parachute__MotionControl__Destroy();
        }

        public void DOF(int nComPort, int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed)
        {
            Parachute__MotionControl__DOF(nComPort, nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed);
        }
        public byte[] GetCommand_DOF(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Parachute__MotionControl__GetCommand_DOF(nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void DOF_and_Blower(int nComPort, int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower)
        {
            Parachute__MotionControl__DOF_and_Blower(nComPort, nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nBlower);
        }

        public byte[] GetCommand_DOF_and_Blower(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower)
        { 
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Parachute__MotionControl__GetCommand_DOF_and_Blower(nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nBlower, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void DOF_and_Blower_and_DO(int nComPort, int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower, int nDO)
        {
            Parachute__MotionControl__DOF_and_Blower_and_DO(nComPort, nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nBlower, nDO);
        }

        public byte[] GetCommand_DOF_and_Blower_and_DO(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower, int nDO)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Parachute__MotionControl__GetCommand_DOF_and_Blower_and_DO(nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nBlower, nDO, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void DO(int nComPort, int nDO)
        {
            Parachute__MotionControl__DO(nComPort, nDO);
        }

        public byte[] GetCommand_DO(int nDO)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Parachute__MotionControl__GetCommand_DO(nDO, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int DI(int nComPort)
        {
            return Parachute__MotionControl__DI(nComPort);
        }

        public byte[] GetCommand_DI()
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Parachute__MotionControl__GetCommand_DI(ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void Axis(int nComPort, int nAxisPos1, int nAxisSpeed1, int nAxisPos2, int nAxisSpeed2, int nAxisPos3, int nAxisSpeed3,
                        int nAxisPos4, int nAxisSpeed4, int nAxisPos5, int nAxisSpeed5, int nAxisPos6, int nAxisSpeed6)
        {
            Parachute__MotionControl__Axis( nComPort, nAxisPos1, nAxisSpeed1, nAxisPos2, nAxisSpeed2, nAxisPos3, nAxisSpeed3,
                                            nAxisPos4, nAxisSpeed4, nAxisPos5, nAxisSpeed5, nAxisPos6, nAxisSpeed6);
        }

        public byte[] GetCommand_Axis(  int nAxisPos1, int nAxisSpeed1, int nAxisPos2, int nAxisSpeed2, int nAxisPos3, int nAxisSpeed3,
                                        int nAxisPos4, int nAxisSpeed4, int nAxisPos5, int nAxisSpeed5, int nAxisPos6, int nAxisSpeed6)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Parachute__MotionControl__GetCommand_Axis(   nAxisPos1, nAxisSpeed1, nAxisPos2, nAxisSpeed2, nAxisPos3, nAxisSpeed3,
                                                                    nAxisPos4, nAxisSpeed4, nAxisPos5, nAxisSpeed5, nAxisPos6, nAxisSpeed6, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void Axis_and_Blower( int nComPort, int nAxisPos1, int nAxisSpeed1, int nAxisPos2, int nAxisSpeed2, int nAxisPos3, int nAxisSpeed3,
                                            int nAxisPos4, int nAxisSpeed4, int nAxisPos5, int nAxisSpeed5, int nAxisPos6, int nAxisSpeed6, int nBlower)
        {
            Parachute__MotionControl__Axis_and_Blower(  nComPort, nAxisPos1, nAxisSpeed1, nAxisPos2, nAxisSpeed2, nAxisPos3, nAxisSpeed3,
                                                        nAxisPos4, nAxisSpeed4, nAxisPos5, nAxisSpeed5, nAxisPos6, nAxisSpeed6, nBlower);
        }

        public byte[] GetCommand_Axis_and_Blower(int nComPort, int nAxisPos1, int nAxisSpeed1, int nAxisPos2, int nAxisSpeed2, int nAxisPos3, int nAxisSpeed3,
                                            int nAxisPos4, int nAxisSpeed4, int nAxisPos5, int nAxisSpeed5, int nAxisPos6, int nAxisSpeed6, int nBlower)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Parachute__MotionControl__GetCommand_Axis_and_Blower(nAxisPos1, nAxisSpeed1, nAxisPos2, nAxisSpeed2, nAxisPos3, nAxisSpeed3,
                                                                            nAxisPos4, nAxisSpeed4, nAxisPos5, nAxisSpeed5, nAxisPos6, nAxisSpeed6, nBlower, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void Axis_and_Blower_and_DO(  int nComPort, int nAxisPos1, int nAxisSpeed1, int nAxisPos2, int nAxisSpeed2, int nAxisPos3, int nAxisSpeed3,
                                                    int nAxisPos4, int nAxisSpeed4, int nAxisPos5, int nAxisSpeed5, int nAxisPos6, int nAxisSpeed6, int nBlower, int nDO)
        {
            Parachute__MotionControl__Axis_and_Blower_and_DO(   nComPort, nAxisPos1, nAxisSpeed1, nAxisPos2, nAxisSpeed2, nAxisPos3, nAxisSpeed3,
                                                                nAxisPos4, nAxisSpeed4, nAxisPos5, nAxisSpeed5, nAxisPos6, nAxisSpeed6, nBlower, nDO);
        }

        public byte[] GetCommand_Axis_and_Blower_and_DO(int nComPort, int nAxisPos1, int nAxisSpeed1, int nAxisPos2, int nAxisSpeed2, int nAxisPos3, int nAxisSpeed3,
                                    int nAxisPos4, int nAxisSpeed4, int nAxisPos5, int nAxisSpeed5, int nAxisPos6, int nAxisSpeed6, int nBlower, int nDO)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Parachute__MotionControl__GetCommand_Axis_and_Blower_and_DO( nAxisPos1, nAxisSpeed1, nAxisPos2, nAxisSpeed2, nAxisPos3, nAxisSpeed3,
                                                                                    nAxisPos4, nAxisSpeed4, nAxisPos5, nAxisSpeed5, nAxisPos6, nAxisSpeed6, nBlower, nDO, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void DOF_and_Blower_and_GripStrength_and_GripPos( int nComPort,
                                                                int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed,
                                                                int nBlower1, int nBlower2, int nBlower3, int nBlower4,
                                                                int nGripStrengthLeft, int nGripStrengthRight,
                                                                ref int pnGripPosLeft, ref int pnGripPosRight)
        {
            Parachute__MotionControl__DOF_and_Blower_and_GripStrength_and_GripPos(  nComPort,
                                                                                    nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed,
                                                                                    nBlower1, nBlower2, nBlower3, nBlower4,
                                                                                    nGripStrengthLeft, nGripStrengthRight,
                                                                                    ref pnGripPosLeft, ref pnGripPosRight);
        }

        public byte[] GetCommand_DOF_and_Blower_and_GripStrength_and_GripPos(
                                                                int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed,
                                                                int nBlower1, int nBlower2, int nBlower3, int nBlower4,
                                                                int nGripStrengthLeft, int nGripStrengthRight,
                                                                int nGripPosLeft, int nGripPosRight)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Parachute__MotionControl__GetCommand_DOF_and_Blower_and_GripStrength_and_GripPos(nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed,
                                                                                    nBlower1, nBlower2, nBlower3, nBlower4, nGripStrengthLeft, nGripStrengthRight,
                                                                                    nGripPosLeft, nGripPosRight, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void DOF_and_Blower_and_GripStrength_and_GripPos__Yaw_Direction(  int nComPort,
                                                                int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nYawMode,
                                                                int nBlower1, int nBlower2, int nBlower3, int nBlower4,
                                                                int nGripStrengthLeft, int nGripStrengthRight,
                                                                ref int pnGripPosLeft, ref int pnGripPosRight)
        {
            Parachute__MotionControl__DOF_and_Blower_and_GripStrength_and_GripPos__Yaw_Direction(   nComPort,
                                                                                                    nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nYawMode,
                                                                                                    nBlower1, nBlower2, nBlower3, nBlower4,
                                                                                                    nGripStrengthLeft, nGripStrengthRight,
                                                                                                    ref pnGripPosLeft, ref pnGripPosRight);
        }

        public byte[] GetCommand_DOF_and_Blower_and_GripStrength_and_GripPos__Yaw_Direction(
                                                                int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nYawMode,
                                                                int nBlower1, int nBlower2, int nBlower3, int nBlower4,
                                                                int nGripStrengthLeft, int nGripStrengthRight,
                                                                int nGripPosLeft, int nGripPosRight)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Parachute__MotionControl__GetCommand_DOF_and_Blower_and_GripStrength_and_GripPos__Yaw_Direction(nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nYawMode,
                                                                                                    nBlower1, nBlower2, nBlower3, nBlower4, nGripStrengthLeft, nGripStrengthRight,
                                                                                                    nGripPosLeft, nGripPosRight, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void Action(  int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nSpeedYaw,
                                    int nBlower1, int nBlower2, int nBlower3, int nBlower4,
                                    int nGripStrengthLeft, int nGripStrengthRight,
                                    ref int pnGripPosLeft, ref int pnGripPosRight)
        {
            Parachute__MotionControl__Action(nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nSpeedYaw,
                                             nBlower1, nBlower2, nBlower3, nBlower4,
                                             nGripStrengthLeft, nGripStrengthRight,
                                             ref pnGripPosLeft, ref pnGripPosRight);
        }

        public void Action__Yaw_Direction(   int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nSpeedYaw, int nYawActionMode, ref int pnYawCurrentPosition,
                                                    int nBlower1, int nBlower2, int nBlower3, int nBlower4,
                                                    int nGripStrengthLeft, int nGripStrengthRight,
                                                    ref int pnGripPosLeft, ref int pnGripPosRight)
        {
            Parachute__MotionControl__Action__Yaw_Direction(
                                             nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nSpeedYaw, nYawActionMode, ref pnYawCurrentPosition,
                                             nBlower1, nBlower2, nBlower3, nBlower4,
                                             nGripStrengthLeft, nGripStrengthRight,
                                             ref pnGripPosLeft, ref pnGripPosRight);
        }

        public void Action__Yaw_Direction__Action_DOF_Motion(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed)
        {
            Parachute__MotionControl__Action__Yaw_Direction__Action_DOF_Motion(nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed);
        }

        public void Action__Yaw_Direction__Action_Yaw_Motion(int nSpeedYaw, int nYawActionMode, ref int pnYawCurrentPosition)
        {
            Parachute__MotionControl__Action__Yaw_Direction__Action_Yaw_Motion(nSpeedYaw, nYawActionMode, ref pnYawCurrentPosition);
        }

        public void Action__Yaw_Direction__Action_Blower(int nBlower1, int nBlower2, int nBlower3, int nBlower4)
        {
            Parachute__MotionControl__Action__Yaw_Direction__Action_Blower(nBlower1, nBlower2, nBlower3, nBlower4);
        }

        public void Action__Yaw_Direction__Action_GripStrength(int nGripStrengthLeft, int nGripStrengthRight)
        {
            Parachute__MotionControl__Action__Yaw_Direction__Action_GripStrength(nGripStrengthLeft, nGripStrengthRight);
        }

        public void Action__Yaw_Direction__Obtain_GripPos(ref int pnGripPosLeft, ref int pnGripPosRight)
        {
            Parachute__MotionControl__Action__Yaw_Direction__Obtain_GripPos(ref pnGripPosLeft, ref pnGripPosRight);
        }
    }
}
