using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AvSimDevKit
{
    public partial class Calibrator
    {
        // Initial
        [DllImport("AvSimDevKit_Calibrator.dll")]
        private extern static int Calibrator__Initial();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__Initial_Port(int nPort);

        // Destroy
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__Destroy();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__Destroy_Port(int nPort);

        //	Ping
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__Ping();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__Ping_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_Ping(ref int pnCommandSize);

        //	SimulatorMode
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadSimulatorMode();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadSimulatorMode_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadSimulatorMode(ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteSimulatorMode(int nSimulatorMode);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteSimulatorMode_Port(int nPort, int nSimulatorMode);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_WriteSimulatorMode(int nSimulatorMode, ref int pnCommandSize);

        //	SimulatorScale Min Center Max
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadSimulatorScaleMin();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadSimulatorScaleMin_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadSimulatorScaleMin(ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteSimulatorScaleMin(int nScaleMin);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteSimulatorScaleMin_Port(int nPort, int nScaleMin);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_WriteSimulatorScaleMin(int nScaleMin, ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadSimulatorScaleCenter();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadSimulatorScaleCenter_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadSimulatorScaleCenter(ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteSimulatorScaleCenter(int nScaleCenter);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteSimulatorScaleCenter_Port(int nPort, int nScaleCenter);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_WriteSimulatorScaleCenter(int nScaleCenter, ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadSimulatorScaleMax();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadSimulatorScaleMax_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadSimulatorScaleMax(ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteSimulatorScaleMax(int nScaleMax);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteSimulatorScaleMax_Port(int nPort, int nScaleMax);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_WriteSimulatorScaleMax(int nScaleMax, ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadSpeedDipSw();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadSpeedDipSw_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadSpeedDipSw(ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadJogDipSw();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadJogDipSw_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadJogDipSw(ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadHome();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadHome_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadHome(ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadLsp();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadLsp_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadLsp(ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadLsn();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadLsn_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadLsn(ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadDi();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadDi_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadDi(ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadDo();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadDo_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadDo(ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteDo(byte[] szDo, int nDoSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteDo_Port(int nPort, byte[] szDo, int nDoSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_WriteDo(byte[] szDo, int nDoSize, ref int pnCommandSize);
        
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadDoMotorModuleRelay();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadDoMotorModuleRelay_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadDoMotorModuleRelay(ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteDoMotorModuleRelay(byte[] szDo, int nDoSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteDoMotorModuleRelay_Port(int nPort, byte[] szDo, int nDoSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_WriteDoMotorModuleRelay(byte[] szDo, int nDoSize, ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadGaugeSpeed();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadGaugeSpeed_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadGaugeSpeed(ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteGaugeSpeed(int nGaugeSpeed);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteGaugeSpeed_Port(int nPort, int nGaugeSpeed);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_WriteGaugeSpeed(int nGaugeSpeed, ref int pnCommandSize);
        
        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadGaugeRpm();

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadGaugeRpm_Port(int nPort);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadGaugeRpm(ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteGaugeRpm(int nGaugeSpeed);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteGaugeRpm_Port(int nPort, int nGaugeSpeed);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_WriteGaugeRpm(int nGaugeSpeed, ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadGaugeBlowerFrequency(int nChannel);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadGaugeBlowerFrequency_Port(int nPort, int nChannel);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadGaugeBlowerFrequency(int nChannel, ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteGaugeBlowerFrequency(int nChannel, int nGaugeBlowerFrequency);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteGaugeBlowerFrequency_Port(int nPort, int nChannel, int nGaugeBlowerFrequency);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_WriteGaugeBlowerFrequency(int nChannel, int nGaugeBlowerFrequency, ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadGaugeClutchFrequency(int nChannel);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadGaugeClutchFrequency_Port(int nPort, int nChannel);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadGaugeClutchFrequency(int nChannel, ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteGaugeClutchFrequency(int nChannel, int nGaugeClutchFrequency);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteGaugeClutchFrequency_Port(int nPort, int nChannel, int nGaugeClutchFrequency);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_WriteGaugeClutchFrequency(int nChannel, int nGaugeClutchFrequency, ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadGaugeBlowerRate(int nChannel);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadGaugeBlowerRate_Port(int nPort, int nChannel);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadGaugeBlowerRate(int nChannel, ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteGaugeBlowerRate(int nChannel, int nGaugeBlowerRate);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteGaugeBlowerRate_Port(int nPort, int nChannel, int nGaugeBlowerRate);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_WriteGaugeBlowerRate(int nChannel, int nGaugeBlowerRate, ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__ReadGaugeClutchRate(int nChannel);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static int Calibrator__ReadGaugeClutchRate_Port(int nPort, int nChannel);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_ReadGaugeClutchRate(int nChannel, ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteGaugeClutchRate(int nChannel, int nGaugeClutchFrequency);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__WriteGaugeClutchRate_Port(int nPort, int nChannel, int nGaugeClutchFrequency);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_WriteGaugeClutchRate(int nChannel, int nGaugeClutchFrequency, ref int pnCommandSize);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__CommandSend(     int nMainCmd, int nSubCmd, byte chResponseType, int nDataLength,
                                                                int nData1, int nData2, int nData3, int nData4, int nData5, int nData6, int nData7, int nData8, int nData9, int nData10,
                                                                int nData11, int nData12, int nData13, int nData14, int nData15, int nData16, int nData17, int nData18, int nData19, int nData20,
                                                                int nData21, int nData22, int nData23, int nData24, int nData25, int nData26, int nData27, int nData28, int nData29, int nData30,
                                                                int nData31, int nData32);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static void Calibrator__CommandSend_Port(int nPort, int nMainCmd, int nSubCmd, byte chResponseType, int nDataLength,
                                                                int nData1, int nData2, int nData3, int nData4, int nData5, int nData6, int nData7, int nData8, int nData9, int nData10,
                                                                int nData11, int nData12, int nData13, int nData14, int nData15, int nData16, int nData17, int nData18, int nData19, int nData20,
                                                                int nData21, int nData22, int nData23, int nData24, int nData25, int nData26, int nData27, int nData28, int nData29, int nData30,
                                                                int nData31, int nData32);

        [DllImport("AvSimDllMotionExternC.dll")]
        private extern static IntPtr Calibrator__GetCommand_CommandSend(int nMainCmd, int nSubCmd, byte chResponseType, int nDataLength,
                                                                int nData1, int nData2, int nData3, int nData4, int nData5, int nData6, int nData7, int nData8, int nData9, int nData10,
                                                                int nData11, int nData12, int nData13, int nData14, int nData15, int nData16, int nData17, int nData18, int nData19, int nData20,
                                                                int nData21, int nData22, int nData23, int nData24, int nData25, int nData26, int nData27, int nData28, int nData29, int nData30,
                                                                int nData31, int nData32, ref int pnCommandSize);

    }
}
