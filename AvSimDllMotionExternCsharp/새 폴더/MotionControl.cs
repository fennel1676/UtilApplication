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

        private int Communication_MakeCmd(int nMainCmd, int nSubCmd, byte chResponseType, int nDataLength, byte[] pDataBuf, byte[] pComDataFrame)
        {
            UInt16 nCRC = 0;
            int nIndex = 0;

            byte[] aMainCmd = BitConverter.GetBytes(nMainCmd);
            byte[] aSubCmd = BitConverter.GetBytes(nSubCmd);
            byte[] aResponseType = BitConverter.GetBytes(chResponseType);
            byte[] aDataLength = BitConverter.GetBytes(nDataLength);
            
            // Header
            pComDataFrame[nIndex] = (byte)0xFF;                             nIndex += 1;
            pComDataFrame[nIndex] = (byte)0x55;                             nIndex += 1;
            pComDataFrame[nIndex] = (byte)0xEE;                             nIndex += 1;
            pComDataFrame[nIndex] = (byte)0xAA;                             nIndex += 1;
            // Command            
            Array.Copy(aMainCmd, 0, pComDataFrame, nIndex, 4);              nIndex += 4;            // Main Cmd
            Array.Copy(aSubCmd, 0, pComDataFrame, nIndex, 4);               nIndex += 4;            // Sub Cmd 
            Array.Copy(aResponseType, 0, pComDataFrame, nIndex, 1);         nIndex += 1;            // Response Type
            Array.Copy(aDataLength, 0, pComDataFrame, nIndex, 4);           nIndex += 4;            // Data Length
            Array.Copy(pDataBuf, 0, pComDataFrame, nIndex, nDataLength);    nIndex += nDataLength;  // Data
            // CRC16
            nCRC = AvSimDllCommSubFunc.MakeCRC16(pComDataFrame, nIndex);
            byte[] aCRC = BitConverter.GetBytes(nCRC);
            Array.Copy(aCRC, 0, pComDataFrame, nIndex, 2);                  nIndex += 2;

            return nIndex;
        }

        private UInt16 Communication_MakeCmd_CRC16(int nMainCmd, int nSubCmd, byte chResponseType, int nDataLength, byte[] pDataBuf, byte[] pComDataFrame)
        {
            UInt16 nCRC = 0;
            int nIndex = 0;
            int nCmdCnt = 0;

            byte[] aMainCmd = BitConverter.GetBytes(nMainCmd);
            byte[] aSubCmd = BitConverter.GetBytes(nSubCmd);
            byte[] aResponseType = BitConverter.GetBytes(chResponseType);
            byte[] aDataLength = BitConverter.GetBytes(nDataLength);

            // Header
            pComDataFrame[nIndex] = (byte)0xFF;                         nIndex += 1;
            pComDataFrame[nIndex] = (byte)0x55;                         nIndex += 1;
            pComDataFrame[nIndex] = (byte)0xEE;                         nIndex += 1;
            pComDataFrame[nIndex] = (byte)0xAA;                         nIndex += 1;
            // Command
            //nIndex += 1; Array.Copy(aMainCmd, 0, pComDataFrame, nIndex, 4);         // Main Cmd
            //nIndex += 4; Array.Copy(aSubCmd, 0, pComDataFrame, nIndex, 4);          // Sub Cmd                                                                                    
            //nIndex += 4; Array.Copy(aResponseType, 0, pComDataFrame, nIndex, 1);    // Response Type                                                                                    
            //nIndex += 1; Array.Copy(aDataLength, 0, pComDataFrame, nIndex, 4);      // Data Length
            //nIndex += 4;                                                            // DataLength까지 CRC계산을 하기 위해서 처리 해준다....
            Array.Copy(aMainCmd, 0, pComDataFrame, nIndex, 4);           nIndex += 4;   // Main Cmd
            Array.Copy(aSubCmd, 0, pComDataFrame, nIndex, 4);            nIndex += 4;   // Sub Cmd 
            Array.Copy(aResponseType, 0, pComDataFrame, nIndex, 1);      nIndex += 1;   // Response Type
            Array.Copy(aDataLength, 0, pComDataFrame, nIndex, 4);        nIndex += 4;   // Data Length
                                                                                        // DataLength까지 CRC계산을 하기 위해서 처리 해준다....
            nCRC = AvSimDllCommSubFunc.MakeCRC16(pComDataFrame, nIndex);                // CRC16

            return nCRC;
        }

        private void Communication_Parsing(byte[] pCommDataBuf, int[] pnResult)
        {
            int nMainCmd = 0;
            int nSubCmd = 0;
            byte chResponseType = 0;
            int nDataLength = 0;
            int[] nDataBuf = new int[1024];

            int nIndex = 0;

            // Header
            nIndex += 0;
            
            // Command
            nIndex += 4; nMainCmd = BitConverter.ToInt32(pCommDataBuf, nIndex);     // Main Cmd
            nIndex += 4; nSubCmd = BitConverter.ToInt32(pCommDataBuf, nIndex);      // Sub Cmd
            nIndex += 4; chResponseType = pCommDataBuf[nIndex];                     // Response Type
            nIndex += 1; nDataLength = BitConverter.ToInt32(pCommDataBuf, nIndex);  // Data Length
            nIndex += 4;                                                            // Data

            if (nDataLength > 0)
                Buffer.BlockCopy(pCommDataBuf, nIndex, nDataBuf, 0, nDataLength);

            switch (nMainCmd)
            {
                case 0:
                    {
                        Console.WriteLine("ping");
                    }
                    break;
                case 1:
                    {
                        switch (nSubCmd)
                        {
                            case 10003: Buffer.BlockCopy(nDataBuf, 0, pnResult, 0, nDataLength); break;
                        }
                    }
                    break;
                case 9:
                    {
                        switch (nSubCmd)
                        {
                            case 5: Buffer.BlockCopy(nDataBuf, 0, pnResult, 0, 8); break;
                        }
                    }
                    break;
                case 3:
                    {
                        switch (nSubCmd)
                        {
                            case 5: Buffer.BlockCopy(nDataBuf, 0, pnResult, 0, 8); break;
                            case 13: pnResult[0] = nDataBuf[0]; break;
                            case 19: Buffer.BlockCopy(nDataBuf, 0, pnResult, 0, 9); break;
                            case 27:
                            case 29:
                            case 31:
                            case 33:
                            case 35:
                            case 37:
                            case 43:
                            case 45:
                            case 47:
                            case 49: Buffer.BlockCopy(nDataBuf, 0, pnResult, 0, nDataLength); break;
                            case 51: pnResult[0] = nDataBuf[0]; break;
                        }
                    }
                    break;
                case 103:
                    {
                        switch (nSubCmd)
                        {
                            case 5: Buffer.BlockCopy(nDataBuf, 0, pnResult, 0, 8); break;
                            case 13: pnResult[0] = nDataBuf[0]; break;
                            case 19: Buffer.BlockCopy(nDataBuf, 0, pnResult, 0, 9); break;
                            case 27:
                            case 29:
                            case 31:
                            case 33:
                            case 35:
                            case 37:
                            case 43:
                            case 45:
                            case 47:
                            case 49: Buffer.BlockCopy(nDataBuf, 0, pnResult, 0, nDataLength); break;
                            case 51: pnResult[0] = nDataBuf[0]; break;
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
                            //case 5:		memmove(pnResult, pCommDataBuf + 17, 8);break;   not used
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
                            //case 5:		memmove(pnResult, pCommDataBuf + 17, 8);break; not used
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
            byte[] szRxBuf = new byte[1024];

            int nRxCnt = 0, nTxCnt = 0, nIndex = 0, nOffset = 0;
            int nRespMainCmd = 0, nRespSubCmd = 0, nRespDataLength = 0;
            nTxCnt = m_avSimDllComm.ComWrite(pCommDataBuf, nDataLength);

            if (nTxCnt == 0 || nTxCnt != nDataLength)
                return false;

            if (chResponseType == 0)
                return true;

            string strMsg = string.Empty, strMsgBuf = string.Empty;
            eState nCommStateInput = eState.NONE;

            long timer = DateTime.Now.Ticks;

            while (nCommStateInput == eState.NONE)
            {
                if (AvSimDllCommSubFunc.TickCheck(g_nTimeout, timer))
                {
                    nCommStateInput = eState.TIMEOUT;
                    break;
                }

                if (m_avSimDllComm.ComState())
                {
                    if ((nRxCnt = m_avSimDllComm.ComRead(szRxBuf, 1024)) > 0)
                    {
                        byte[] aRecvCmd = new byte[4];

                        //if ((byte)szRxBuf[nIndex + 0] == (byte)0xFF && (byte)szRxBuf[nIndex + 1] == (byte)0x55 &&
                        //    (byte)szRxBuf[nIndex + 2] == (byte)0xEE && (byte)szRxBuf[nIndex + 3] == (byte)0xAA)
                        //    nIndex = 7;
                        //else
                        //    return false;

                        bool isResult = false;
                        while (22 + nOffset < nRxCnt)
                        {
                            //  nMainCmd
                            Array.Copy(szRxBuf, 7 + nOffset - 3, aRecvCmd, 0, 4);
                            nRespMainCmd = BitConverter.ToInt32(aRecvCmd, 0);

                            //  nSubCmd
                            Array.Copy(szRxBuf, 11 + nOffset - 3, aRecvCmd, 0, 4);
                            nRespSubCmd = BitConverter.ToInt32(aRecvCmd, 0);

                            //  nRespDataLength
                            Array.Copy(szRxBuf, 16 + nOffset - 3, aRecvCmd, 0, 4);
                            nRespDataLength = BitConverter.ToInt32(aRecvCmd, 0);

                            if ((byte)szRxBuf[nIndex + nOffset + 0] == (byte)0xFF && (byte)szRxBuf[nIndex + nOffset + 1] == (byte)0x55 &&
                                (byte)szRxBuf[nIndex + nOffset + 2] == (byte)0xEE && (byte)szRxBuf[nIndex + nOffset + 3] == (byte)0xAA &&
                                (nRespMainCmd == (nMainCmd + 1)) &&
                                (nRespSubCmd == (nSubCmd + 1)) &&
                                (nRespDataLength > 0))
                            {
                                nIndex = nRespDataLength + 19;
                                isResult = true;
                                break;
                            }
                            else
                                nOffset++;
                        }

                        if (!isResult)
                            return false;

                        ////  nMainCmd
                        //Array.Copy(szRxBuf, nIndex + nOffset - 3, aRecvCmd, 0, 4);
                        //nRespMainCmd = BitConverter.ToInt32(aRecvCmd, 0);
                        //if (nRespMainCmd == (nMainCmd + 1))
                        //    nIndex = 11;
                        //else
                        //    return false;

                        ////  nSubCmd
                        //Array.Copy(szRxBuf, nIndex + nOffset - 3, aRecvCmd, 0, 4);
                        //nRespSubCmd = BitConverter.ToInt32(aRecvCmd, 0);
                        //if (nRespSubCmd == (nSubCmd + 1))
                        //    nIndex = 16;
                        //else
                        //    return false;

                        ////  nRespDataLength
                        //Array.Copy(szRxBuf, nIndex + nOffset - 3, aRecvCmd, 0, 4);
                        //nRespDataLength = BitConverter.ToInt32(aRecvCmd, 0);
                        //if (nRespDataLength > 0)
                        //    nIndex = nRespDataLength + 19;
                        //else
                        //    return false;

                        //  CRC
                        UInt16 nCRC1 = 0;
                        UInt16 nCRC2 = 0;

                        Array.Copy(szRxBuf, nIndex + nOffset - 2, aRecvCmd, 0, 2);
                        nCRC1 = BitConverter.ToUInt16(aRecvCmd, 0);
                        nCRC2 = AvSimDllCommSubFunc.MakeCRC16(szRxBuf, nOffset, nIndex + nOffset - 2);

                        if (nCRC1 == nCRC2)
                        {
                            Buffer.BlockCopy(szRxBuf, 17, pnResult, 0, nRespDataLength);
                            nCommStateInput = eState.SUCCESS;
                            return true;
                        }
                        else
                        {
                            nCommStateInput = eState.ERROR;
                            return false;
                        }

                        //int nCount = 0;
                        //while (nRxCnt >= nCount)
                        //{
                        //    switch (nIndex)
                        //    {
                        //        case 0:
                        //            if ((byte)szRxBuf[nIndex + 0] == (byte)0xFF && (byte)szRxBuf[nIndex + 1] == (byte)0x55 &&
                        //                (byte)szRxBuf[nIndex + 2] == (byte)0xEE && (byte)szRxBuf[nIndex + 3] == (byte)0xAA)
                        //                nIndex = 4;
                        //            else
                        //                return false;
                        //            break;
                        //        case 4: case 5: case 6: nIndex = 7; break;
                        //        case 7:
                        //            {
                        //                Array.Copy(szRxBuf, nIndex - 3, aRecvCmd, 0, 4);
                        //                nRespMainCmd = BitConverter.ToInt32(aRecvCmd, 0);
                        //                if (nRespMainCmd == (nMainCmd + 1))
                        //                    nIndex = nIndex + 1;
                        //                else
                        //                    return false;
                        //            }
                        //            break;
                        //        case 8: case 9: case 10: nIndex = 11; break;
                        //        case 11:
                        //            {
                        //                Array.Copy(szRxBuf, nIndex - 3, aRecvCmd, 0, 4);
                        //                nRespSubCmd = BitConverter.ToInt32(aRecvCmd, 0);
                        //                if (nRespSubCmd == (nSubCmd + 1))
                        //                    nIndex = nIndex + 1;
                        //                else
                        //                    return false;
                        //            }
                        //            break;
                        //        case 12: case 13: case 14: case 15: nIndex = 16; break;
                        //        case 16:
                        //            {
                        //                Array.Copy(szRxBuf, nIndex - 3, aRecvCmd, 0, 4);
                        //                nRespDataLength = BitConverter.ToInt32(aRecvCmd, 0);
                        //                if (nRespDataLength > 0)
                        //                    nIndex = nIndex + 1;
                        //                else
                        //                    return false;
                        //            }
                        //            break;
                        //        default:
                        //            {
                        //                nIndex = nRespDataLength + 19;
                        //                UInt16 nCRC1 = 0;
                        //                UInt16 nCRC2 = 0;

                        //                Array.Copy(szRxBuf, nIndex - 2, aRecvCmd, 0, 2);
                        //                nCRC1 = BitConverter.ToUInt16(aRecvCmd, 0);
                        //                nCRC2 = AvSimDllCommSubFunc.MakeCRC16(szRxBuf, nIndex - 2);

                        //                if (nCRC1 == nCRC2)
                        //                {
                        //                    Buffer.BlockCopy(szRxBuf, 17, pnResult, 0, nRespDataLength);
                        //                    nCommStateInput = eState.SUCCESS;
                        //                    return true;
                        //                }
                        //                else
                        //                {
                        //                    nCommStateInput = eState.ERROR;
                        //                    return false;
                        //                }
                        //            }
                        //    }
                        //}
                    }
                    else
                    {
                        if (nRxCnt == -1)
                        {
                            nCommStateInput = eState.ERROR;
                            return false;
                        }
                    }
                }
            }

            return nCommStateInput != eState.SUCCESS ? false : true;
        }

        private bool Communication_CmdSend_Sock(int nMainCmd, int nSubCmd, byte chResponseType, int nDataLength, byte[] pCommDataBuf, ref int[] pnResult, int nRecvPacketLength)
        {
            byte[] szRxBuf = new byte[1024];

            int nRxCnt = 0, nTxCnt = 0, nIndex = 0, nOffset = 0;
            int nRespMainCmd = 0, nRespSubCmd = 0, nRespDataLength = 0;
            nTxCnt = m_avSimDllComm.ComWrite(pCommDataBuf, nDataLength);
            if (nTxCnt == 0 || nTxCnt != nDataLength)
                return false;

            if (chResponseType == 0)
                return true;

            string strMsg = string.Empty, strMsgBuf = string.Empty;
            eState nCommStateInput = eState.NONE;

            long timer = DateTime.Now.Ticks;
            while (nCommStateInput == eState.NONE)
            {
                if (AvSimDllCommSubFunc.TickCheck(g_nTimeout, timer))
                {
                    nCommStateInput = eState.TIMEOUT;
                    return false;
                }

                if (m_avSimDllComm.ComState())
                {
                    if ((nRxCnt = m_avSimDllComm.ComRead(szRxBuf, 1024, nRecvPacketLength)) > 0)
                    {
                        byte[] aRecvCmd = new byte[4];

                        //if ((byte)szRxBuf[nIndex + 0] == (byte)0xFF && (byte)szRxBuf[nIndex + 1] == (byte)0x55 &&
                        //    (byte)szRxBuf[nIndex + 2] == (byte)0xEE && (byte)szRxBuf[nIndex + 3] == (byte)0xAA)
                        //    nIndex = 7;
                        //else
                        //    return false;

                        bool isResult = false;
                        while (22 + nOffset < nRxCnt)
                        {
                            //  nMainCmd
                            Array.Copy(szRxBuf, 7 + nOffset - 3, aRecvCmd, 0, 4);
                            nRespMainCmd = BitConverter.ToInt32(aRecvCmd, 0);

                            //  nSubCmd
                            Array.Copy(szRxBuf, 11 + nOffset - 3, aRecvCmd, 0, 4);
                            nRespSubCmd = BitConverter.ToInt32(aRecvCmd, 0);

                            //  nRespDataLength
                            Array.Copy(szRxBuf, 16 + nOffset - 3, aRecvCmd, 0, 4);
                            nRespDataLength = BitConverter.ToInt32(aRecvCmd, 0);

                            if ((byte)szRxBuf[nIndex + nOffset + 0] == (byte)0xFF && (byte)szRxBuf[nIndex + nOffset + 1] == (byte)0x55 &&
                                (byte)szRxBuf[nIndex + nOffset + 2] == (byte)0xEE && (byte)szRxBuf[nIndex + nOffset + 3] == (byte)0xAA &&
                                (nRespMainCmd == (nMainCmd + 1)) && (nRespSubCmd == (nSubCmd + 1)) && (nRespDataLength > 0))
                            {
                                nIndex = nRespDataLength + 19;
                                isResult = true;
                                break;
                            }
                            else
                                nOffset++;
                        }

                        if (!isResult)
                            return false;

                        ////  nMainCmd
                        //Array.Copy(szRxBuf, nIndex + nOffset - 3, aRecvCmd, 0, 4);
                        //nRespMainCmd = BitConverter.ToInt32(aRecvCmd, 0);
                        //if (nRespMainCmd == (nMainCmd + 1))
                        //    nIndex = 11;
                        //else
                        //    return false;

                        ////  nSubCmd
                        //Array.Copy(szRxBuf, nIndex + nOffset - 3, aRecvCmd, 0, 4);
                        //nRespSubCmd = BitConverter.ToInt32(aRecvCmd, 0);
                        //if (nRespSubCmd == (nSubCmd + 1))
                        //    nIndex = 16;
                        //else
                        //    return false;

                        ////  nRespDataLength
                        //Array.Copy(szRxBuf, nIndex + nOffset - 3, aRecvCmd, 0, 4);
                        //nRespDataLength = BitConverter.ToInt32(aRecvCmd, 0);
                        //if (nRespDataLength > 0)
                        //    nIndex = nRespDataLength + 19;
                        //else
                        //    return false;

                        //  CRC
                        UInt16 nCRC1 = 0;
                        UInt16 nCRC2 = 0;

                        Array.Copy(szRxBuf, nIndex + nOffset - 2, aRecvCmd, 0, 2);
                        nCRC1 = BitConverter.ToUInt16(aRecvCmd, 0);
                        nCRC2 = AvSimDllCommSubFunc.MakeCRC16(szRxBuf, nOffset, nIndex + nOffset - 2);
                        
                        if (nCRC1 == nCRC2)
                        {
                            Buffer.BlockCopy(szRxBuf, 17, pnResult, 0, nRespDataLength);
                            nCommStateInput = eState.SUCCESS;
                            return true;
                        }
                        else
                        {
                            nCommStateInput = eState.ERROR;
                            return false;
                        }
                    }
                    else
                    {
                        if (nRxCnt == -1)
                        {
                            nCommStateInput = eState.ERROR;
                            return false;
                        }
                    }
                }
            }

            return nCommStateInput != eState.SUCCESS ? false : true;
        }

        public bool Initial()
        {
            bool bResult = false;
            EnvironmentParameterXml xml = null;
            Util.Load<EnvironmentParameterXml>(out xml, ENVIRONMENT_INI_FILE_NAME);

            if (null == xml)
                return false;

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
                case 0: bResult = false; break; //bResult = ComOpen(g_nComPort); break;
                case 1: bResult = m_avSimDllComm.ComOpen(g_strRemoteIP, g_nRemotePort); break;
            }

            return bResult;
        }

        public bool Initial(string strIP, int nPort, eOS os, int nTimeout = 500)
        {
            bool bResult = false;

            g_nType = 1;
            g_nTimeout = nTimeout;
            g_strRemoteIP = strIP;
            g_nRemotePort = nPort;

            m_avSimDllComm = Factory.CreateAvSimDllCommSocket(os);
            if (null == m_avSimDllComm)
                return false;

            switch (g_nType)
            {
                case 0: bResult = false; break; //bResult = ComOpen(g_nComPort); break;
                case 1: bResult = m_avSimDllComm.ComOpen(g_strRemoteIP, g_nRemotePort); break;
            }

            return bResult;
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
            bool bResult = false;

            switch (g_nType)
            {
                case 0: bResult = false; break; //bResult = ComClose(g_nComPort); break;
                case 1: bResult = m_avSimDllComm.ComClose(); break;
            }

            if (bResult)
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

            return bResult;
        }

        public bool Destroy_Sock()
        {
            return m_avSimDllComm.ComClose();
        }

        public bool State()
        {
            bool bResult = false;

            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = ComState(g_nComPort); break;
                case 1: bResult = m_avSimDllComm.ComState(); break;
            }

            return bResult;
        }

        public bool State_Sock()
        {
            return m_avSimDllComm.ComState();
        }

        public bool Ping()
        {
            bool bResult = false;

            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = MotionControl__Ping_Port(g_nComPort); break;
                case 1: bResult = Ping_Sock(); break;
            }
            return bResult;
        }

        public bool Ping_Sock(bool bResp = true)
        {
            int[] nResult = new int[256];
            byte[] nDataBuf = new byte[1024];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 0;
            int nSubCmd = 0;
            byte chResponseType = (byte)(bResp ? eDefine.AV_SIM__RESPONSE_ENABLE : eDefine.AV_SIM__RESPONSE_DISABLE);
            int nDataLength = 0;
            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bool isResult = false;
            if (m_isMode)
                isResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResult, nSendDataLength);
            else
                isResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResult);

            return isResult;
        }

        public bool DO(int nDO)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = MotionControl__DO_Port(g_nComPort, nDO); break;
                case 1: bResult = DO_Sock(nDO); break;
            }
            return bResult;
        }

        public bool DO_Sock(int nDO)
        {
            byte[] nDataBuf = new byte[1024];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 2;
            int nSubCmd = 10;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            int nDataLength = sizeof(int);

            byte[] aDO = BitConverter.GetBytes(nDO);
            Array.Copy(aDO, 0, nDataBuf, 0, 4);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public int GetDI()
        {
            int nDI = -1;
            switch (g_nType)
            {
                case 0: nDI = -1; break; // nDI = MotionControl__GetDI_Port(g_nComPort); break;
                case 1: nDI = GetDI_Sock(); break;
            }

            return nDI;
        }

        public int GetDI_Sock()
        {
            int[] nResult = new int[256];
            byte[] nDataBuf = new byte[1024];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 2;
            int nSubCmd = 12;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            int nDataLength = 0;
            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bool isResult = false;
            if (m_isMode)
                isResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResult, nSendDataLength + 4);
            else
                isResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResult);

            if (!isResult)
                return -1;
            else
                return nResult[0];
        }

        public int GetDO()
        {
            int nDO = -1;
            switch (g_nType)
            {
                case 0: nDO = -1; break; // nDO = MotionControl__GetDO_Port(g_nComPort); break;
                case 1: nDO = GetDO_Sock(); break;
            }

            return nDO;


        }

        public int GetDO_Sock()
        {
            int[] nResult = new int[256];
            byte[] nDataBuf = new byte[1024];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 2;
            int nSubCmd = 50;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_ENABLE;
            int nDataLength = 0;
            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bool isResult = false;
            if (m_isMode)
                isResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResult, nSendDataLength + 4);
            else
                isResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResult);

            if (!isResult)
                return -1;
            else
                return nResult[0];
        }

        public bool EQExtendData(out EQUIPMENT_EXTEND_DATA pEQData, bool bResp = true)
        {
            bool bResult = false;
            pEQData = null;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = MotionControl__EQExtendData_Port(g_nComPort, pEQData, bResp); break;
                case 1: bResult = EQExtendData_Sock(out pEQData, bResp); break;
            }
            return bResult;
        }

        public bool EQExtendData_Sock(out EQUIPMENT_EXTEND_DATA pEQExtendData, bool bResp = true)
        {
            pEQExtendData = null;


            int[] nResultData = new int[256];
            byte[] nDataBuf = new byte[1024];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 2;
            int nSubCmd = 48;
            byte chResponseType = (byte)(bResp ? eDefine.AV_SIM__RESPONSE_ENABLE : eDefine.AV_SIM__RESPONSE_DISABLE);
            int nDataLength = Marshal.SizeOf(typeof(EQUIPMENT_EXTEND_DATA));

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bool isResult = false;
            if (m_isMode)
                isResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData, nSendDataLength);
            else
                isResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData);

            if (isResult)
                pEQExtendData = Util.Array2Struct<int, EQUIPMENT_EXTEND_DATA>(nResultData);

            return isResult;
        }

        public bool DOF_and_Blower(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = MotionControl__DOF_and_Blower_Port(g_nComPort, nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nBlower); break;
                case 1: bResult = DOF_and_Blower_Sock(nRoll, nPitch, nYaw, nSway, nSurge, nHeave, nSpeed, nBlower); break;
            }
            return bResult;
        }

        public bool DOF_and_Blower_Sock(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower)
        {
            if (nSpeed < 1)
                return false;

            byte[] nDataBuf = new byte[1024];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 2;
            int nSubCmd = 4;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            int nDataLength = Marshal.SizeOf(typeof(MOTION_DATA));
            int nIndex = 0;

            byte[] aRoll = BitConverter.GetBytes(nRoll);
            byte[] aPitch = BitConverter.GetBytes(nPitch);
            byte[] aYaw = BitConverter.GetBytes(nYaw);
            byte[] aSway = BitConverter.GetBytes(nSway);
            byte[] aSurge = BitConverter.GetBytes(nSurge);
            byte[] aHeave = BitConverter.GetBytes(nHeave);
            byte[] aSpeed = BitConverter.GetBytes(nSpeed);
            byte[] aBlower = BitConverter.GetBytes(nBlower);

            Array.Copy(aRoll, 0, nDataBuf, nIndex, 4);      nIndex += 4;
            Array.Copy(aPitch, 0, nDataBuf, nIndex, 4);     nIndex += 4;
            Array.Copy(aYaw, 0, nDataBuf, nIndex, 4);       nIndex += 4;
            Array.Copy(aSway, 0, nDataBuf, nIndex, 4);      nIndex += 4;
            Array.Copy(aSurge, 0, nDataBuf, nIndex, 4);     nIndex += 4;
            Array.Copy(aHeave, 0, nDataBuf, nIndex, 4);     nIndex += 4;
            Array.Copy(aSpeed, 0, nDataBuf, nIndex, 4);     nIndex += 4;
            Array.Copy(aBlower, 0, nDataBuf, nIndex, 4);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool V2_DOF_and_Blower(int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower)
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

            return V2_DOF_and_Blower(data);
        }

        public bool V2_DOF_and_Blower(MOTION_DATA stMotionData)
        {
            bool bResult = false;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = MotionControlV2__DOF_and_Blower_Port(g_nComPort, stMotionData); break;
                case 1: bResult = V2_DOF_and_Blower_Sock(stMotionData); break;
            }
            return bResult;
        }

        public bool V2_DOF_and_Blower_Sock(MOTION_DATA stMotionData)
        {
            byte[] nDataBuf = new byte[1024];
            byte[] szComDataFrame = new byte[1024];

            int nMainCmd = 102;
            int nSubCmd = 4;
            byte chResponseType = (byte)eDefine.AV_SIM__RESPONSE_DISABLE;
            int nDataLength = Marshal.SizeOf(typeof(MOTION_DATA));

            byte[] aMotionData = Util.Struct2Byte(stMotionData);
            Array.Copy(aMotionData, 0, nDataBuf, 0, nDataLength);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            int nTxCnt = m_avSimDllComm.ComWrite(szComDataFrame, nSendDataLength);

            return (nTxCnt != nSendDataLength) ? false : true;
        }

        public bool V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis(
            int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower,
            int nRolling, int nRollingSpeed, int nRollingMode,
            int nPitching, int nPitchingSpeed, int nPitchingMode,
            int nYawing, int nYawingSpeed, int nYawingMode,
            out EQUIPMENT_DATA pEquipmentData, bool bResp = true)
        {
            pEquipmentData = null;

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

            return V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis(data, out pEquipmentData, bResp);
        }

        public bool V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis(MOTION_EXTEND_DATA stMotionData, out EQUIPMENT_DATA pEquipmentData, bool bResp = true)
        {
            bool bResult = false;
            pEquipmentData = null;
            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_Port(g_nComPort, stMotionData, pEQData, bResp); break;
                case 1: bResult = V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_Sock(stMotionData, out pEquipmentData, bResp); break;
            }
            return bResult;
        }

        public bool V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_Sock(MOTION_EXTEND_DATA stMotionData, out EQUIPMENT_DATA pEquipmentData, bool bResp = true)
        {
            pEquipmentData = null;

            int[] nResultData = new int[256];
            byte[] nDataBuf = new byte[1024];
            byte[] szComDataFrame= new byte[1024];
            int nMainCmd = 102;
            int nSubCmd = 42;
            byte chResponseType = (byte)(bResp ? eDefine.AV_SIM__RESPONSE_ENABLE : eDefine.AV_SIM__RESPONSE_DISABLE);
            int nDataLength = Marshal.SizeOf(typeof(MOTION_EXTEND_DATA)) + Marshal.SizeOf(typeof(EQUIPMENT_DATA)); 

            byte[] aMotionData = Util.Struct2Byte(stMotionData);
            int nMotionExtendData = Marshal.SizeOf(typeof(MOTION_EXTEND_DATA));
            Array.Copy(aMotionData, 0, nDataBuf, 0, nMotionExtendData);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bool isResult = false;
            if (m_isMode)
                isResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData, nSendDataLength);
            else
                isResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData);

            if (isResult)
            {
                Array.Copy(nResultData, nMotionExtendData / 4, nResultData, 0, Marshal.SizeOf(typeof(EQUIPMENT_DATA)));
                pEquipmentData = Util.Array2Struct<int, EQUIPMENT_DATA>(nResultData);
            }

            return isResult;
        }

        public bool V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm(
            int nRoll, int nPitch, int nYaw, int nSway, int nSurge, int nHeave, int nSpeed, int nBlower,
            int nRolling, int nRollingSpeed, int nRollingMode,
            int nPitching, int nPitchingSpeed, int nPitchingMode,
            int nYawing, int nYawingSpeed, int nYawingMode, 
            out EQUIPMENT_EXTEND_DATA pEquipmentExtendData, bool bResp = true)
        {
            pEquipmentExtendData = null;

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

            return V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm(data, out pEquipmentExtendData, bResp);
        }

        public bool V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm(MOTION_EXTEND_DATA stMotionData, out EQUIPMENT_EXTEND_DATA pEquipmentExtendData, bool bResp = true)
        {
            bool bResult = false;
            pEquipmentExtendData = null;

            switch (g_nType)
            {
                case 0: bResult = false; break; // bResult = MotionControlV2__DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm_Port(g_nComPort, stMotionData, pEQData, bResp); break;
                case 1: bResult = V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm_Sock(stMotionData, out pEquipmentExtendData, bResp); break;
            }
            return bResult;
        }

        public bool V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm_Sock(MOTION_EXTEND_DATA stMotionData, out EQUIPMENT_EXTEND_DATA pEquipmentExtendData, bool bResp = true)
        {
            pEquipmentExtendData = null;

            int[] nResultData = new int[256];
            byte[] nDataBuf = new byte[1024];
            byte[] szComDataFrame = new byte[1024];
            int nMainCmd = 102;
            int nSubCmd = 46;
            byte chResponseType = (byte)(bResp ? eDefine.AV_SIM__RESPONSE_ENABLE : eDefine.AV_SIM__RESPONSE_DISABLE);
            int nMotionExtendData = Marshal.SizeOf(typeof(MOTION_EXTEND_DATA));
            int nEquipmentExtendData = Marshal.SizeOf(typeof(EQUIPMENT_EXTEND_DATA));
            int nDataLength = nMotionExtendData + nEquipmentExtendData;

            byte[] aMotionData = Util.Struct2Byte(stMotionData);
            Array.Copy(aMotionData, 0, nDataBuf, 0, nMotionExtendData);

            int nSendDataLength = Communication_MakeCmd(nMainCmd, nSubCmd, chResponseType, nDataLength, nDataBuf, szComDataFrame);

            bool isResult = false;
            if (m_isMode)
                isResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData, nSendDataLength);
            else
                isResult = Communication_CmdSend_Sock(nMainCmd, nSubCmd, chResponseType, nSendDataLength, szComDataFrame, ref nResultData);

            if (isResult)
            {
                Array.Copy(nResultData, nMotionExtendData / 4, nResultData, 0, nEquipmentExtendData);
                pEquipmentExtendData = Util.Array2Struct<int, EQUIPMENT_EXTEND_DATA>(nResultData);
            }

            return isResult;
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
