using System;
using System.Runtime.InteropServices;
using System.Text;

namespace AvSimDllMotionExternCsharp
{
    public class MotionControl
    {
        private const string ENVIRONMENT_INI_FILE_NAME = "AvSimDllMotionExternCsharp_Environment.xml";

        private IAvSimDllComm m_avSimDllComm;

        private IntPtr m_ptrEQExtendData = IntPtr.Zero;
        private IntPtr m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis = IntPtr.Zero;
        private IntPtr m_ptrV2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm = IntPtr.Zero;

        private int g_nType = 1;
        private long g_nTimeout = 500;

        private string g_strRemoteIP = "127.0.0.1";
        private int g_nRemotePort = 4001;

        private eComPort g_nComPort = eComPort.COM1;
        private byte[] g_szComDataFrame = new byte[1024];

        private bool m_isMode = true;

        private UInt16 m_nCRC = 0;
        private int m_nIndex = 0;
        private int m_nCmdCnt = 0;
        private int m_nMainCmd = 0;
        private int m_nSubCmd = 0;
        private byte m_chResponseType = 0;
        private int m_nDataLength = 0;
        private int[] m_nDataBuf = new int[1024];
        private byte[] m_szRxBuf = new byte[1024];

        private int m_nRxCnt = 0;
        private int m_nTxCnt = 0;
        private int m_nOffset = 0;
        private int m_nRespMainCmd = 0;
        private int m_nRespSubCmd = 0;
        private int m_nRespDataLength = 0;

        private string m_strMsg = string.Empty;
        private string m_strMsgBuf = string.Empty;
        private eState m_nCommStateInput = eState.NONE;
        private long m_timer = DateTime.Now.Ticks;
        private byte[] m_aRecvCmd = new byte[4];
        private bool m_isResult = false;
        private UInt16 m_nCRC1 = 0;
        private UInt16 m_nCRC2 = 0;
        private int m_nSendDataLength = 0;

        private int[] m_nResult = new int[256];
        private byte[] m_aDataBuf = new byte[1024];
        private byte[] m_szComDataFrame = new byte[1024];

        private byte[] m_aDO = null;
        private int m_nDI = -1;
        private int m_nDO = -1;

        private byte[] m_aCRC = null;

        private byte[] m_aRoll = null;
        private byte[] m_aPitch = null;
        private byte[] m_aYaw = null;
        private byte[] m_aSway = null;
        private byte[] m_aSurge = null;
        private byte[] m_aHeave = null;
        private byte[] m_aSpeed = null;
        private byte[] m_aBlower = null;

        private MOTION_DATA m_dataMotion = new MOTION_DATA();
        private MOTION_EXTEND_DATA m_dataMotionExtend = new MOTION_EXTEND_DATA();

        private byte[] m_aMotionData = null;
        private int m_nMotionExtendData = 0;
        private int m_nEquipmentExtendData = 0;

        private int Communication_MakeCmd(int nMainCmd, int nSubCmd, byte chResponseType, int nDataLength, byte[] pDataBuf, byte[] pComDataFrame)
        {
            byte[] aMainCmd = BitConverter.GetBytes(nMainCmd);
            byte[] aSubCmd = BitConverter.GetBytes(nSubCmd);
            byte[] aResponseType = BitConverter.GetBytes(chResponseType);
            byte[] aDataLength = BitConverter.GetBytes(nDataLength);
            
            m_nCRC = 0;
            m_nIndex = 0;

            // Header
            pComDataFrame[m_nIndex] = (byte)0xFF;                               m_nIndex += 1;
            pComDataFrame[m_nIndex] = (byte)0x55;                               m_nIndex += 1;
            pComDataFrame[m_nIndex] = (byte)0xEE;                               m_nIndex += 1;
            pComDataFrame[m_nIndex] = (byte)0xAA;                               m_nIndex += 1;
            // Command            
            Array.Copy(aMainCmd, 0, pComDataFrame, m_nIndex, 4);                m_nIndex += 4;            // Main Cmd
            Array.Copy(aSubCmd, 0, pComDataFrame, m_nIndex, 4);                 m_nIndex += 4;            // Sub Cmd 
            Array.Copy(aResponseType, 0, pComDataFrame, m_nIndex, 1);           m_nIndex += 1;            // Response Type
            Array.Copy(aDataLength, 0, pComDataFrame, m_nIndex, 4);             m_nIndex += 4;            // Data Length
            Array.Copy(pDataBuf, 0, pComDataFrame, m_nIndex, nDataLength);      m_nIndex += nDataLength;  // Data
            // CRC16
            m_nCRC = AvSimDllCommSubFunc.MakeCRC16(pComDataFrame, m_nIndex);
            m_aCRC = BitConverter.GetBytes(m_nCRC);
            Array.Copy(m_aCRC, 0, pComDataFrame, m_nIndex, 2);                  m_nIndex += 2;

            return m_nIndex;
        }

        private UInt16 Communication_MakeCmd_CRC16(int nMainCmd, int nSubCmd, byte chResponseType, int nDataLength, byte[] pDataBuf, byte[] pComDataFrame)
        {
            m_nCRC = 0;
            m_nIndex = 0;
            m_nCmdCnt = 0;

            byte[] aMainCmd = BitConverter.GetBytes(nMainCmd);
            byte[] aSubCmd = BitConverter.GetBytes(nSubCmd);
            byte[] aResponseType = BitConverter.GetBytes(chResponseType);
            byte[] aDataLength = BitConverter.GetBytes(nDataLength);

            // Header
            pComDataFrame[m_nIndex] = (byte)0xFF;                           m_nIndex += 1;
            pComDataFrame[m_nIndex] = (byte)0x55;                           m_nIndex += 1;
            pComDataFrame[m_nIndex] = (byte)0xEE;                           m_nIndex += 1;
            pComDataFrame[m_nIndex] = (byte)0xAA;                           m_nIndex += 1;
            // Command
            Array.Copy(aMainCmd, 0, pComDataFrame, m_nIndex, 4);            m_nIndex += 4;   // Main Cmd
            Array.Copy(aSubCmd, 0, pComDataFrame, m_nIndex, 4);             m_nIndex += 4;   // Sub Cmd 
            Array.Copy(aResponseType, 0, pComDataFrame, m_nIndex, 1);       m_nIndex += 1;   // Response Type
            Array.Copy(aDataLength, 0, pComDataFrame, m_nIndex, 4);         m_nIndex += 4;   // Data Length
                                                                                             // DataLength까지 CRC계산을 하기 위해서 처리 해준다....
            m_nCRC = AvSimDllCommSubFunc.MakeCRC16(pComDataFrame, m_nIndex);                // CRC16

            return m_nCRC;
        }

        private void Communication_Parsing(byte[] pCommDataBuf, int[] pnResult)
        {
            m_nMainCmd = 0;
            m_nSubCmd = 0;
            m_chResponseType = 0;
            m_nDataLength = 0;
            Array.Clear(m_nDataBuf, 0, m_nDataBuf.Length);
            m_nIndex = 0;

            // Header
            m_nIndex += 0;
            
            // Command
            m_nIndex += 4; m_nMainCmd = BitConverter.ToInt32(pCommDataBuf, m_nIndex);     // Main Cmd
            m_nIndex += 4; m_nSubCmd = BitConverter.ToInt32(pCommDataBuf, m_nIndex);      // Sub Cmd
            m_nIndex += 4; m_chResponseType = pCommDataBuf[m_nIndex];                     // Response Type
            m_nIndex += 1; m_nDataLength = BitConverter.ToInt32(pCommDataBuf, m_nIndex);  // Data Length
            m_nIndex += 4;                                                            // Data

            if (m_nDataLength > 0)
                Buffer.BlockCopy(pCommDataBuf, m_nIndex, m_nDataBuf, 0, m_nDataLength);

            switch (m_nMainCmd)
            {
                case 0:
                    {
                        Console.WriteLine("ping");
                    }
                    break;
                case 1:
                    {
                        switch (m_nSubCmd)
                        {
                            case 10003: Buffer.BlockCopy(m_nDataBuf, 0, pnResult, 0, m_nDataLength); break;
                        }
                    }
                    break;
                case 9:
                    {
                        switch (m_nSubCmd)
                        {
                            case 5: Buffer.BlockCopy(m_nDataBuf, 0, pnResult, 0, 8); break;
                        }
                    }
                    break;
                case 3:
                    {
                        switch (m_nSubCmd)
                        {
                            case 5: Buffer.BlockCopy(m_nDataBuf, 0, pnResult, 0, 8); break;
                            case 13: pnResult[0] = m_nDataBuf[0]; break;
                            case 19: Buffer.BlockCopy(m_nDataBuf, 0, pnResult, 0, 9); break;
                            case 27:
                            case 29:
                            case 31:
                            case 33:
                            case 35:
                            case 37:
                            case 43:
                            case 45:
                            case 47:
                            case 49: Buffer.BlockCopy(m_nDataBuf, 0, pnResult, 0, m_nDataLength); break;
                            case 51: pnResult[0] = m_nDataBuf[0]; break;
                        }
                    }
                    break;
                case 103:
                    {
                        switch (m_nSubCmd)
                        {
                            case 5: Buffer.BlockCopy(m_nDataBuf, 0, pnResult, 0, 8); break;
                            case 13: pnResult[0] = m_nDataBuf[0]; break;
                            case 19: Buffer.BlockCopy(m_nDataBuf, 0, pnResult, 0, 9); break;
                            case 27:
                            case 29:
                            case 31:
                            case 33:
                            case 35:
                            case 37:
                            case 43:
                            case 45:
                            case 47:
                            case 49: Buffer.BlockCopy(m_nDataBuf, 0, pnResult, 0, m_nDataLength); break;
                            case 51: pnResult[0] = m_nDataBuf[0]; break;
                        }
                    }
                    break;
            }
        }

        private void Communication_Parsing(int nMainCommand, int nSubCommand, int nDataLength, byte[] pCommDataBuf, int[] pnResult)
        {
            switch (nMainCommand)
            {
                case 3:
                    {
                        switch (nSubCommand)
                        {
                            case 13: Buffer.BlockCopy(pCommDataBuf, 17, pnResult, 0, nDataLength); break;
                            case 19: Buffer.BlockCopy(pCommDataBuf, 17, pnResult, 0, nDataLength); break;
                            case 27:
                            case 29:
                            case 31:
                            case 33:
                            case 35:
                            case 37:
                            case 43:
                            case 45:
                            case 47:
                            case 49: Buffer.BlockCopy(pCommDataBuf, 17, pnResult, 0, nDataLength); break;
                            case 51: Buffer.BlockCopy(pCommDataBuf, 17, pnResult, 0, nDataLength); break;
                        }
                    }
                    break;
                case 103:
                    {
                        switch (nSubCommand)
                        {
                            case 13: pnResult[0] = pCommDataBuf[17]; break;
                            case 19: Buffer.BlockCopy(pCommDataBuf, 17, pnResult, 0, 9); break;
                            case 27:
                            case 29:
                            case 31:
                            case 33:
                            case 35:
                            case 37:
                            case 43:
                            case 45:
                            case 47:
                            case 49: Buffer.BlockCopy(pCommDataBuf, 17, pnResult, 0, nDataLength); break;
                            case 51: pnResult[0] = pCommDataBuf[17]; break;
                        }
                    }
                    break;
            }
        }

        private bool Communication_CmdSend_Sock(int nMainCmd, int nSubCmd, byte chResponseType, int nDataLength, byte[] pCommDataBuf, ref int[] pnResult)
        {
            Array.Clear(m_szRxBuf, 0, m_szRxBuf.Length);

            m_nRxCnt = 0;
            m_nTxCnt = 0;
            m_nOffset = 0;
            m_nRespMainCmd = 0;
            m_nRespSubCmd = 0;
            m_nRespDataLength = 0;
            m_nIndex = 0;

            m_nTxCnt = m_avSimDllComm.ComWrite(pCommDataBuf, nDataLength);

            if (m_nTxCnt == 0 || m_nTxCnt != nDataLength)
                return false;

            if (chResponseType == 0)
                return true;

            m_strMsg = string.Empty;
            m_strMsgBuf = string.Empty;
            m_nCommStateInput = eState.NONE;
            m_timer = DateTime.Now.Ticks;

            while (m_nCommStateInput == eState.NONE)
            {
                if (AvSimDllCommSubFunc.TickCheck(g_nTimeout, m_timer))
                {
                    m_nCommStateInput = eState.TIMEOUT;
                    break;
                }

                if (m_avSimDllComm.ComState())
                {
                    if ((m_nRxCnt = m_avSimDllComm.ComRead(m_szRxBuf, 1024)) > 0)
                    {
                        Array.Clear(m_aRecvCmd, 0, m_aRecvCmd.Length);
                        m_isResult = false;
                        while (22 + m_nOffset < m_nRxCnt)
                        {
                            //  nMainCmd
                            Array.Copy(m_szRxBuf, 7 + m_nOffset - 3, m_aRecvCmd, 0, 4);
                            m_nRespMainCmd = BitConverter.ToInt32(m_aRecvCmd, 0);

                            //  nSubCmd
                            Array.Copy(m_szRxBuf, 11 + m_nOffset - 3, m_aRecvCmd, 0, 4);
                            m_nRespSubCmd = BitConverter.ToInt32(m_aRecvCmd, 0);

                            //  nRespDataLength
                            Array.Copy(m_szRxBuf, 16 + m_nOffset - 3, m_aRecvCmd, 0, 4);
                            m_nRespDataLength = BitConverter.ToInt32(m_aRecvCmd, 0);

                            if ((byte)m_szRxBuf[m_nIndex + m_nOffset + 0] == (byte)0xFF && (byte)m_szRxBuf[m_nIndex + m_nOffset + 1] == (byte)0x55 &&
                                (byte)m_szRxBuf[m_nIndex + m_nOffset + 2] == (byte)0xEE && (byte)m_szRxBuf[m_nIndex + m_nOffset + 3] == (byte)0xAA &&
                                (m_nRespMainCmd == (nMainCmd + 1)) &&
                                (m_nRespSubCmd == (nSubCmd + 1)) &&
                                (m_nRespDataLength > 0))
                            {
                                m_nIndex = m_nRespDataLength + 19;
                                m_isResult = true;
                                break;
                            }
                            else
                                m_nOffset++;
                        }

                        if (!m_isResult)
                            return false;

                        //  CRC
                        m_nCRC1 = 0;
                        m_nCRC2 = 0;

                        Array.Copy(m_szRxBuf, m_nIndex + m_nOffset - 2, m_aRecvCmd, 0, 2);
                        m_nCRC1 = BitConverter.ToUInt16(m_aRecvCmd, 0);
                        m_nCRC2 = AvSimDllCommSubFunc.MakeCRC16(m_szRxBuf, m_nOffset, m_nIndex + m_nOffset - 2);

                        if (m_nCRC1 == m_nCRC2)
                        {
                            Buffer.BlockCopy(m_szRxBuf, 17, pnResult, 0, m_nRespDataLength);
                            m_nCommStateInput = eState.SUCCESS;
                            return true;
                        }
                        else
                        {
                            m_nCommStateInput = eState.ERROR;
                            return false;
                        }
                    }
                    else
                    {
                        if (m_nRxCnt == -1)
                        {
                            m_nCommStateInput = eState.ERROR;
                            return false;
                        }
                    }
                }
            }

            return m_nCommStateInput != eState.SUCCESS ? false : true;
        }

        private bool Communication_CmdSend_Sock(int nMainCmd, int nSubCmd, byte chResponseType, int nDataLength, byte[] pCommDataBuf, ref int[] pnResult, int nRecvPacketLength)
        {
            Array.Clear(m_szRxBuf, 0, m_szRxBuf.Length);

            m_nRxCnt = 0;
            m_nTxCnt = 0;
            m_nOffset = 0;
            m_nRespMainCmd = 0;
            m_nRespSubCmd = 0;
            m_nRespDataLength = 0;
            m_nIndex = 0;

            m_nTxCnt = m_avSimDllComm.ComWrite(pCommDataBuf, nDataLength);
            if (m_nTxCnt == 0 || m_nTxCnt != nDataLength)
                return false;

            if (chResponseType == 0)
                return true;

            m_strMsg = string.Empty;
            m_strMsgBuf = string.Empty;
            m_nCommStateInput = eState.NONE;
            m_timer = DateTime.Now.Ticks;

            while (m_nCommStateInput == eState.NONE)
            {
                if (AvSimDllCommSubFunc.TickCheck(g_nTimeout, m_timer))
                {
                    m_nCommStateInput = eState.TIMEOUT;
                    return false;
                }

                if (m_avSimDllComm.ComState())
                {
                    if ((m_nRxCnt = m_avSimDllComm.ComRead(m_szRxBuf, 1024, nRecvPacketLength)) > 0)
                    {
                        Array.Clear(m_aRecvCmd, 0, m_aRecvCmd.Length);
                        m_isResult = false;

                        while (22 + m_nOffset < m_nRxCnt)
                        {
                            //  nMainCmd
                            Array.Copy(m_szRxBuf, 7 + m_nOffset - 3, m_aRecvCmd, 0, 4);
                            m_nRespMainCmd = BitConverter.ToInt32(m_aRecvCmd, 0);

                            //  nSubCmd
                            Array.Copy(m_szRxBuf, 11 + m_nOffset - 3, m_aRecvCmd, 0, 4);
                            m_nRespSubCmd = BitConverter.ToInt32(m_aRecvCmd, 0);

                            //  nRespDataLength
                            Array.Copy(m_szRxBuf, 16 + m_nOffset - 3, m_aRecvCmd, 0, 4);
                            m_nRespDataLength = BitConverter.ToInt32(m_aRecvCmd, 0);

                            if ((byte)m_szRxBuf[m_nIndex + m_nOffset + 0] == (byte)0xFF && (byte)m_szRxBuf[m_nIndex + m_nOffset + 1] == (byte)0x55 &&
                                (byte)m_szRxBuf[m_nIndex + m_nOffset + 2] == (byte)0xEE && (byte)m_szRxBuf[m_nIndex + m_nOffset + 3] == (byte)0xAA &&
                                (m_nRespMainCmd == (nMainCmd + 1)) && (m_nRespSubCmd == (nSubCmd + 1)) && (m_nRespDataLength > 0))
                            {
                                m_nIndex = m_nRespDataLength + 19;
                                m_isResult = true;
                                break;
                            }
                            else
                                m_nOffset++;
                        }

                        if (!m_isResult)
                            return false;

                        //  CRC
                        m_nCRC1 = 0;
                        m_nCRC2 = 0;

                        Array.Copy(m_szRxBuf, m_nIndex + m_nOffset - 2, m_aRecvCmd, 0, 2);
                        m_nCRC1 = BitConverter.ToUInt16(m_aRecvCmd, 0);
                        m_nCRC2 = AvSimDllCommSubFunc.MakeCRC16(m_szRxBuf, m_nOffset, m_nIndex + m_nOffset - 2);
                        
                        if (m_nCRC1 == m_nCRC2)
                        {
                            Buffer.BlockCopy(m_szRxBuf, 17, pnResult, 0, m_nRespDataLength);
                            m_nCommStateInput = eState.SUCCESS;
                            return true;
                        }
                        else
                        {
                            m_nCommStateInput = eState.ERROR;
                            return false;
                        }
                    }
                    else
                    {
                        if (m_nRxCnt == -1)
                        {
                            m_nCommStateInput = eState.ERROR;
                            return false;
                        }
                    }
                }
            }

            return m_nCommStateInput != eState.SUCCESS ? false : true;
        }

        public bool Initial()
        {
            EnvironmentParameterXml xml = null;
            Util.Load<EnvironmentParameterXml>(out xml, ENVIRONMENT_INI_FILE_NAME);

            if (null == xml)
                return false;

            m_isResult = false;
            g_nType = xml.CommunicationType.Type;
            g_nTimeout = xml.ResponseTimeout.LimitTime;
            g_nComPort = (eComPort)Enum.ToObject(typeof(eComPort), xml.ComPort.Port);
            g_strRemoteIP = xml.UdpParameter.IP;
            g_nRemotePort = xml.UdpParameter.Port;

            m_avSimDllComm = Factory.CreateAvSimDllCommSocket(xml.OsType.Type);
            if (null == m_avSimDllComm)
                return false;

            switch (g_nType)
            {
                case 0: m_isResult = false; break; //bResult = ComOpen(g_nComPort); break;
                case 1: m_isResult = m_avSimDllComm.ComOpen(g_strRemoteIP, g_nRemotePort); break;
            }

            return m_isResult;
        }

        public bool Initial(string strIP, int nPort, eOS os, int nTimeout = 500)
        {
            m_isResult = false;
            g_nType = 1;
            g_nTimeout = nTimeout;
            g_strRemoteIP = strIP;
            g_nRemotePort = nPort;

            m_avSimDllComm = Factory.CreateAvSimDllCommSocket(os);
            if (null == m_avSimDllComm)
                return false;

            switch (g_nType)
            {
                case 0: m_isResult = false; break; //bResult = ComOpen(g_nComPort); break;
                case 1: m_isResult = m_avSimDllComm.ComOpen(g_strRemoteIP, g_nRemotePort); break;
            }

            return m_isResult;
        }

        public bool Initial_Sock(string szRemoteIP, int nPort, int nTimeout)
        {
            g_strRemoteIP = szRemoteIP;
	        g_nRemotePort = nPort;

	        g_nTimeout = nTimeout;

	        return m_avSimDllComm.ComOpen(g_strRemoteIP, g_nRemotePort);
        }

        public bool Destroy()
        {
            m_isResult = false;
            switch (g_nType)
            {
                case 0: m_isResult = false; break; //bResult = ComClose(g_nComPort); break;
                case 1: m_isResult = m_avSimDllComm.ComClose(); break;
            }

            if (m_isResult)
            {
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

            return m_isResult;
        }

        public bool Destroy_Sock()
        {
            return m_avSimDllComm.ComClose();
        }

        public bool State()
        {
            m_isResult = false;

            switch (g_nType)
            {
                case 0: m_isResult = false; break; // bResult = ComState(g_nComPort); break;
                case 1: m_isResult = m_avSimDllComm.ComState(); break;
            }

            return m_isResult;
        }

        public bool State_Sock()
        {
            return m_avSimDllComm.ComState();
        }

        public bool Ping()
        {
            m_isResult = false;

            switch (g_nType)
            {
                case 0: m_isResult = false; break; // bResult = MotionControl__Ping_Port(g_nComPort); break;
                case 1: m_isResult = Ping_Sock(); break;
            }
            return m_isResult;
        }

        public bool Ping_Sock(bool bResp = true)
        {
            Array.Clear(m_nResult, 0, m_nResult.Length);
            Array.Clear(m_aDataBuf, 0, m_aDataBuf.Length);
            Array.Clear(m_szComDataFrame, 0, m_szComDataFrame.Length);

            m_nMainCmd = 0;
            m_nSubCmd = 0;
            m_chResponseType = (byte)(bResp ? eDefine.AV_SIM__RESPONSE_ENABLE : eDefine.AV_SIM__RESPONSE_DISABLE);
            m_nDataLength = 0;
            m_nSendDataLength = Communication_MakeCmd(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nDataLength, m_aDataBuf, m_szComDataFrame);

            m_isResult = false;
            if (m_isMode)
                m_isResult = Communication_CmdSend_Sock(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nSendDataLength, m_szComDataFrame, ref m_nResult, m_nSendDataLength);
            else
                m_isResult = Communication_CmdSend_Sock(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nSendDataLength, m_szComDataFrame, ref m_nResult);

            return m_isResult;
        }

        public bool DO(int nDO)
        {
            m_isResult = false;
            switch (g_nType)
            {
                case 0: m_isResult = false; break; // bResult = MotionControl__DO_Port(g_nComPort, nDO); break;
                case 1: m_isResult = DO_Sock(nDO); break;
            }
            return m_isResult;
        }

        public bool DO_Sock(int nDO)
        {
            Array.Clear(m_aDataBuf, 0, m_aDataBuf.Length);
            Array.Clear(m_szComDataFrame, 0, m_szComDataFrame.Length);

            m_nMainCmd = 2;
            m_nSubCmd = 10;
            m_chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            m_nDataLength = sizeof(int);

            m_aDO = BitConverter.GetBytes(nDO);
            Array.Copy(m_aDO, 0, m_aDataBuf, 0, 4);

            m_nSendDataLength = Communication_MakeCmd(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nDataLength, m_aDataBuf, m_szComDataFrame);

            m_nTxCnt = m_avSimDllComm.ComWrite(m_szComDataFrame, m_nSendDataLength);

            return (m_nTxCnt != m_nSendDataLength) ? false : true;
        }

        public int GetDI()
        {
            m_nDI = -1;
            switch (g_nType)
            {
                case 0: m_nDI = -1; break; // nDI = MotionControl__GetDI_Port(g_nComPort); break;
                case 1: m_nDI = GetDI_Sock(); break;
            }

            return m_nDI;
        }

        public int GetDI_Sock()
        {
            Array.Clear(m_nResult, 0, m_nResult.Length);
            Array.Clear(m_aDataBuf, 0, m_aDataBuf.Length);
            Array.Clear(m_szComDataFrame, 0, m_szComDataFrame.Length);

            m_nMainCmd = 2;
            m_nSubCmd = 12;
            m_chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            m_nDataLength = 0;
            m_nSendDataLength = Communication_MakeCmd(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nDataLength, m_aDataBuf, m_szComDataFrame);

            bool isResult = false;
            if (m_isMode)
                isResult = Communication_CmdSend_Sock(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nSendDataLength, m_szComDataFrame, ref m_nResult, m_nSendDataLength + 4);
            else
                isResult = Communication_CmdSend_Sock(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nSendDataLength, m_szComDataFrame, ref m_nResult);

            if (!isResult)
                return -1;
            else
                return m_nResult[0];
        }

        public int GetDO()
        {
            m_nDO = -1;
            switch (g_nType)
            {
                case 0: m_nDO = -1; break; // nDO = MotionControl__GetDO_Port(g_nComPort); break;
                case 1: m_nDO = GetDO_Sock(); break;
            }

            return m_nDO;
        }

        public int GetDO_Sock()
        {
            Array.Clear(m_nResult, 0, m_nResult.Length);
            Array.Clear(m_aDataBuf, 0, m_aDataBuf.Length);
            Array.Clear(m_szComDataFrame, 0, m_szComDataFrame.Length);

            m_nMainCmd = 2;
            m_nSubCmd = 50;
            m_chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            m_nDataLength = 0;
            m_nSendDataLength = Communication_MakeCmd(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nDataLength, m_aDataBuf, m_szComDataFrame);

            bool isResult = false;
            if (m_isMode)
                isResult = Communication_CmdSend_Sock(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nSendDataLength, m_szComDataFrame, ref m_nResult, m_nSendDataLength + 4);
            else
                isResult = Communication_CmdSend_Sock(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nSendDataLength, m_szComDataFrame, ref m_nResult);

            if (!isResult)
                return -1;
            else
                return m_nResult[0];
        }

        public bool EQExtendData(out EQUIPMENT_EXTEND_DATA pEQData, bool bResp = true)
        {
            m_isResult = false;
            pEQData = null;
            switch (g_nType)
            {
                case 0: m_isResult = false; break; // bResult = MotionControl__EQExtendData_Port(g_nComPort, pEQData, bResp); break;
                case 1: m_isResult = EQExtendData_Sock(out pEQData, bResp); break;
            }
            return m_isResult;
        }

        public bool EQExtendData_Sock(out EQUIPMENT_EXTEND_DATA pEQExtendData, bool bResp = true)
        {
            pEQExtendData = null;

            Array.Clear(m_nResult, 0, m_nResult.Length);
            Array.Clear(m_aDataBuf, 0, m_aDataBuf.Length);
            Array.Clear(m_szComDataFrame, 0, m_szComDataFrame.Length);

            m_nMainCmd = 2;
            m_nSubCmd = 48;
            m_chResponseType = (byte)(bResp ? eDefine.AV_SIM__RESPONSE_ENABLE : eDefine.AV_SIM__RESPONSE_DISABLE);
            m_nDataLength = Marshal.SizeOf(typeof(EQUIPMENT_EXTEND_DATA));

            m_nSendDataLength = Communication_MakeCmd(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nDataLength, m_aDataBuf, m_szComDataFrame);

            m_isResult = false;
            if (m_isMode)
                m_isResult = Communication_CmdSend_Sock(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nSendDataLength, m_szComDataFrame, ref m_nResult, m_nSendDataLength);
            else
                m_isResult = Communication_CmdSend_Sock(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nSendDataLength, m_szComDataFrame, ref m_nResult);

            if (m_isResult)
                pEQExtendData = Util.Array2Struct<int, EQUIPMENT_EXTEND_DATA>(m_nResult);

            return m_isResult;
        }

        public bool DOF_and_Blower(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower)
        {
            m_isResult = false;
            switch (g_nType)
            {
                case 0: m_isResult = false; break; // bResult = MotionControl__DOF_and_Blower_Port(g_nComPort, nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nBlower); break;
                case 1: m_isResult = DOF_and_Blower_Sock(nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nBlower); break;
            }
            return m_isResult;
        }

        public bool DOF_and_Blower_Sock(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower)
        {
            if (nSpeed < 1)
                return false;

            Array.Clear(m_aDataBuf, 0, m_aDataBuf.Length);
            Array.Clear(m_szComDataFrame, 0, m_szComDataFrame.Length);

            m_nMainCmd = 2;
            m_nSubCmd = 4;
            m_chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            m_nDataLength = Marshal.SizeOf(typeof(MOTION_DATA));
            m_nIndex = 0;

            m_aRoll = BitConverter.GetBytes(nRoll);
            m_aPitch = BitConverter.GetBytes(nPitch);
            m_aYaw = BitConverter.GetBytes(nYaw);
            m_aSway = BitConverter.GetBytes(nSway);
            m_aSurge = BitConverter.GetBytes(nSurge);
            m_aHeave = BitConverter.GetBytes(nHeave);
            m_aSpeed = BitConverter.GetBytes(nSpeed);
            m_aBlower = BitConverter.GetBytes(nBlower);

            Array.Copy(m_aRoll, 0, m_aDataBuf, m_nIndex, 4);      m_nIndex += 4;
            Array.Copy(m_aPitch, 0, m_aDataBuf, m_nIndex, 4);     m_nIndex += 4;
            Array.Copy(m_aYaw, 0, m_aDataBuf, m_nIndex, 4);       m_nIndex += 4;
            Array.Copy(m_aSway, 0, m_aDataBuf, m_nIndex, 4);      m_nIndex += 4;
            Array.Copy(m_aSurge, 0, m_aDataBuf, m_nIndex, 4);     m_nIndex += 4;
            Array.Copy(m_aHeave, 0, m_aDataBuf, m_nIndex, 4);     m_nIndex += 4;
            Array.Copy(m_aSpeed, 0, m_aDataBuf, m_nIndex, 4);     m_nIndex += 4;
            Array.Copy(m_aBlower, 0, m_aDataBuf, m_nIndex, 4);

            m_nSendDataLength = Communication_MakeCmd(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nDataLength, m_aDataBuf, m_szComDataFrame);

            m_nTxCnt = m_avSimDllComm.ComWrite(m_szComDataFrame, m_nSendDataLength);

            return (m_nTxCnt != m_nSendDataLength) ? false : true;
        }

        public bool V2_DOF_and_Blower(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower)
        {
            m_dataMotion.Roll = nRoll;
            m_dataMotion.Pitch = nPitch;
            m_dataMotion.Yaw = nYaw;
            m_dataMotion.Sway = nSway;
            m_dataMotion.Surge = nSurge;
            m_dataMotion.Heave = nHeave;
            m_dataMotion.MotionSpeed = nSpeed;
            m_dataMotion.Blower = nBlower;

            return V2_DOF_and_Blower(m_dataMotion);
        }

        public bool V2_DOF_and_Blower(MOTION_DATA stMotionData)
        {
            m_isResult = false;
            switch (g_nType)
            {
                case 0: m_isResult = false; break; // bResult = MotionControlV2__DOF_and_Blower_Port(g_nComPort, stMotionData); break;
                case 1: m_isResult = V2_DOF_and_Blower_Sock(stMotionData); break;
            }
            return m_isResult;
        }

        public bool V2_DOF_and_Blower_Sock(MOTION_DATA stMotionData)
        {
            Array.Clear(m_aDataBuf, 0, m_aDataBuf.Length);
            Array.Clear(m_szComDataFrame, 0, m_szComDataFrame.Length);

            m_nMainCmd = 102;
            m_nSubCmd = 4;
            m_chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            m_nDataLength = Marshal.SizeOf(typeof(MOTION_DATA));

            byte[] aMotionData = Util.Struct2Byte2(stMotionData);
            Array.Copy(aMotionData, 0, m_aDataBuf, 0, m_nDataLength);

            m_nSendDataLength = Communication_MakeCmd(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nDataLength, m_aDataBuf, m_szComDataFrame);

            m_nTxCnt = m_avSimDllComm.ComWrite(m_szComDataFrame, m_nSendDataLength);

            return (m_nTxCnt != m_nSendDataLength) ? false : true;
        }

        public bool V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis(
            int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower,
            int nRolling, int nRollingSpeed, int nRollingMode,
            int nPitching, int nPitchingSpeed, int nPitchingMode,
            int nYawing, int nYawingSpeed, int nYawingMode,
            out EQUIPMENT_DATA pEquipmentData, bool bResp = true)
        {
            pEquipmentData = null;

            m_dataMotionExtend.Roll = nRoll;
            m_dataMotionExtend.Pitch = nPitch;
            m_dataMotionExtend.Yaw = nYaw;
            m_dataMotionExtend.Sway = nSway;
            m_dataMotionExtend.Surge = nSurge;
            m_dataMotionExtend.Heave = nHeave;
            m_dataMotionExtend.MotionSpeed = nSpeed;
            m_dataMotionExtend.Blower = nBlower;
            m_dataMotionExtend.Rolling = nRolling;
            m_dataMotionExtend.RollingSpeed = nRollingSpeed;
            m_dataMotionExtend.RollingMode = nRollingMode;
            m_dataMotionExtend.Pitching = nPitching;
            m_dataMotionExtend.PitchingSpeed = nPitchingSpeed;
            m_dataMotionExtend.PitchingMode = nPitchingMode;
            m_dataMotionExtend.Yawing = nYawing;
            m_dataMotionExtend.YawingSpeed = nYawingSpeed;
            m_dataMotionExtend.YawingMode = nYawingMode;

            return V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis(m_dataMotionExtend, out pEquipmentData, bResp);
        }

        public bool V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis(MOTION_EXTEND_DATA stMotionData, out EQUIPMENT_DATA pEquipmentData, bool bResp = true)
        {
            m_isResult = false;
            pEquipmentData = null;
            switch (g_nType)
            {
                case 0: m_isResult = false; break; // bResult = MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_Port(g_nComPort, stMotionData, pEQData, bResp); break;
                case 1: m_isResult = V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_Sock(stMotionData, out pEquipmentData, bResp); break;
            }
            return m_isResult;
        }

        public bool V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_Sock(MOTION_EXTEND_DATA stMotionData, out EQUIPMENT_DATA pEquipmentData, bool bResp = true)
        {
            pEquipmentData = null;

            Array.Clear(m_nResult, 0, m_nResult.Length);
            Array.Clear(m_aDataBuf, 0, m_aDataBuf.Length);
            Array.Clear(m_szComDataFrame, 0, m_szComDataFrame.Length);

            m_nMainCmd = 102;
            m_nSubCmd = 42;
            m_chResponseType = (byte)(bResp ? eDefine.AV_SIM__RESPONSE_ENABLE : eDefine.AV_SIM__RESPONSE_DISABLE);
            m_nDataLength = Marshal.SizeOf(typeof(MOTION_EXTEND_DATA)) + Marshal.SizeOf(typeof(EQUIPMENT_DATA));

            m_aMotionData = Util.Struct2Byte2(stMotionData);
            m_nMotionExtendData = Marshal.SizeOf(typeof(MOTION_EXTEND_DATA));
            Array.Copy(m_aMotionData, 0, m_aDataBuf, 0, m_nMotionExtendData);

            m_nSendDataLength = Communication_MakeCmd(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nDataLength, m_aDataBuf, m_szComDataFrame);

            m_isResult = false;
            if (m_isMode)
                m_isResult = Communication_CmdSend_Sock(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nSendDataLength, m_szComDataFrame, ref m_nResult, m_nSendDataLength);
            else
                m_isResult = Communication_CmdSend_Sock(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nSendDataLength, m_szComDataFrame, ref m_nResult);

            if (m_isResult)
            {
                Array.Copy(m_nResult, m_nMotionExtendData / 4, m_nResult, 0, Marshal.SizeOf(typeof(EQUIPMENT_DATA)));
                pEquipmentData = Util.Array2Struct<int, EQUIPMENT_DATA>(m_nResult);
            }

            return m_isResult;
        }

        public bool V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm(
            int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower,
            int nRolling, int nRollingSpeed, int nRollingMode,
            int nPitching, int nPitchingSpeed, int nPitchingMode,
            int nYawing, int nYawingSpeed, int nYawingMode, 
            out EQUIPMENT_EXTEND_DATA pEquipmentExtendData, bool bResp = true)
        {
            pEquipmentExtendData = null;

            m_dataMotionExtend.Roll = nRoll;
            m_dataMotionExtend.Pitch = nPitch;
            m_dataMotionExtend.Yaw = nYaw;
            m_dataMotionExtend.Sway = nSway;
            m_dataMotionExtend.Surge = nSurge;
            m_dataMotionExtend.Heave = nHeave;
            m_dataMotionExtend.MotionSpeed = nSpeed;
            m_dataMotionExtend.Blower = nBlower;
            m_dataMotionExtend.Rolling = nRolling;
            m_dataMotionExtend.RollingSpeed = nRollingSpeed;
            m_dataMotionExtend.RollingMode = nRollingMode;
            m_dataMotionExtend.Pitching = nPitching;
            m_dataMotionExtend.PitchingSpeed = nPitchingSpeed;
            m_dataMotionExtend.PitchingMode = nPitchingMode;
            m_dataMotionExtend.Yawing = nYawing;
            m_dataMotionExtend.YawingSpeed = nYawingSpeed;
            m_dataMotionExtend.YawingMode = nYawingMode;

            return V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm(m_dataMotionExtend, out pEquipmentExtendData, bResp);
        }

        public bool V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm(MOTION_EXTEND_DATA stMotionData, out EQUIPMENT_EXTEND_DATA pEquipmentExtendData, bool bResp = true)
        {
            m_isResult = false;
            pEquipmentExtendData = null;

            switch (g_nType)
            {
                case 0: m_isResult = false; break; // bResult = MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm_Port(g_nComPort, stMotionData, pEQData, bResp); break;
                case 1: m_isResult = V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm_Sock(stMotionData, out pEquipmentExtendData, bResp); break;
            }
            return m_isResult;
        }

        public bool V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm_Sock(MOTION_EXTEND_DATA stMotionData, out EQUIPMENT_EXTEND_DATA pEquipmentExtendData, bool bResp = true)
        {
            pEquipmentExtendData = null;

            Array.Clear(m_nResult, 0, m_nResult.Length);
            Array.Clear(m_aDataBuf, 0, m_aDataBuf.Length);
            Array.Clear(m_szComDataFrame, 0, m_szComDataFrame.Length);

            m_nMainCmd = 102;
            m_nSubCmd = 46;
            m_chResponseType = (byte)(bResp ? eDefine.AV_SIM__RESPONSE_ENABLE : eDefine.AV_SIM__RESPONSE_DISABLE);
            m_nMotionExtendData = Marshal.SizeOf(typeof(MOTION_EXTEND_DATA));
            m_nEquipmentExtendData = Marshal.SizeOf(typeof(EQUIPMENT_EXTEND_DATA));
            m_nDataLength = m_nMotionExtendData + m_nEquipmentExtendData;

            m_aMotionData = Util.Struct2Byte2(stMotionData);
            Array.Copy(m_aMotionData, 0, m_aDataBuf, 0, m_nMotionExtendData);

            m_nSendDataLength = Communication_MakeCmd(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nDataLength, m_aDataBuf, m_szComDataFrame);

            m_isResult = false;
            if (m_isMode)
                m_isResult = Communication_CmdSend_Sock(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nSendDataLength, m_szComDataFrame, ref m_nResult, m_nSendDataLength);
            else
                m_isResult = Communication_CmdSend_Sock(m_nMainCmd, m_nSubCmd, m_chResponseType, m_nSendDataLength, m_szComDataFrame, ref m_nResult);

            if (m_isResult)
            {
                Array.Copy(m_nResult, m_nMotionExtendData / 4, m_nResult, 0, m_nEquipmentExtendData);
                pEquipmentExtendData = Util.Array2Struct<int, EQUIPMENT_EXTEND_DATA>(m_nResult);
            }

            return m_isResult;
        }
        
        #region Wifi
        public bool ReadTcpSocketMode(ref int pData)
        {
            bool bResult = false;

            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = ReadTcpSocketMode_Port(out pData); break;
                case 1: bResult = ReadTcpSocketMode_Sock(ref pData); break;
            }
            return bResult;
        }

        public bool ReadTcpSocketMode_Sock(ref int pData)
        {
            pData = 0;

            bool isResult = false;
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];
            int[] nResultData = new int[128];

            int nMainCmd = 1000;
            int nSubCmd = 10002;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            int nDataLength = 4;
            ushort nCRC = 0;
            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);
            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            isResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData);

            if (isResult)
                pData = nResultData[0];

            return isResult;
        }

        public bool WriteTcpSocketMode(int nData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = WriteTcpSocketMode_Port(g_nComPort, nData); break;
                case 1: bResult = WriteTcpSocketMode_Sock(nData); break;
            }
            return bResult;
        }

        public bool WriteTcpSocketMode_Sock(int nData)
        {
            bool isResult = false;
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20002;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            int nDataLength = 8;
            ushort nCRC = 0;

            byte[] aData = BitConverter.GetBytes(nData);
            Array.Copy(aData, 0, nDataBuf, 4, 4);
                        
            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool ReadWifiMode(ref int pData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = ReadWifiMode_Port(g_nComPort, pData); break;
                case 1: bResult = ReadWifiMode_Sock(ref pData); break;
            }
            return bResult;
        }

        public bool ReadWifiMode_Sock(ref int pData)
        {
            pData = 0;
            bool bResult = false;
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];
            int[] nResultData = new int[128];

            int nMainCmd = 1000;
            int nSubCmd = 10004;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            ushort nCRC = 0;
            int nDataLength = 4;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData);

            if (bResult)
                pData = nResultData[0];

            return bResult;
        }

        public bool WriteWifiMode(int nData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = WriteWifiMode_Port(g_nComPort, nData); break;
                case 1: bResult = WriteWifiMode_Sock(nData); break;
            }
            return bResult;
        }

        public bool WriteWifiMode_Sock(int nData)
        {
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20004;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            int nDataLength = 8;
            ushort nCRC = 0;

            byte[] aData = BitConverter.GetBytes(nData);
            Array.Copy(aData, 0, nDataBuf, 4, 4);

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool ReadWifiChannel(ref int pData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = ReadWifiChannel_Port(g_nComPort, pData); break;
                case 1: bResult = ReadWifiChannel_Sock(ref pData); break;
            }
            return bResult;
        }

        public bool ReadWifiChannel_Sock(ref int pData)
        {
            pData = 0;
            bool bResult = false;
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];
            int[] nResultData = new int[128];

            int nMainCmd = 1000;
            int nSubCmd = 10026;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            ushort nCRC = 0;
            int nDataLength = 4;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData);

            if (bResult)
                pData = nResultData[0];

            return bResult;
        }

        public bool WriteWifiChannel(int nData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = WriteWifiChannel_Port(g_nComPort, nData); break;
                case 1: bResult = WriteWifiChannel_Sock(nData); break;
            }
            return bResult;
        }

        public bool WriteWifiChannel_Sock(int nData)
        {
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20026;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            int nDataLength = 8;
            ushort nCRC = 0;

            byte[] aData = BitConverter.GetBytes(nData);
            Array.Copy(aData, 0, nDataBuf, 4, 4);

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool ReadBluetoothDeviceId(ref string szData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = ReadBluetoothDeviceId_Port(g_nComPort, szData); break;
                case 1: bResult = ReadBluetoothDeviceId_Sock(ref szData); break;
            }
            return bResult;
        }

        public bool ReadBluetoothDeviceId_Sock(ref string szData)
        {
            szData = string.Empty;
            bool bResult = false;
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];
            int[] nResultData = new int[128];

            int nMainCmd = 1000;
            int nSubCmd = 10006;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            ushort nCRC = 0;
            int nDataLength = 4;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData);

            byte[] aString1 = BitConverter.GetBytes(nResultData[0]);
            byte[] aString2 = BitConverter.GetBytes(nResultData[1]);

            Array.Clear(szComDataFrame, 0, szComDataFrame.Length);
            Array.Copy(aString1, 0, szComDataFrame, 0, 4);
            Array.Copy(aString2, 0, szComDataFrame, 4, 4);

            if (bResult)
                szData = BitConverter.ToString(szComDataFrame, 0, 8);

            return bResult;
        }

        public bool WriteBluetoothDeviceId(string szData)
        {

            bool bResult = false;
	        switch (g_nType)
	        {
	            case 0: bResult = false; break; // bResult = WriteBluetoothDeviceId_Port(g_nComPort, szData); break;
                case 1:	bResult = WriteBluetoothDeviceId_Sock(szData); break;
	        }
	        return bResult;
        }

        public bool WriteBluetoothDeviceId_Sock(string szData)
        {
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20006;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 4 + 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            byte[] aData = Encoding.Default.GetBytes(szData);
            Array.Copy(aData, 0, nDataBuf, 4, aData.Length);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool ReadBluetoothDevicePassword(ref string szData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = ReadBluetoothDevicePassword_Port(g_nComPort, szData); break;
                case 1: bResult = ReadBluetoothDevicePassword_Sock(ref szData); break;
            }
            return bResult;
        }

        public bool ReadBluetoothDevicePassword_Sock(ref string szData)
        {
            szData = string.Empty;
            bool bResult = false;
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];
            int[] nResultData = new int[128];

            int nMainCmd = 1000;
            int nSubCmd = 10028;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            ushort nCRC = 0;
            int nDataLength = 4;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData);

            byte[] aString1 = BitConverter.GetBytes(nResultData[0]);
            byte[] aString2 = BitConverter.GetBytes(nResultData[1]);

            Array.Clear(szComDataFrame, 0, szComDataFrame.Length);
            Array.Copy(aString1, 0, szComDataFrame, 0, 4);
            Array.Copy(aString2, 0, szComDataFrame, 4, 4);

            if (bResult)
                szData = BitConverter.ToString(szComDataFrame, 0, 8);

            return bResult;
        }

        public bool WriteBluetoothDevicePassword(string szData)
        {
            bool bResult = false;
	        switch (g_nType)
	        {
	            case 0: bResult = false; break; // bResult = WriteBluetoothDevicePassword_Port(g_nComPort, szData); break;
                case 1:	bResult = WriteBluetoothDevicePassword_Sock(szData); break;
	        }
	        return bResult;
        }

        public bool WriteBluetoothDevicePassword_Sock(string szData)
        {
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20028;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 4 + 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            byte[] aData = Encoding.Default.GetBytes(szData);
            Array.Copy(aData, 0, nDataBuf, 4, aData.Length);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool ReadSocketServerIP(ref string szData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = ReadSocketServerIP_Port(g_nComPort, szData); break;
                case 1: bResult = ReadSocketServerIP_Sock(ref szData); break;
            }
            return bResult;
        }

        public bool ReadSocketServerIP_Sock(ref string szData)
        {
            bool bResult = false;
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];
            int[] nResultData = new int[128];

            int nMainCmd = 1000;
            int nSubCmd = 10010;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            ushort nCRC = 0;
            int nDataLength = 4;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData);

            byte[] aResult = BitConverter.GetBytes(nResultData[0]);
            szData = string.Format("{0}.{1}.{2}.{3}", (int)aResult[0], (int)aResult[1], (int)aResult[2], (int)aResult[3]);

            return bResult;
        }
        
        public bool WriteSocketServerIP(string szData)
        {
            bool bResult = false;
	        switch (g_nType)
	        {
	            case 0: bResult = false; break; // bResult = WriteSocketServerIP_Port(g_nComPort, szData); break;
                case 1:	bResult = WriteSocketServerIP_Sock(szData); break;
	        }
	        return bResult;
        }

        public bool WriteSocketServerIP(byte[] aData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = WriteSocketServerIP_Port(g_nComPort, szData); break;
                case 1: bResult = WriteSocketServerIP_Sock(aData); break;
            }
            return bResult;
        }

        public bool WriteSocketServerIP_Sock(string szData)
        {
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20010;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            string[] aData = szData.Split('.');
            if (4 != aData.Length)
                return false;

            for (int i = 0; i < 4; i++)
            {
                nDataBuf[4 + i] = byte.Parse(aData[i]);
            }

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool WriteSocketServerIP_Sock(byte[] aData)
        {
            if (4 != aData.Length)
                return false;

            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20010;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            for (int i = 0; i < 4; i++)
            {
                nDataBuf[4 + i] = aData[i];
            }

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool ReadSocketServerSubnetMask(ref string szData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = ReadSocketServerSubnetMask_Port(g_nComPort, szData); break;
                case 1: bResult = ReadSocketServerSubnetMask_Sock(ref szData); break;
            }
            return bResult;
        }

        public bool ReadSocketServerSubnetMask_Sock(ref string szData)
        {
            bool bResult = false;
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];
            int[] nResultData = new int[128];

            int nMainCmd = 1000;
            int nSubCmd = 10012;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            ushort nCRC = 0;
            int nDataLength = 4;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData);

            byte[] aResult = BitConverter.GetBytes(nResultData[0]);
            szData = string.Format("{0}.{1}.{2}.{3}", (int)aResult[0], (int)aResult[1], (int)aResult[2], (int)aResult[3]);

            return bResult;
        }

        public bool WriteSocketServerSubnetMask(string szData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = WriteSocketServerSubnetMask_Port(g_nComPort, szData); break;
                case 1: bResult = WriteSocketServerSubnetMask_Sock(szData); break;
            }
            return bResult;
        }

        public bool WriteSocketServerSubnetMask(byte[] aData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = WriteSocketServerSubnetMask_Port(g_nComPort, szData); break;
                case 1: bResult = WriteSocketServerSubnetMask_Sock(aData); break;
            }
            return bResult;
        }

        public bool WriteSocketServerSubnetMask_Sock(string szData)
        {
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20012;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            string[] aData = szData.Split('.');
            if (4 != aData.Length)
                return false;

            for (int i = 0; i < 4; i++)
            {
                nDataBuf[4 + i] = byte.Parse(aData[i]);
            }

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool WriteSocketServerSubnetMask_Sock(byte[] aData)
        {
            if (4 != aData.Length)
                return false;

            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20012;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);
            
            for (int i = 0; i < 4; i++)
            {
                nDataBuf[4 + i] = aData[i];
            }

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool ReadSocketServerGateway(ref string szData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = ReadSocketServerGateway_Port(g_nComPort, szData); break;
                case 1: bResult = ReadSocketServerGateway_Sock(ref szData); break;
            }
            return bResult;
        }

        public bool ReadSocketServerGateway_Sock(ref string szData)
        {
            bool bResult = false;
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];
            int[] nResultData = new int[128];

            int nMainCmd = 1000;
            int nSubCmd = 10014;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            ushort nCRC = 0;
            int nDataLength = 4;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData);

            byte[] aResult = BitConverter.GetBytes(nResultData[0]);
            szData = string.Format("{0}.{1}.{2}.{3}", (int)aResult[0], (int)aResult[1], (int)aResult[2], (int)aResult[3]);

            return bResult;
        }

        public bool WriteSocketServerGateway(string szData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = WriteSocketServerGateway_Port(g_nComPort, szData); break;
                case 1: bResult = WriteSocketServerGateway_Sock(szData); break;
            }
            return bResult;
        }

        public bool WriteSocketServerGateway(byte[] aData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = WriteSocketServerGateway_Port(g_nComPort, szData); break;
                case 1: bResult = WriteSocketServerGateway_Sock(aData); break;
            }
            return bResult;
        }

        public bool WriteSocketServerGateway_Sock(string szData)
        {
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20014;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            string[] aData = szData.Split('.');
            if (4 != aData.Length)
                return false;

            for (int i = 0; i < 4; i++)
            {
                nDataBuf[4 + i] = byte.Parse(aData[i]);
            }

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool WriteSocketServerGateway_Sock(byte[] aData)
        {
            if (4 != aData.Length)
                return false;

            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20014;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            for (int i = 0; i < 4; i++)
            {
                nDataBuf[4 + i] = aData[i];
            }

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool ReadSocketServerDNS(ref string szData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = ReadSocketServerDNS_Port(g_nComPort, szData); break;
                case 1: bResult = ReadSocketServerDNS_Sock(ref szData); break;
            }
            return bResult;
        }

        public bool ReadSocketServerDNS_Sock(ref string szData)
        {
            bool bResult = false;
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];
            int[] nResultData = new int[128];

            int nMainCmd = 1000;
            int nSubCmd = 10016;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            ushort nCRC = 0;
            int nDataLength = 4;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData);

            byte[] aResult = BitConverter.GetBytes(nResultData[0]);
            szData = string.Format("{0}.{1}.{2}.{3}", (int)aResult[0], (int)aResult[1], (int)aResult[2], (int)aResult[3]);

            return bResult;
        }

        public bool WriteSocketServerDNS(string szData)
        {
            bool bResult = false;
            switch (g_nType)
	        {
	            case 0: bResult = false; break; // bResult = WriteSocketServerDNS_Port(g_nComPort, szData); break;
                case 1:	bResult = WriteSocketServerDNS_Sock(szData); break;
	        }
	        return bResult;
        }

        public bool WriteSocketServerDNS(byte[] aData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = WriteSocketServerDNS_Port(g_nComPort, szData); break;
                case 1: bResult = WriteSocketServerDNS_Sock(aData); break;
            }
            return bResult;
        }

        public bool WriteSocketServerDNS_Sock(string szData)
        {
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20016;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            string[] aData = szData.Split('.');
            if (4 != aData.Length)
                return false;

            for (int i = 0; i < 4; i++)
            {
                nDataBuf[4 + i] = byte.Parse(aData[i]);
            }

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool WriteSocketServerDNS_Sock(byte[] aData)
        {
            if (4 != aData.Length)
                return false;

            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20016;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            for (int i = 0; i < 4; i++)
            {
                nDataBuf[4 + i] = aData[i];
            }

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool ReadSocketServerPort(ref int pData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = ReadSocketServerPort_Port(g_nComPort, pData); break;
                case 1: bResult = ReadSocketServerPort_Sock(ref pData); break;
            }
            return bResult;
        }

        public bool ReadSocketServerPort_Sock(ref int pData)
        {
            bool bResult = false;
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];
            int[] nResultData = new int[128];

            int nMainCmd = 1000;
            int nSubCmd = 10008;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            ushort nCRC = 0;
            int nDataLength = 4;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData);

            if (bResult)
                pData = nResultData[0];

            return bResult;
        }

        public bool WriteSocketServerPort(int nData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = WriteSocketServerPort_Port(g_nComPort, nData); break;
                case 1: bResult = WriteSocketServerPort_Sock(nData); break;
            }
            return bResult;
        }

        public bool WriteSocketServerPort_Sock(int nData)
        {
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20008;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            int nDataLength = 8;
            ushort nCRC = 0;

            byte[] aData = BitConverter.GetBytes(nData);
            Array.Copy(aData, 0, nDataBuf, 4, 4);

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }
        
        public bool ReadSocketClientIP(ref string szData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = ReadSocketClientIP_Port(g_nComPort, szData); break;
                case 1: bResult = ReadSocketClientIP_Sock(ref szData); break;
            }
            return bResult;
        }

        public bool ReadSocketClientIP_Sock(ref string szData)
        {
            bool bResult = false;
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];
            int[] nResultData = new int[128];

            int nMainCmd = 1000;
            int nSubCmd = 10018;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            ushort nCRC = 0;
            int nDataLength = 4;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData);

            byte[] aResult = BitConverter.GetBytes(nResultData[0]);
            szData = string.Format("{0}.{1}.{2}.{3}", (int)aResult[0], (int)aResult[1], (int)aResult[2], (int)aResult[3]);

            return bResult;
        }

        public bool WriteSocketClientIP(string szData)
        {
            bool bResult = false;
	        switch (g_nType)
	        {
	            case 0: bResult = false; break; // bResult = WriteSocketClientIP_Port(g_nComPort, szData); break;
                case 1:	bResult = WriteSocketClientIP_Sock(szData); break;
	        }
	        return bResult;
        }

        public bool WriteSocketClientIP(byte[] aData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = WriteSocketClientIP_Port(g_nComPort, szData); break;
                case 1: bResult = WriteSocketClientIP_Sock(aData); break;
            }
            return bResult;
        }

        public bool WriteSocketClientIP_Sock(string szData)
        {
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20018;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            string[] aData = szData.Split('.');
            if (4 != aData.Length)
                return false;

            for (int i = 0; i < 4; i++)
            {
                nDataBuf[4 + i] = byte.Parse(aData[i]);
            }

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool WriteSocketClientIP_Sock(byte[] aData)
        {
            if (4 != aData.Length)
                return false;

            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20018;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            for (int i = 0; i < 4; i++)
            {
                nDataBuf[4 + i] = aData[i];
            }

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool ReadSocketClientSubnetMask(ref string szData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = ReadSocketClientSubnetMask_Port(g_nComPort, szData); break;
                case 1: bResult = ReadSocketClientSubnetMask_Sock(ref szData); break;
            }
            return bResult;
        }

        public bool ReadSocketClientSubnetMask_Sock(ref string szData)
        {
            bool bResult = false;
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];
            int[] nResultData = new int[128];

            int nMainCmd = 1000;
            int nSubCmd = 10020;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            ushort nCRC = 0;
            int nDataLength = 4;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData);

            byte[] aResult = BitConverter.GetBytes(nResultData[0]);
            szData = string.Format("{0}.{1}.{2}.{3}", (int)aResult[0], (int)aResult[1], (int)aResult[2], (int)aResult[3]);

            return bResult;
        }

        public bool WriteSocketClientSubnetMask(string szData)
        {
            bool bResult = false;
	        switch (g_nType)
	        {
	            case 0: bResult = false; break; // bResult = WriteSocketClientSubnetMask_Port(g_nComPort, szData); break;
                case 1:	bResult = WriteSocketClientSubnetMask_Sock(szData); break;
	        }
	        return bResult;
        }

        public bool WriteSocketClientSubnetMask(byte[] aData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = WriteSocketClientSubnetMask_Port(g_nComPort, szData); break;
                case 1: bResult = WriteSocketClientSubnetMask_Sock(aData); break;
            }
            return bResult;
        }

        public bool WriteSocketClientSubnetMask_Sock(string szData)
        {
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20020;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            string[] aData = szData.Split('.');
            if (4 != aData.Length)
                return false;

            for (int i = 0; i < 4; i++)
            {
                nDataBuf[4 + i] = byte.Parse(aData[i]);
            }

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool WriteSocketClientSubnetMask_Sock(byte[] aData)
        {
            if (4 != aData.Length)
                return false;

            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20020;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            for (int i = 0; i < 4; i++)
            {
                nDataBuf[4 + i] = aData[i];
            }

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool ReadSocketClientGateway(ref string szData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = ReadSocketClientGateway_Port(g_nComPort, szData); break;
                case 1: bResult = ReadSocketClientGateway_Sock(ref szData); break;
            }
            return bResult;
        }

        public bool ReadSocketClientGateway_Sock(ref string szData)
        {
            bool bResult = false;
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];
            int[] nResultData = new int[128];

            int nMainCmd = 1000;
            int nSubCmd = 10022;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            ushort nCRC = 0;
            int nDataLength = 4;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData);

            byte[] aResult = BitConverter.GetBytes(nResultData[0]);
            szData = string.Format("{0}.{1}.{2}.{3}", (int)aResult[0], (int)aResult[1], (int)aResult[2], (int)aResult[3]);

            return bResult;
        }

        public bool WriteSocketClientGateway(string szData)
        {
            bool bResult = false;
	        switch (g_nType)
	        {
	            case 0: bResult = false; break; // bResult = WriteSocketClientGateway_Port(g_nComPort, szData); break;
                case 1:	bResult = WriteSocketClientGateway_Sock(szData); break;
	        }
	        return bResult;
        }

        public bool WriteSocketClientGateway(byte[] aData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = WriteSocketClientGateway_Port(g_nComPort, szData); break;
                case 1: bResult = WriteSocketClientGateway_Sock(aData); break;
            }
            return bResult;
        }

        public bool WriteSocketClientGateway_Sock(string szData)
        {
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20022;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            string[] aData = szData.Split('.');
            if (4 != aData.Length)
                return false;

            for (int i = 0; i < 4; i++)
            {
                nDataBuf[4 + i] = byte.Parse(aData[i]);
            }

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool WriteSocketClientGateway_Sock(byte[] aData)
        {
            if (4 != aData.Length)
                return false;

            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20022;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            for (int i = 0; i < 4; i++)
            {
                nDataBuf[4 + i] = aData[i];
            }

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool ReadSocketClientDNS(ref string szData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = ReadSocketClientDNS_Port(g_nComPort, szData); break;
                case 1: bResult = ReadSocketClientDNS_Sock(ref szData); break;
            }
            return bResult;
        }

        public bool ReadSocketClientDNS_Sock(ref string szData)
        {
            bool bResult = false;
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];
            int[] nResultData = new int[128];

            int nMainCmd = 1000;
            int nSubCmd = 10024;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            ushort nCRC = 0;
            int nDataLength = 4;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData);

            byte[] aResult = BitConverter.GetBytes(nResultData[0]);
            szData = string.Format("{0}.{1}.{2}.{3}", (int)aResult[0], (int)aResult[1], (int)aResult[2], (int)aResult[3]);

            return bResult;
        }

        public bool WriteSocketClientDNS(string szData)
        {

            bool bResult = false;
	        switch (g_nType)
	        {
	            case 0: bResult = false; break; // bResult = WriteSocketClientDNS_Port(g_nComPort, szData); break;
                case 1:	bResult = WriteSocketClientDNS_Sock(szData); break;
	        }
	        return bResult;
        }

        public bool WriteSocketClientDNS(byte[] aData)
        {

            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = WriteSocketClientDNS_Port(g_nComPort, szData); break;
                case 1: bResult = WriteSocketClientDNS_Sock(aData); break;
            }
            return bResult;
        }

        public bool WriteSocketClientDNS_Sock(string szData)
        {
            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20024;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            string[] aData = szData.Split('.');
            if (4 != aData.Length)
                return false;

            for (int i = 0; i < 4; i++)
            {
                nDataBuf[4 + i] = byte.Parse(aData[i]);
            }

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool WriteSocketClientDNS_Sock(byte[] aData)
        {
            if (4 != aData.Length)
                return false;

            byte[] nDataBuf = new byte[128];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 1000;
            int nSubCmd = 20024;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            ushort nCRC = 0;
            int nDataLength = 8;

            nCRC = Communication_MakeCmd_CRC16(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, nDataBuf, 0, 2);

            for (int i = 0; i < 4; i++)
            {
                nDataBuf[4 + i] = aData[i];
            }

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            // Communiaion Data Send -> 전송만 하고 회신을 받지 않는다.
            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        #endregion
    }
}
