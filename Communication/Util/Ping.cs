using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;

using AKI.Communication.ICMP;
using AKI.Util;

namespace AKI.Communication.Util
{
    public class Ping
    {
        private class SendingParam
        {
            public int MessageSize;
            public int SendCount;
            public int ID;
            public string Host;
            public string IP;
            public EventPingStart Event_PingStart;
            public EventPingRun Event_PingRun;
            public EventPingMessage Event_PingMessage;
            public EventPingStop Event_PingStop;
        }

        public delegate void EventPingStart(int nId, string strHost, string strMessage);
        public delegate void EventPingRun(int nId, EndPoint ep, int nIndex, double dElapsedtime, int nMessageSize, string strMessage);
        public delegate void EventPingMessage(int nId, string strMessage);
        public delegate void EventPingStop(int nId);

        public event EventPingStart Event_PingStart;
        public event EventPingRun Event_PingRun;
        public event EventPingMessage Event_PingMessage;
        public event EventPingStop Event_PingStop;


        private int m_nMessageSize = 4;
        private Thread m_thread;
        private string m_strHost = "www.cisco.com";
        private int m_nId = 0;
        private int m_nSendCount = 1;
        private string m_strIpAddress = string.Empty;

        private static Socket g_socket;
        private static byte[] g_abData;

        public string Host { get { return m_strHost; } set { m_strHost = value; } }
        public string IpAddress { get { return m_strIpAddress; } set { m_strIpAddress = value; } }
        public int MessageSize { get { return m_nMessageSize; } set { m_nMessageSize = value; } }
        public int SendCount { get { return m_nSendCount; } set { m_nSendCount = value; } }

        public Ping() { }

        public void Initialize()
        {
            if (null == g_socket)
            {
                g_socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
                g_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);
            }

            if (null == Ping.g_abData)
                g_abData = Encoding.ASCII.GetBytes(Ping.g_strData);
        }

        public static void CallEvent(EventPingStart cb, int nId, string strHost, string strMessage)
        {
            if (null == cb)
                return;

            EventPingStart delegateEventState = new EventPingStart(cb);
            IAsyncResult ar = delegateEventState.BeginInvoke(nId, strHost, strMessage, null, null);
            delegateEventState.EndInvoke(ar);
        }

        public static void CallEvent(EventPingRun cb, int nId, EndPoint ep, int nIndex, double dElapsedtime, int nMessageSize, string strMessage)
        {
            if (null == cb)
                return;

            EventPingRun delegateEventState = new EventPingRun(cb);
            IAsyncResult ar = delegateEventState.BeginInvoke(nId, ep, nIndex, dElapsedtime, nMessageSize, strMessage, null, null);
            delegateEventState.EndInvoke(ar);
        }

        public static void CallEvent(EventPingMessage cb, int nId, string strMessage)
        {
            if (null == cb)
                return;

            EventPingMessage delegateEventState = new EventPingMessage(cb);
            IAsyncResult ar = delegateEventState.BeginInvoke(nId, strMessage, null, null);
            delegateEventState.EndInvoke(ar);
        }


        public void Send()
        {
            SendingParam param = new SendingParam();
            param.ID = m_nId++;
            param.MessageSize = m_nMessageSize;
            param.SendCount = m_nSendCount;
            param.Host = m_strHost;
            param.IP = m_strIpAddress;
            param.Event_PingStart = Event_PingStart;
            param.Event_PingRun = Event_PingRun;
            param.Event_PingMessage = Event_PingMessage;
            param.Event_PingStop = Event_PingStop;

            m_thread = new Thread(Ping.SendPing);
            m_thread.IsBackground = true;
            m_thread.Start(param);
        }

        private static void SendPing(object objData)
        {
            if (null == g_socket)
                return;

            if (null == Ping.g_abData)
                return;

            double dPingstart, dPingstop, dElapsedtime;
            SendingParam param = (SendingParam)objData;

            IPEndPoint iep = null;
            if (string.Empty == param.IP)
            {
                IPHostEntry iphe = Dns.Resolve(param.Host);
                iep = new IPEndPoint(iphe.AddressList[0], 0);
            }
            else
            {
                IPAddress ipAddress = AKI.Util.ConvertUtil.String2IPAddress(param.IP);
                if (null == ipAddress)
                    return;
                iep = new IPEndPoint(ipAddress, 0);
            }
            EndPoint ep = (EndPoint)iep;
            IcmpBase packet = new IcmpBase();
            int recv, i = 1;

            packet.Type = 0x08;
            packet.Code = 0x00;
            Buffer.BlockCopy(BitConverter.GetBytes(1), 0, packet.Message, 0, 2);
            byte[] data = Encoding.ASCII.GetBytes(g_strData);
            Buffer.BlockCopy(g_abData, 0, packet.Message, 4, param.MessageSize);
            packet.MessageSize = param.MessageSize + 4;
            int packetsize = packet.MessageSize + 4;

            string strMessage = string.Format("{0} Pinging : {1}", param.ID, string.Empty == param.IP ? param.Host : param.IP);
            Ping.CallEvent(param.Event_PingStart, param.ID, string.Empty == param.IP ? param.Host : param.IP, strMessage);

            HiPerfTimerUtil timer = new HiPerfTimerUtil();
            timer.Start();
            while (true)
            {
                packet.Checksum = 0;
                Buffer.BlockCopy(BitConverter.GetBytes(i), 0, packet.Message, 2, 2);
                UInt16 chcksum = packet.getChecksum();
                packet.Checksum = chcksum;

                timer.Stop();
                dPingstart = timer.DurationMilisecond;
                g_socket.SendTo(packet.getBytes(), packetsize, SocketFlags.None, iep);
                try
                {
                    data = new byte[1024];
                    recv = g_socket.ReceiveFrom(data, ref ep);

                    timer.Stop();
                    dPingstop = timer.DurationMilisecond;
                    dElapsedtime = dPingstop - dPingstart;
                    strMessage = string.Format("{0} reply from: {1}, seq: {2}, size: {3}, time = {4:F2}ms", param.ID, ep.ToString(), i, param.MessageSize, dElapsedtime);
                    Ping.CallEvent(param.Event_PingRun, param.ID, ep, i, dElapsedtime, param.MessageSize, strMessage);
                }
                catch (SocketException)
                {
                    strMessage = string.Format("{0} no reply from host", param.ID);
                    Ping.CallEvent(param.Event_PingMessage, param.ID, strMessage);
                }
                i++;

                if (i > param.SendCount)
                {
                    strMessage = string.Format("{0} Ping Complete", param.ID);
                    Ping.CallEvent(param.Event_PingMessage, param.ID, strMessage);
                    break;
                }

                Thread.Sleep(3000);
            }
        }

        private static string g_strData =
@"[ch]프롤로그[/ch]
- 젠장!-
한 여인이 사정없이 얼굴을 구기고 있었다.아니, 그는 여인이 아니었다.단지 여인처럼 보이는(?) 남자였다.남자는 무언가에 쫓기고 있는 듯 서둘러 한 곳을 향해 검을 휘둘렀다.그의 흑발과 흑안과 잘 어울리는 검은 강기가 그곳으로 날아갔다.
쾅! 하지만 강기의 힘은 많이 약해져 있었다.그것을 보고 또 욕지거리를 내뱉는 사내였다.
-씨발!-
욕이 좀 심했다.
그의 이름은 하유현.무림 최강의 집단인 마교의 어엿한 소교주였다. 어릴 적 힘든 삶 때문에 다소 거친 말투를 가지고 있었지만 그래도 어릴 적 마교의 교주의 눈에 띄어 이렇듯 마교주의 소교주가 되었다.무공의 재능은 남달랐기에 마교의 교주의 아들까지 뛰어넘어버린 유현이였다.
하지만 아무리 마교가 힘을 숭배하는 집단이라 하나 유현을 배척하는 집단이 많았다.마교의 전통을 따라야 한다며 현 마교 교주의 아들인 사무연도 인정한 유현을 배척해 이렇게까지 몰아붙이고 있는 것이었다. 유현은 장로들을 향해 이를 부득부득 갈았다.
_그 씹새끼들, 걸리기만 해봐라! 완전 아작을 내버릴 테닷!_
유현은 그렇게 생각하며 몸을 날렸다.
그는 경공술을 이용해 빠르게 이곳을 벗어나야 했다.
아무리 천재에다 소교주라 하지만 유현은 아직 24세의 젊은 사내였다.게다가 외모를 보자면 손은 여자의 그것처럼 고왔으며 얼굴선과 이목구비는 여자의 그것처럼 아름다운, 너무도 아름다운 하유현.그가 달리고 있었다.
추적자들은 유현에게 아직도 그런 힘이 남아 있는지 몰랐기에 당혹한 표정을 지었다.하지만 다음에 들리는 유현의 섬뜩한 웃음을 듣고 그대로 세상을 하직하고 말았다.
-킥킥킥킥, 병신들.-
툭!
투두두두둑!
10명 정도의 추적자들의 목이 머리에서 떨어져 내렸다.천잠사...유현이 그것을 이용해 30명 정도의 추적자 중 3분의 1, 즉 10명에 해당하는 이들의 목을 따버린 것이다.
-킥킥킥, 나 혼자 죽을 수야 없지.와라! 걸리는 놈은 다 세상 하직시켜주마!-
그리고 기세를 뿜는 유현이였다. 마교의 추척자들은 그것을 보며 침을 꿀꺽 삼켰다.역시 마교의 소교주는 괜히 된 것이 아니었다.하지만 이미 지칠 대로 지친 유현... 게다가 내공도 거의 바닥을 기고 있었다. 그러나 그는 여유로웠다.이것이 바로 죽음을 직면한 자의 여유로움일까?
하지만 여유로운 것과 달리 속은 그렇지 않았다.
_크흑, 여기서 끝인가? 무연 형, 혈사 아저씨... 저, 여기서 죽습니다._
자신에게 잘 대해주는 교주 사혈사. 그들을 생각하니 유현의 눈가에 언뜻 슬픔이 스치고 지나갔다.
그렇게 침울한 표정을 짓기 시작하는 유현의 모습은 그야말로 미인도에서 툭 튀어나온 모습이 아닐 수 없었다.추적자들도 그 모습에 움찔했다. 무림오화보다 아름답다고 하는 유현이다. 남자라지만 너무도 아름다운 유현의 외모는 인간의 그것과 거리가 멀었다.꼭 선녀가 강림한 것 같은 얼굴이랄까?
그러나 정작 유현은 이런 자신의 외모에 대해 자각이 전혀 없었다. 추적자들에게서 빈틈을 발견한 유현이 소리쳤다.
-빈틈이닷!-
촤악!
분명 추적자를 베었다.하지만 이상하게도 죽어가는 자는 비명 하나 지르지 않았다. 그것에 기가 질리는 유현이었다. 아무리 살수들이라지만 어찌된 것들이기에 죽을 때 비명조차 지르지 않는단 말인가!
유현은 질린 눈으로 그들을 보다가 자신의 뒤를 덮치는 이에게 뒤차기를 작렬시켰다.
퍽!
그리고 옆을 베어오는 이의 목을 깔끔하게 그어버렸다.하지만 거기까지였다. 유현의 발악은 딱 거기까지였던 것이다.
푹!
자신의 배를 뚫고 나오는 검을 쳐다보며 유현이 말한다.
-이런 씨발!- 
역시 입에 달린 욕이라 죽음과 직면했을 때도 튀어나왔다. 그러고는 풀썩 주저앉았다.그런 유현을 살수들은 무심한 눈으로 바라보고만 있었다.그때였다.살수의 우두머리로 보이는 이가 입을 열었다.
-소교주님, 유언이 있다면 말씀하십시오.-
유현이 살수의 우두머리를 바라보다가 웃으며 말했다.
-만약 내가, 내가 다시 환생한다면...-
유현은 그렇게 말한 뒤 화사하게 웃으며 말한다.
-장로들 목을 다 따버리겠다고 전해줘.-
그 말을 끝으로 유현이 고개를 떨궜다. 그런 유현을 무심하게 바라보던 살수들의 우두머리가 그를 향해 예를 표했다.
-알겠습니다, 소교주님.-
그들은 유현의 시신을 처리하지 않았다.죽이라고 했지, 시신까지 처리하라는 명은 듣지 못했기 때문이다. 평소대로라면 시신을 처리했겠지만 장로들의 행동이 마음에 들지 않는 추적자들은 유현의 시신을 그대로 방치했다.
다음날, 유현의 시신이 발견되었다.
무연은 큰 충격에 빠졌다. 혈사 역시 마찬가지였다.
무연은 자신이 친동생처럼 아끼던 유현의 죽음을 믿지 못했고, 혈사는 친자식 같은 유현의 죽음에 연루된 자들을 철저히 조사하여 능지처참 시켜버렸다.
그렇게 마교에 피바람이 불었다. 하지만 조용히 웃고 있는 듯한 유현의 시신은 너무도 평화로워 보였다.";
    }
}
