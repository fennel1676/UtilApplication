using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace AKI.Communication.Util
{
    public class MagicPacket
    {
        private static Socket m_socketServer;
        private static int m_nPort = 40000;
        private static IPEndPoint m_iepTarget;

        public static int Port
        {
            get
            {
                return m_nPort;
            }

            set
            {
                m_nPort = value;
                m_iepTarget = new IPEndPoint(IPAddress.Broadcast, m_nPort);
            }
        }

        public static void Create()
        {
            m_socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            m_socketServer.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            m_iepTarget = new IPEndPoint(IPAddress.Broadcast, m_nPort);
        }

        public static void Close()
        {
            if (null != m_socketServer)
                m_socketServer.Close();
        }

        public static bool SendMagicPacket(string strIpAddress)
        {
            if (null == m_socketServer)
                return false;

            IPAddress ipAddress = AKI.Util.ConvertUtil.String2IPAddress(strIpAddress);
            if (null != ipAddress)
            {
                PhysicalAddress pa = NetworkInfoUtil.GetTargetDevicesOnLAN(ipAddress);
                if (null == pa)
                    return false;

                return SendMagicPacket(pa.GetAddressBytes());
            }
            else
            {
                byte[] abMapAddress = AKI.Util.ConvertUtil.HexString2ByteArray(strIpAddress);
                if (null == abMapAddress || 6 != abMapAddress.Length)
                    return false;

                return SendMagicPacket(abMapAddress);
            }
        }

        public static bool SendMagicPacket(byte[] abMapAddress)
        {
            if (null == m_socketServer)
                return false;

            // 매직패킷 생성 방법 arMagicPacket[102]

            // -----Hexa FF를 저장 --------| Mac Addr을 정수형으로 | 이후 90바이트는 7~12 내용이 반복되어 저장된다.
            // +---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+
            // |255|255|255|255|255|255|255|120|000|123|111|101|150|120|000|123|111|101|150|120|000|123|111|101|150|...
            // +---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+
            // 	0		1		2		3		4		5		6		7		8		9		10	11	12	13	14	15	16	17	18	19	20	21	22	23 .......

            byte[] arMagicPacket = new byte[102];

            // 매직 패킷 생성 및 전송 부분
            for (int i = 0; i < 6; i++)
            {
                arMagicPacket[i] = 0xff; // Hexa 로 저장
            }

            for (int i = 0; i < 6; i++)
            {
                arMagicPacket[i + 6] = abMapAddress[i];
            }

            for (int i = 0; i < 15; i++) // 90 바이트를 채운다
            {
                Array.Copy(abMapAddress, 0, arMagicPacket, (i + 2) * 6, 6);
            }

            m_socketServer.SendTo(arMagicPacket, arMagicPacket.Length, SocketFlags.None, m_iepTarget);
            return true;
        }
    }
}
