using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AvSimDevKit
{
    public partial class Calibrator
    {
        public int Port { get; set; }

        public int OpenPortCount { get; private set; }

        public int Initial(int nPort = -1)
        {
            Port = nPort;
            if (-1 == nPort)
            {
                OpenPortCount = Calibrator__Initial();
                return OpenPortCount;
            }
            else
            {
                OpenPortCount = -1;
                return Calibrator__Initial_Port(Port);
            }
        }

        public int Destroy()
        {
            if (-1 == Port)
                return Calibrator__Destroy();
            else
                return Calibrator__Destroy_Port(Port);
        }

        public void Ping()
        {
            if (-1 == Port)
                Calibrator__Ping();
            else
                Calibrator__Ping_Port(Port);
        }

        public static byte[] GetCommand_Ping()
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_Ping(ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int[] ReadSimulatorMode()
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadSimulatorMode();
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
               aResult[0] = Calibrator__ReadSimulatorMode_Port(Port);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadSimulatorMode()
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadSimulatorMode(ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void WriteSimulatorMode(eSimulatorMode eSimulatorMode)
        {
            if (-1 == Port)
                Calibrator__WriteSimulatorMode((int)eSimulatorMode);
            else
                Calibrator__WriteSimulatorMode_Port(Port, (int)eSimulatorMode);
        }

        public static byte[] GetCommand_WriteSimulatorMode(eSimulatorMode eSimulatorMode)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_WriteSimulatorMode((int)eSimulatorMode, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int[] ReadSimulatorScaleMin()
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadSimulatorScaleMin();
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
               aResult[0] = Calibrator__ReadSimulatorScaleMin_Port(Port);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadSimulatorScaleMin()
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadSimulatorScaleMin(ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void WriteSimulatorScaleMin(int nScaleMin)
        {
            if (-1 == Port)
                Calibrator__WriteSimulatorScaleMin(nScaleMin);
            else
                Calibrator__WriteSimulatorScaleMin_Port(Port, nScaleMin);
        }

        public static byte[] GetCommand_WriteSimulatorScaleMin(int nScaleMin)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_WriteSimulatorScaleMin(nScaleMin, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int[] ReadSimulatorScaleCenter()
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadSimulatorScaleCenter();
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
               aResult[0] = Calibrator__ReadSimulatorScaleCenter_Port(Port);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadSimulatorScaleCenter()
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadSimulatorScaleCenter(ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void WriteSimulatorScaleCenter(int nScaleCenter)
        {
            if (-1 == Port)
                Calibrator__WriteSimulatorScaleCenter(nScaleCenter);
            else
                Calibrator__WriteSimulatorScaleCenter_Port(Port, nScaleCenter);
        }

        public static byte[] GetCommand_WriteSimulatorScaleCenter(int nScaleCenter)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_WriteSimulatorScaleCenter(nScaleCenter, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int[] ReadSimulatorScaleMax()
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadSimulatorScaleMax();
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
               aResult[0] = Calibrator__ReadSimulatorScaleMax_Port(Port);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadSimulatorScaleMax()
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadSimulatorScaleMax(ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void WriteSimulatorScaleMax(int nScaleMax)
        {
            if (-1 == Port)
                Calibrator__WriteSimulatorScaleMax(nScaleMax);
            else
                Calibrator__WriteSimulatorScaleMax_Port(Port, nScaleMax);
        }

        public static byte[] GetCommand_WriteSimulatorScaleMax(int nScaleMax)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_WriteSimulatorScaleMax(nScaleMax, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int[] ReadSpeedDipSw()
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadSpeedDipSw();
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
               aResult[0] = Calibrator__ReadSpeedDipSw_Port(Port);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadSpeedDipSw()
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadSpeedDipSw(ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int[] ReadJogDipSw()
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadJogDipSw();
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
               aResult[0] = Calibrator__ReadJogDipSw_Port(Port);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadJogDipSw()
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadJogDipSw(ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }
        
        public int[] ReadHome()
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadHome();
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
               aResult[0] = Calibrator__ReadHome_Port(Port);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadHome()
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadHome(ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int[] ReadLsp()
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadLsp();
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
               aResult[0] = Calibrator__ReadLsp_Port(Port);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadLsp()
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadLsp(ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int[] ReadLsn()
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadLsn();
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
               aResult[0] = Calibrator__ReadLsn_Port(Port);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadLsn()
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadLsn(ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int[] ReadDi()
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadDi();
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
                aResult[0] = Calibrator__ReadDi_Port(Port);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadDi()
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadDi(ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int[] ReadDo()
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadDo();
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
               aResult[0] = Calibrator__ReadDo_Port(Port);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadDo()
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadDo(ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void WriteDo(byte[] aDo, int nDoSize)
        {
            if (-1 == Port)
                Calibrator__WriteDo(aDo, nDoSize);
            else
                Calibrator__WriteDo_Port(Port, aDo, nDoSize);
        }

        public static byte[] GetCommand_WriteDo(byte[] aDo, int nDoSize)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_WriteDo(aDo, nDoSize, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int[] ReadDoMotorModuleRelay()
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadDoMotorModuleRelay();
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
               aResult[0] = Calibrator__ReadDoMotorModuleRelay_Port(Port);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadDoMotorModuleRelay()
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadDoMotorModuleRelay(ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void WriteDoMotorModuleRelay(byte[] aDo, int nDoSize)
        {
            if (-1 == Port)
                Calibrator__WriteDoMotorModuleRelay(aDo, nDoSize);
            else
                Calibrator__WriteDoMotorModuleRelay_Port(Port, aDo, nDoSize);
        }

        public static byte[] GetCommand_WriteDoMotorModuleRelay(byte[] aDo, int nDoSize)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_WriteDoMotorModuleRelay(aDo, nDoSize, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int[] ReadGaugeSpeed()
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadGaugeSpeed();
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
                aResult[0] = Calibrator__ReadGaugeSpeed_Port(Port);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadGaugeSpeed()
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadGaugeSpeed(ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void WriteGaugeSpeed(int nGaugeSpeed)
        {
            if (-1 == Port)
                Calibrator__WriteGaugeSpeed(nGaugeSpeed);
            else
                Calibrator__WriteGaugeSpeed_Port(Port, nGaugeSpeed);
        }

        public static byte[] GetCommand_WriteGaugeSpeed(int nGaugeSpeed)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_WriteGaugeSpeed(nGaugeSpeed, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int[] ReadGaugeRpm()
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadGaugeRpm();
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
                aResult[0] = Calibrator__ReadGaugeRpm_Port(Port);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadGaugeRpm()
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadGaugeRpm(ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void WriteGaugeRpm(int nGaugeRpm)
        {
            if (-1 == Port)
                Calibrator__WriteGaugeRpm(nGaugeRpm);
            else
                Calibrator__WriteGaugeRpm_Port(Port, nGaugeRpm);
        }

        public static byte[] GetCommand_WriteGaugeRpm(int nGaugeRpm)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_WriteGaugeRpm(nGaugeRpm, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int[] ReadGaugeBlowerFrequency(int nChannel)
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadGaugeBlowerFrequency(nChannel);
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
                aResult[0] = Calibrator__ReadGaugeBlowerFrequency_Port(Port, nChannel);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadGaugeBlowerFrequency(int nChannel)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadGaugeBlowerFrequency(nChannel, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void WriteGaugeBlowerFrequency(int nChannel, int nGaugeBlowerFrequency)
        {
            if (-1 == Port)
                Calibrator__WriteGaugeBlowerFrequency(nChannel, nGaugeBlowerFrequency);
            else
                Calibrator__WriteGaugeBlowerFrequency_Port(Port, nChannel, nGaugeBlowerFrequency);
        }

        public static byte[] GetCommand_WriteGaugeBlowerFrequency(int nChannel, int nGaugeBlowerFrequency)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_WriteGaugeBlowerFrequency(nChannel, nGaugeBlowerFrequency, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int[] ReadGaugeClutchFrequency(int nChannel)
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadGaugeClutchFrequency(nChannel);
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
                aResult[0] = Calibrator__ReadGaugeClutchFrequency_Port(Port, nChannel);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadGaugeClutchFrequency(int nChannel)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadGaugeClutchFrequency(nChannel, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void WriteGaugeClutchFrequency(int nChannel, int nGaugeClutchFrequency)
        {
            if (-1 == Port)
                Calibrator__WriteGaugeClutchFrequency(nChannel, nGaugeClutchFrequency);
            else
                Calibrator__WriteGaugeClutchFrequency_Port(Port, nChannel, nGaugeClutchFrequency);
        }

        public static byte[] GetCommand_WriteGaugeClutchFrequency(int nChannel, int nGaugeClutchFrequency)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_WriteGaugeClutchFrequency(nChannel, nGaugeClutchFrequency, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int[] ReadGaugeBlowerRate(int nChannel)
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadGaugeBlowerRate(nChannel);
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
                aResult[0] = Calibrator__ReadGaugeBlowerRate_Port(Port, nChannel);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadGaugeBlowerRate(int nChannel)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadGaugeBlowerRate(nChannel, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void WriteGaugeBlowerRate(int nChannel, int nGaugeBlowerRate)
        {
            if (-1 == Port)
                Calibrator__WriteGaugeBlowerRate(nChannel, nGaugeBlowerRate);
            else
                Calibrator__WriteGaugeBlowerRate_Port(Port, nChannel, nGaugeBlowerRate);
        }

        public static byte[] GetCommand_WriteGaugeBlowerRate(int nChannel, int nGaugeBlowerRate)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_WriteGaugeBlowerRate(nChannel, nGaugeBlowerRate, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public int[] ReadGaugeClutchRate(int nChannel)
        {
            int[] aResult = null;
            if (-1 == Port)
            {
                IntPtr pCommand = Calibrator__ReadGaugeClutchRate(nChannel);
                aResult = new int[OpenPortCount];
                Marshal.Copy(pCommand, aResult, 0, OpenPortCount);
            }
            else
            {
                aResult = new int[1];
                aResult[0] = Calibrator__ReadGaugeClutchRate_Port(Port, nChannel);
            }
            return aResult;
        }

        public static byte[] GetCommand_ReadGaugeClutchRate(int nChannel)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_ReadGaugeClutchRate(nChannel, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void WriteGaugeClutchRate(int nChannel, int nGaugeClutchFrequency)
        {
            if (-1 == Port)
                Calibrator__WriteGaugeClutchRate(nChannel, nGaugeClutchFrequency);
            else
                Calibrator__WriteGaugeClutchRate_Port(Port, nChannel, nGaugeClutchFrequency);
        }

        public static byte[] GetCommand_WriteGaugeClutchRate(int nChannel, int nGaugeClutchFrequency)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_WriteGaugeClutchRate(nChannel, nGaugeClutchFrequency, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

        public void CommandSend(int nMainCmd, int nSubCmd, byte chResponseType, int nDataLength,
                                int nData1 = 0, int nData2 = 0, int nData3 = 0, int nData4 = 0, int nData5 = 0,
                                int nData6 = 0, int nData7 = 0, int nData8 = 0, int nData9 = 0, int nData10 = 0,
                                int nData11 = 0, int nData12 = 0, int nData13 = 0, int nData14 = 0, int nData15 = 0,
                                int nData16 = 0, int nData17 = 0, int nData18 = 0, int nData19 = 0, int nData20 = 0,
                                int nData21 = 0, int nData22 = 0, int nData23 = 0, int nData24 = 0, int nData25 = 0,
                                int nData26 = 0, int nData27 = 0, int nData28 = 0, int nData29 = 0, int nData30 = 0,
                                int nData31 = 0, int nData32 = 0)
        {
            if (-1 == Port)
                Calibrator__CommandSend(nMainCmd, nSubCmd, chResponseType, nDataLength,
                                        nData1, nData2, nData3, nData4, nData5, nData6, nData7, nData8, nData9, nData10,
                                        nData11, nData12, nData13, nData14, nData15, nData16, nData17, nData18, nData19, nData20,
                                        nData21, nData22, nData23, nData24, nData25, nData26, nData27, nData28, nData29, nData30,
                                        nData31, nData32);
            else
                Calibrator__CommandSend_Port(Port, nMainCmd, nSubCmd, chResponseType, nDataLength,
                                        nData1, nData2, nData3, nData4, nData5, nData6, nData7, nData8, nData9, nData10,
                                        nData11, nData12, nData13, nData14, nData15, nData16, nData17, nData18, nData19, nData20,
                                        nData21, nData22, nData23, nData24, nData25, nData26, nData27, nData28, nData29, nData30,
                                        nData31, nData32);
        }

        public static byte[] GetCommand_CommandSend(int nMainCmd, int nSubCmd, byte chResponseType, int nDataLength,
                                int nData1 = 0, int nData2 = 0, int nData3 = 0, int nData4 = 0, int nData5 = 0,
                                int nData6 = 0, int nData7 = 0, int nData8 = 0, int nData9 = 0, int nData10 = 0,
                                int nData11 = 0, int nData12 = 0, int nData13 = 0, int nData14 = 0, int nData15 = 0,
                                int nData16 = 0, int nData17 = 0, int nData18 = 0, int nData19 = 0, int nData20 = 0,
                                int nData21 = 0, int nData22 = 0, int nData23 = 0, int nData24 = 0, int nData25 = 0,
                                int nData26 = 0, int nData27 = 0, int nData28 = 0, int nData29 = 0, int nData30 = 0,
                                int nData31 = 0, int nData32 = 0)
        {
            IntPtr pCommand = IntPtr.Zero;
            int nComandSize = 0;
            pCommand = Calibrator__GetCommand_CommandSend(nMainCmd, nSubCmd, chResponseType, nDataLength,
                                        nData1, nData2, nData3, nData4, nData5, nData6, nData7, nData8, nData9, nData10,
                                        nData11, nData12, nData13, nData14, nData15, nData16, nData17, nData18, nData19, nData20,
                                        nData21, nData22, nData23, nData24, nData25, nData26, nData27, nData28, nData29, nData30,
                                        nData31, nData32, ref nComandSize);
            byte[] aResult = new byte[nComandSize];
            Marshal.Copy(pCommand, aResult, 0, nComandSize);
            return aResult;
        }

    }
}
