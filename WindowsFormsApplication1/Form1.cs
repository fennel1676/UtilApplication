using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AvSimDll.MotionExternC;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;

using AKI.MotionFunction;
using AKI.Win32.API;
using System.IO;
using System.Drawing.Imaging;
using AKI.Communication.Util;
using System.Net.NetworkInformation;
using System.Threading;
using unirest_net.http;
using Newtonsoft.Json;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public delegate bool EnumWindowCallback(int hwnd, int lParam);

        [DllImport("user32.dll")]
        public static extern int EnumWindows(EnumWindowCallback callback, int y);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(int hWnd, StringBuilder text, int count);

        AKI.Util.KeyboardHookUtil util = new AKI.Util.KeyboardHookUtil();
        AKI.Util.JoystickUtil utilJoystick = new AKI.Util.JoystickUtil();

        public Form1()
        {
            InitializeComponent();

            //button1.AddMouseEventImage(AKI.Control.Button.eMouseEvent.Enable, global::WindowsFormsApplication1.Properties.Resources.pni_logo_icon);
            //button1.AddMouseEventImage(AKI.Control.Button.eMouseEvent.MouseLeave, global::WindowsFormsApplication1.Properties.Resources.캡처);
            //button1.AddMouseEventImage(AKI.Control.Button.eMouseEvent.MouseMove, global::WindowsFormsApplication1.Properties.Resources.KakaoTalk_20170703_085201014);
            button1.AddMouseEventImage(0, AKI.Control.Button.eMouseEvent.MouseLeave, global::WindowsFormsApplication1.Properties.Resources.캡처);
            button1.AddMouseEventImage(0, AKI.Control.Button.eMouseEvent.MouseMove, global::WindowsFormsApplication1.Properties.Resources.KakaoTalk_20170703_085201014);
            button1.AddMouseEventImage(1, AKI.Control.Button.eMouseEvent.MouseLeave, global::WindowsFormsApplication1.Properties.Resources.KakaoTalk_20170703_085201014);
            button1.AddMouseEventImage(1, AKI.Control.Button.eMouseEvent.MouseMove, global::WindowsFormsApplication1.Properties.Resources.캡처);
            Console.WriteLine(uint.MaxValue);

            //Console.WriteLine(AKI.Util.JoystickUtil.CheckJoystic());
            //utilJoystick.EventJoystickInfo += new AKI.Util.JoystickUtil.EventJoystick(EventJoystick);
            //utilJoystick.Start();
            //int nStyle = AKI.Win32.API.User32.GetWindowLong(this.Handle, (int)AKI.Win32.GWL.EXSTYLE);

            //int nResult = AKI.Win32.API.User32.SetWindowLong(this.Handle, AKI.Win32.GWL.EXSTYLE, (int)AKI.Win32.WS.EX_NOACTIVATE);
            //Console.WriteLine(nResult);

            //AKI.Util.EtcUtil.KeyAdd(Keys.Space);
            //Console.WriteLine(AKI.Util.EtcUtil.KeyRegisterHook(ResultKey));
            util.Hook();
            util.KeyDown += new KeyEventHandler(gkh_KeyDown);
            util.AddKey(Keys.Space);
            util.AddKey(Keys.Home);

            //pni_logo_icon
        }

        void EventJoystick(uint nJoystickId, Winmm.joyinfoexStruct joyinfoex)
        {
            //Console.WriteLine("{0} = {1} / {2} / {3} / {4} / {5} / {6} / {7} / {8} / {9} / {10} / {11}",
            //                nJoystickId, joyinfoex.dwRpos, joyinfoex.dwUpos, joyinfoex.dwVpos, joyinfoex.dwXpos, joyinfoex.dwYpos, joyinfoex.dwZpos,
            //                joyinfoex.dwPOV, joyinfoex.dwFlags, joyinfoex.dwSize, joyinfoex.dwButtons, joyinfoex.dwButtonNumber);
            Console.WriteLine("{0} = {1} / {2}",
                    nJoystickId, joyinfoex.dwYpos, joyinfoex.dwPOV);
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("Down\t" + e.KeyCode.ToString());
            e.Handled = true;
        }

        private IntPtr m_hWndKey;

        public bool EnumWindowsProc(int hWnd, int lParam)
        {
            StringBuilder Buf = new StringBuilder(256);
            //응용프로그램의 이름을 얻어온다   
            if (GetWindowText(hWnd, Buf, 256) > 0)
            {
                if (Buf.ToString().Contains(textBox1.Text))
                {
                    m_hWndKey = (IntPtr)hWnd;
                    Console.WriteLine(Buf.ToString());
                }
            }
            return true;
        }


        public bool InitializeThread()
        {
            try
            {
                Thread thread = new Thread(new ThreadStart(PlayThread));
                thread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        private void PlayThread()
        {
            AvSimDllMotionExternCsharp.MotionControl av = new AvSimDllMotionExternCsharp.MotionControl();
            AvSimDllMotionExternCsharp.EQUIPMENT_EXTEND_DATA pEQDataEx2 = null;
            AvSimDllMotionExternCsharp.EQUIPMENT_DATA pEQData2 = null;

            Console.WriteLine("Initial = {0}", av.Initial());
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("EQExtendData = {0}", av.EQExtendData(out pEQDataEx2));
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("ReadDO = {0}", av.GetDO());
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("ReadDI = {0}", av.GetDI());
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("WriteDO = {0}", av.DO(0x0a));
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("DOF_and_Blower = {0}", av.DOF_and_Blower(0, 1, 2, 3, 4, 5, 6, 7));
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("V2_DOF_and_Blower = {0}", av.V2_DOF_and_Blower(0, 1, 2, 3, 4, 5, 6, 7));
            for (int i = 0; i < 100000; i++)
            {
                System.Threading.Thread.Sleep(100);
                Console.WriteLine("{0} = V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis = {1}", i, av.V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis(20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, out pEQData2));
                System.Threading.Thread.Sleep(100);
                Console.WriteLine("{0} = V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm = {1}", i, av.V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm(40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, out pEQDataEx2));
            }
            Console.WriteLine("Destroy = {0}", av.Destroy());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MotionControl control = new MotionControl();
            //Console.WriteLine("Initial = {0}", control.Initial(2));
            //EQUIPMENT_EXTEND_DATA pEQDataEx = null;
            //EQUIPMENT_DATA pEQData = new EQUIPMENT_DATA();
            //Console.WriteLine("Ping = {0}", control.Ping());
            //Console.WriteLine("State = {0}", control.State());
            //Console.WriteLine("EQExtendData = {0}", control.EQExtendData(out pEQDataEx));
            //Console.WriteLine("ReadDO = {0}", control.ReadDO());
            //Console.WriteLine("ReadDI = {0}", control.ReadDI());
            //Console.WriteLine("WriteDO = {0}", control.WriteDO(0x0e));
            //Console.WriteLine("ReadDO = {0}", control.ReadDO());
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("DOF_and_Blower = {0}", control.DOF_and_Blower(0, 1, 2, 3, 4, 5, 6, 7));
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("DOF_and_Blower_and_DO = {0}", control.DOF_and_Blower_and_DO(10, 11, 22, 33, 44, 55, 66, 77, 0x04));
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("V2_DOF_and_Blower = {0}", control.V2_DOF_and_Blower(0, 1, 2, 3, 4, 5, 6, 7));
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("V2_DOF_and_Blower_and_DO = {0}", control.V2_DOF_and_Blower_and_DO(10, 11, 22, 33, 44, 55, 66, 77, 0x0a));
            //Console.WriteLine("WriteDO = {0}", control.WriteDO(0x0f));
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("ReadDO = {0}", control.ReadDO());
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis = {0}", control.V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis(20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34,35, 36, out pEQData));
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm = {0}", control.V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm(40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, out pEQDataEx));
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("ReadDO = {0}", control.ReadDO());
            //Console.WriteLine("Destroy = {0}", control.Destroy());
            //button1.Enabled = false;

            //AvSimDllMotionExternCsharp.EQUIPMENT_EXTEND_DATA pEQDataEx2 = null;
            //AvSimDllMotionExternCsharp.EQUIPMENT_DATA pEQData2 = null;

            //Console.WriteLine("Initial = {0}", av.Initial());
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("EQExtendData = {0}", av.EQExtendData(out pEQDataEx2));
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("ReadDO = {0}", av.GetDO());
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("ReadDI = {0}", av.GetDI());
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("WriteDO = {0}", av.DO(0x0a));
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("DOF_and_Blower = {0}", av.DOF_and_Blower(0, 1, 2, 3, 4, 5, 6, 7));
            //System.Threading.Thread.Sleep(50);
            //Console.WriteLine("V2_DOF_and_Blower = {0}", av.V2_DOF_and_Blower(0, 1, 2, 3, 4, 5, 6, 7));
            //for (int i = 0; i < 1000; i++)
            //{
            //    System.Threading.Thread.Sleep(100);
            //    Console.WriteLine("V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis = {0}", av.V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis(20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, out pEQData2));
            //    System.Threading.Thread.Sleep(100);
            //    Console.WriteLine("V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm = {0}", av.V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm(40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, out pEQDataEx2));
            //}

            //Console.WriteLine("Destroy = {0}", av.Destroy());

            InitializeThread();

            return;
            //byte[] aaa = MotionControl.GetCommand_DOF(1000, 2000, 3000, 4000, 5000, 6000, 10);
            return;

            List<string> listExceptionFile = new List<string>();
            listExceptionFile.Add("AKI.MotionData.dll");
            listExceptionFile.Add("AKI.MotionFunction.dll");
            List<IMotionFunction> listMotionFunction = new List<IMotionFunction>();
            AKI.Util.LibraryUtil.Load<IMotionFunction>(".\\MotionFunctions", listExceptionFile, ref listMotionFunction);

            return;
            ////bool isResult = AKI.Util.EtcUtil.RegisterRawInputDevice(this.Handle);
            //bool isResult = AKI.Util.EtcUtil.HotKeyRegister(this.Handle, 4444, Keys.Space);
            //Console.WriteLine(isResult);

            //AKI.Util.EtcUtil.SendMessage(this.Handle, AKI.Win32.WM.CHAR, (int)Keys.Space, 0);

            //EnumWindowCallback callback = new EnumWindowCallback(EnumWindowsProc);
            //EnumWindows(callback, 0);

            //this.Hide();

            IntPtr hWnd = AKI.Win32.API.User32.FindWindow(null, "Whirligig");
            if (IntPtr.Zero == hWnd)
                Console.WriteLine("asdfasdf");

            AKI.Win32.API.User32.SetForegroundWindow(hWnd);
            System.Windows.Forms.SendKeys.Send("{HOME}");
            //AKI.Win32.API.User32.SetActiveWindow(hWnd);
            //AKI.Win32.API.User32.SetFocus(hWnd);

            //uint uiKey = AKI.Win32.API.User32.MapVirtualKey(AKI.Win32.VK.SPACE, AKI.Win32.VK.MAPVK_VK_TO_VSC);
            //AKI.Win32.API.User32.keybd_event((byte)AKI.Win32.VK.SPACE, (byte)uiKey, 0, IntPtr.Zero);
            //AKI.Win32.API.User32.keybd_event((byte)AKI.Win32.VK.SPACE, (byte)uiKey, AKI.Win32.VK.KEYEVENTF_KEYUP, IntPtr.Zero);

            //timer1.Interval = 10000;
            //timer1.Enabled = true;
            //timer1.Start();
        }

        public void EventPingStart(int nId, string strHost, string strMessage)
        {
            Console.WriteLine(strMessage);
        }
        public void EventPingRun(int nId, EndPoint ep, int nIndex, double dElapsedtime, int nMessageSize, string strMessage)
        {
            Console.WriteLine(strMessage);
        }
        public void EventPingMessage(int nId, string strMessage)
        {
            Console.WriteLine(strMessage);
        }
        public void EventPingStop(int nId)
        {
            Console.WriteLine(nId);

//            --a3ba8936 - 86dd - 4eef - b64c - 56845948fdcc
//Content - Type: text / plain; charset = utf - 8
//Content - Disposition: form - data; name = barcode

//1111
//--a3ba8936 - 86dd - 4eef - b64c - 56845948fdcc
//Content - Type: text / plain; charset = utf - 8
//Content - Disposition: form - data; name = ccId

//2222
//--a3ba8936 - 86dd - 4eef - b64c - 56845948fdcc--
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AvSimDll.MotionExternC.MotionControl motion = new MotionControl();
            motion.Initial(3);
            int nResult = 0;
            string strResult = string.Empty;
            motion.Wifi_ReadTcpSocketMode(ref nResult);
            Console.WriteLine("Wifi_ReadTcpSocketMode = {0}", nResult);
            motion.Wifi_ReadWifiMode(ref nResult);
            Console.WriteLine("Wifi_ReadWifiMode = {0}", nResult);
            motion.Wifi_ReadWifiChannel(ref nResult);
            Console.WriteLine("Wifi_ReadWifiChannel = {0}", nResult);
            motion.Wifi_ReadBluetoothDeviceId(ref strResult);
            Console.WriteLine("Wifi_ReadBluetoothDeviceId = {0}", strResult);
            motion.Wifi_ReadBluetoothDevicePassword(ref strResult);
            Console.WriteLine("Wifi_ReadBluetoothDevicePassword = {0}", strResult);
            motion.Wifi_ReadSocketServerIP(ref strResult);
            Console.WriteLine("Wifi_ReadSocketServerIP = {0}", strResult);
            motion.Wifi_ReadSocketServerSubnetMask(ref strResult);
            Console.WriteLine("Wifi_ReadSocketServerSubnetMask = {0}", strResult);
            motion.Wifi_ReadSocketServerGateway(ref strResult);
            Console.WriteLine("Wifi_ReadSocketServerGateway = {0}", strResult);
            motion.Wifi_ReadSocketServerDNS(ref strResult);
            Console.WriteLine("Wifi_ReadSocketServerDNS = {0}", strResult);
            motion.Wifi_ReadSocketClientIP(ref strResult);
            Console.WriteLine("Wifi_ReadSocketClientIP = {0}", strResult);
            motion.Wifi_ReadSocketClientSubnetMask(ref strResult);
            Console.WriteLine("Wifi_ReadSocketClientSubnetMask = {0}", strResult);
            motion.Wifi_ReadSocketClientGateway(ref strResult);
            Console.WriteLine("Wifi_ReadSocketClientGateway = {0}", strResult);
            motion.Wifi_ReadSocketClientDNS(ref strResult);
            Console.WriteLine("Wifi_ReadSocketClientDNS = {0}", strResult);
            //motion.Wifi_WriteTcpSocketMode(2);
            //motion.Wifi_WriteWifiMode(2);
            //motion.Wifi_WriteWifiChannel(4);
            //motion.Wifi_WriteBluetoothDeviceId("444444");
            //motion.Wifi_WriteBluetoothDevicePassword("87654321");
            //motion.Wifi_WriteSocketServerIP("192.168.10.10");
            //motion.Wifi_WriteSocketServerSubnetMask("255.255.0.0");
            //motion.Wifi_WriteSocketServerGateway("192.168.10.1");
            //motion.Wifi_WriteSocketServerDNS("168.126.63.10");
            //motion.Wifi_WriteSocketClientIP("192.168.10.20");
            //motion.Wifi_WriteSocketClientSubnetMask("255.255.128.0");
            //motion.Wifi_WriteSocketClientGateway("192.168.10.1");
            //motion.Wifi_WriteSocketClientDNS("168.126.63.100");


            //motion.Wifi_WriteTcpSocketMode(1);
            //motion.Wifi_WriteWifiMode(1);
            //motion.Wifi_WriteWifiChannel(5);
            //motion.Wifi_WriteBluetoothDeviceId("100018");
            //motion.Wifi_WriteBluetoothDevicePassword("12345678");
            //byte[] aServerIp = new byte[4] { 192, 168, 1, 1 };
            //motion.Wifi_WriteSocketServerIP(aServerIp);
            //byte[] aServerSubnetMask = new byte[4] { 255, 255, 255, 0 };
            //motion.Wifi_WriteSocketServerSubnetMask(aServerSubnetMask);
            //byte[] aServerGateway = new byte[4] { 192, 168, 10, 1 };
            //motion.Wifi_WriteSocketServerGateway(aServerGateway);
            //byte[] aServerDns = new byte[4] { 168, 126, 63, 1 };
            //motion.Wifi_WriteSocketServerDNS(aServerDns);
            //byte[] aClientIp = new byte[4] { 192, 168, 1, 2 };
            //motion.Wifi_WriteSocketClientIP(aClientIp);
            //byte[] aClientSubnetMask = new byte[4] { 255, 255, 255, 0 };
            //motion.Wifi_WriteSocketClientSubnetMask(aClientSubnetMask);
            //byte[] aClientGateway = new byte[4] { 192, 168, 1, 1 };
            //motion.Wifi_WriteSocketClientGateway(aClientGateway);
            //byte[] aClientDns = new byte[4] { 168, 126, 63, 1 };
            //motion.Wifi_WriteSocketClientDNS(aClientDns);

            //motion.Wifi_WriteTcpSocketMode(1);
            //System.Threading.Thread.Sleep(1000);
            //motion.Wifi_WriteWifiMode(1);
            //System.Threading.Thread.Sleep(1000);
            //motion.Wifi_WriteWifiChannel(6);
            //System.Threading.Thread.Sleep(1000);
            //motion.Wifi_WriteBluetoothDeviceId("100018");
            //System.Threading.Thread.Sleep(1000);
            //motion.Wifi_WriteBluetoothDevicePassword("12345678");
            //System.Threading.Thread.Sleep(1000);
            //motion.Wifi_WriteSocketServerIP("192.168.1.1");
            //System.Threading.Thread.Sleep(1000);
            //motion.Wifi_WriteSocketServerSubnetMask("255.255.255.0");
            //System.Threading.Thread.Sleep(1000);
            //motion.Wifi_WriteSocketServerGateway("192.168.1.1");
            //System.Threading.Thread.Sleep(1000);
            //motion.Wifi_WriteSocketServerDNS("168.126.63.1");
            //System.Threading.Thread.Sleep(1000);
            //motion.Wifi_WriteSocketClientIP("192.168.1.2");
            //System.Threading.Thread.Sleep(1000);
            //motion.Wifi_WriteSocketClientSubnetMask("255.255.255.0");
            //System.Threading.Thread.Sleep(1000);
            //motion.Wifi_WriteSocketClientGateway("192.168.1.1");
            //System.Threading.Thread.Sleep(1000);
            //motion.Wifi_WriteSocketClientDNS("168.126.63.1");

            motion.Destroy();
            return;
            WebClient webClient2 = new WebClient();
            byte[] resByte;
            string resString;
            string reqString;
            byte[] reqByte;
            try
            {
                Dictionary<string, object> dictData = new Dictionary<string, object>();
                dictData["barcode"] = "1111";
                dictData["ccId"] = "2222";

                string urlToPost = @"http://192.168.0.48:8090/PNI_SERVER/contents/getContentsOn";
                webClient2.Headers["accept"] = "application/json";
                webClient2.Headers["Content-Type"] = "text/plain";
                webClient2.Headers["Content-Disposition"] = "form-data";
                reqByte = Encoding.Default.GetBytes(JsonConvert.SerializeObject(dictData, Formatting.Indented));
                //reqString = "barcode=1111";
                //reqString += "&ccId=2222";
                //reqByte = Encoding.UTF8.GetBytes(reqString);
                resByte = webClient2.UploadData(urlToPost, "post", reqByte);
                resString = Encoding.UTF8.GetString(resByte);
                Console.WriteLine(resString);

                var response = JsonConvert.DeserializeObject<ㅁㅊ_GetContentsOnOffStop>(resString);
                Console.WriteLine(response.response.header.statusCode);
                Console.WriteLine(response.response.body.resultCode);
                Console.WriteLine(response.response.body.resultMsg);

                webClient2.Dispose();
                return ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return;

            WebClient webClient = new WebClient();
            string uri = @"http://192.168.0.48:8090/PNI_SERVER/contents/";
            string requestJson = "getContentsOn";
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            webClient.Encoding = UTF8Encoding.UTF8;
            try
            {
                string responseJSON = webClient.UploadString(uri, requestJson);
                Console.WriteLine(responseJSON);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return;


            //
            string offlinetext = File.ReadAllText("offline.txt");
            var temp2 = Newtonsoft.Json.JsonConvert.DeserializeObject<BoughtDataList>(offlinetext);
            Console.WriteLine(temp2.ToString());

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(temp2);
            Console.WriteLine(json);
            return;

            MagicPacketSettingXml2 xml = null;
            AKI.Util.XmlUtil.Load<MagicPacketSettingXml2>(out xml, "C:\\Users\\Administrator\\Documents\\PNISYS\\Launcher\\Config\\m62_hmd_01\\MagicPacket.xml");
            if (null == xml.MagicPacket)
                xml.MagicPacket = new List<MagicPacketInfo>();

            MagicPacketInfo info = new MagicPacketInfo();
            info.Name = "aaa";
            info.IP = "192.168.0.1";
            info.Mac = "1122334455";
            xml.MagicPacket.Add(info);
            info = new MagicPacketInfo();
            info.Name = "bbb";
            info.IP = "192.168.0.2";
            info.Mac = "6622334455";

            xml.MagicPacket.Add(info);
            info = new MagicPacketInfo();
            info.Name = "ccc";
            info.IP = "192.168.0.3";
            info.Mac = "6677884455";
            xml.MagicPacket.Add(info);

            xml.Name = "aaabbbccc";
            xml.IP = "192.168.0.123";
            xml.Mac = "112233445544";
            string strXml = string.Empty;
            Type t = xml.GetType();
            //AKI.Util.XmlUtil.SaveXml<t>(xml, out strXml);
            AKI.Util.XmlUtil.SaveXml<MagicPacketSettingXml>(xml, out strXml);
            //GeneralPacketWrapper packet = new GeneralPacketWrapper(100, @"  <Log>    <IsDeleteLog>true</IsDeleteLog>    <DeleteDay>30</DeleteDay>    <FilePath>Log</FilePath>    <FileType>log</FileType>    <NameType>yyyyMMdd_HH</NameType>  </Log> 123  나다. 이렇게 해볼가? zzzzz");
            GeneralPacketWrapper packet = new GeneralPacketWrapper(100, strXml);


            byte[] aData = packet.Struct2Bytes();
            try
            {
                GeneralPacketWrapper packet2 = new GeneralPacketWrapper(aData, packet.FullSize);

                MagicPacketSettingXml xml2 = packet2.Xml2Object<MagicPacketSettingXml>();
                Console.WriteLine(packet2.BodyString);
                Console.WriteLine(packet2.BodySize);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Complete");
            }

            return;
            AKI.Communication.Util.Ping ping = new AKI.Communication.Util.Ping();
            ping.Initialize();
            //ping.IpAddress = "192.168.0.1";
            ping.SendCount = 10;
            ping.MessageSize = 32;
            ping.Event_PingStart += new AKI.Communication.Util.Ping.EventPingStart(EventPingStart);
            ping.Event_PingRun += new AKI.Communication.Util.Ping.EventPingRun(EventPingRun);
            ping.Event_PingMessage += new AKI.Communication.Util.Ping.EventPingMessage(EventPingMessage);
            ping.Event_PingStop += new AKI.Communication.Util.Ping.EventPingStop(EventPingStop);
            ping.Send();
            ping.SendCount = 4;
            ping.IpAddress = "192.168.0.1";
            ping.Send();

            return;
            //
            // Get my PC IP address
            Console.WriteLine("My IP : {0}", NetworkInfoUtil.GetIPAddress());
            // Get My PC MAC address
            Console.WriteLine("My MAC: {0}", NetworkInfoUtil.GetMacAddress());
            // Get all devices on network
            Dictionary<IPAddress, PhysicalAddress> all = NetworkInfoUtil.GetAllDevicesOnLAN();
            foreach (KeyValuePair<IPAddress, PhysicalAddress> kvp in all)
            {
                Console.WriteLine("IP : {0}\n MAC {1}", kvp.Key, kvp.Value);
            }

            Console.WriteLine("IP : {0}\n MAC {1}", "192.168.0.1", NetworkInfoUtil.GetTargetDevicesOnLAN("192.168.0.1"));

            PhysicalAddress pa = NetworkInfoUtil.GetTargetDevicesOnLAN("192.168.0.1");
            if (null == pa)
                Console.WriteLine("Not Exist IP Address");
            MagicPacket.Create();
            if (MagicPacket.SendMagicPacket(pa.GetAddressBytes()))
                Console.WriteLine("Send Success");
            else
                Console.WriteLine("Send Fail");
            return;

            IntPtr hWnd = AKI.Win32.API.User32.FindWindow(null, "Whirligig");
            if (IntPtr.Zero == hWnd)
                Console.WriteLine("asdfasdf");

            AKI.Win32.API.User32.SetForegroundWindow(hWnd);
            //AKI.Util.EtcUtil.SetKeyAction((int)Keys.Space);
            //AKI.Util.EtcUtil.SetKeyAction((int)Keys.Home);
            return;
            bool isResult = AKI.Util.EtcUtil.HotKeyUnregister(this.Handle, 4444);
            Console.WriteLine(isResult);

            return;
            byte[] data = new byte[1024];
            string input, stringData;
            TcpClient server;

            try
            {
                //server = new TcpClient("127.0.0.1", 4444);
                server = new TcpClient("192.168.126.9", 4444);
            }
            catch (SocketException)
            {
                Console.WriteLine("Unable to connect to server");
                return;
            }
            NetworkStream ns = server.GetStream();

            input = "Start";
            ns.Write(Encoding.ASCII.GetBytes(input), 0, input.Length);
            ns.Flush();

            System.Threading.Thread.Sleep(5000);
            input = "Stop";
            ns.Write(Encoding.ASCII.GetBytes(input), 0, input.Length);
            ns.Flush();

            System.Threading.Thread.Sleep(5000);
            input = "Exit";
            ns.Write(Encoding.ASCII.GetBytes(input), 0, input.Length);
            ns.Flush();

            Console.WriteLine("Disconnecting from server...");
            ns.Close();
            server.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string resString;
            string reqString;
            byte[] reqByte;

            reqString = "barcode=1111";
            reqString += "&ccId=2222";
            reqByte = Encoding.UTF8.GetBytes(reqString);

            //HttpResponse<string> jsonResponse = Unirest.post(@"http://192.168.0.48:8090/PNI_SERVER/contents/getContentsOn")
            //                    .header("accept", "application/json")
            //                    .field("barcode", "1111")
            //                    .field("ccId", "2222")
            //                    .asJson<string>();

            HttpResponse<string> jsonResponse = Unirest.post(@"http://192.168.0.48:8090/PNI_SERVER/contents/getContentsOn")
                    .header("accept", "application/json")
                    .field("barcode", "1111")
                    .field("ccId", "2222")
                    .asJson<string>();

            int Error = jsonResponse.Code;
            if (200 == Error)
            {
                var response = JsonConvert.DeserializeObject<ㅁㅊ_GetContentsOnOffStop>(jsonResponse.Body);
                Console.WriteLine(response.response.header.statusCode);
                Console.WriteLine(response.response.body.resultMsg);
                Console.WriteLine(response.response.body.resultMsg);
            }
            return;

            return;

            int recv;
            byte[] data = new byte[1024];

            TcpListener newsock = new TcpListener(4444);
            newsock.Start();
            Console.WriteLine("Waiting for a client...");

            TcpClient client = newsock.AcceptTcpClient();
            NetworkStream ns = client.GetStream();

            string welcome = "Start";
            data = Encoding.ASCII.GetBytes(welcome);
            ns.Write(data, 0, data.Length);
            System.Threading.Thread.Sleep(5000);
            welcome = "Stop";
            data = Encoding.ASCII.GetBytes(welcome);
            ns.Write(data, 0, data.Length);
            System.Threading.Thread.Sleep(5000);
            welcome = "Exit";
            data = Encoding.ASCII.GetBytes(welcome);
            ns.Write(data, 0, data.Length);

            ns.Close();
            client.Close();
            newsock.Stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> dictData = new Dictionary<string, object>();
            dictData["barcode"] = "1111";
            dictData["ccId"] = "2222";


            HttpResponse<string> jsonResponse = Unirest.post(@"http://192.168.0.48:8090/PNI_SERVER/contents/getContentsOn")
                    .header("accept", "application/json")
                    .fields(dictData)
                    .asJson<string>();

            int Error = jsonResponse.Code;
            if (200 == Error)
            {
                var response = JsonConvert.DeserializeObject<ㅁㅊ_GetContentsOnOffStop>(jsonResponse.Body);
                Console.WriteLine(response.response.header.statusCode);
                Console.WriteLine(response.response.body.resultMsg);
                Console.WriteLine(response.response.body.resultMsg);
            }

            return;
            return;

            byte[] data = new byte[1024];
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 4445);
            UdpClient newsock = new UdpClient(ipep);

            Console.WriteLine("Waiting for a client...");

            IPEndPoint sender1 = new IPEndPoint(IPAddress.Broadcast, 4444);

            string welcome = "Start";
            data = Encoding.ASCII.GetBytes(welcome);
            newsock.Send(data, data.Length, sender1);
            welcome = "Stop";
            data = Encoding.ASCII.GetBytes(welcome);
            newsock.Send(data, data.Length, sender1);
            welcome = "End";
            data = Encoding.ASCII.GetBytes(welcome);
            newsock.Send(data, data.Length, sender1);
            newsock.Close();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine(e.KeyChar);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.KeyCode);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.KeyCode);
        }
        const int WM_INPUT = 0x00FF;
        const int WM_KEYDOWN = 0x0100;
        const int WM_KEYUP = 0x0101;
        const int WM_CHAR = 0x0102;
        const int WM_SYSCHAR = 0x0106;
        const int WM_SYSKEYDOWN = 0x0104;
        const int WM_SYSKEYUP = 0x0105;
        const int WM_IME_CHAR = 0x0286;

        protected override void WndProc(ref Message m)
        {
            //if (m.Msg == WM_INPUT)
            //    Console.WriteLine(AKI.Util.EtcUtil.KeyTranslateMessage(m.LParam));

            base.WndProc(ref m); // May or may not need this call.
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> dictData = new Dictionary<string, object>();
            dictData["barcode"] = "1111";
            dictData["ccId"] = "2222";


            HttpResponse<string> jsonResponse = Unirest.post(@"http://192.168.0.48:8090/PNI_SERVER/contents/getContentsOn")
                    .header("accept", "application/json")
                    .body(JsonConvert.SerializeObject(dictData, Formatting.Indented))
                    .asJson<string>();

            int Error = jsonResponse.Code;
            if (200 == Error)
            {
                var response = JsonConvert.DeserializeObject<ㅁㅊ_GetContentsOnOffStop>(jsonResponse.Body);
                Console.WriteLine(response.response.header.statusCode);
                Console.WriteLine(response.response.body.resultMsg);
                Console.WriteLine(response.response.body.resultMsg);
            }

            return;
            return;
            //byte bKey = 0;
            //if (AKI.Util.EtcUtil.GetKeyboard((int)Keys.Space))
            //    Console.WriteLine(bKey);

            //AKI.Util.EtcUtil.KeyAdd(Keys.Space);
            //AKI.Util.EtcUtil.KeyAdd(Keys.Escape);
            //Console.WriteLine(AKI.Util.EtcUtil.KeyRegisterInputDevice(this.Handle));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            //AKI.Util.EtcUtil.SetKeyAction((uint)AKI.Win32.VK.SPACE);
            IntPtr hWnd = AKI.Win32.API.User32.FindWindow(null, "Whirligig");
            if (IntPtr.Zero == hWnd)
                Console.WriteLine("asdfasdf");

            AKI.Win32.API.User32.SetForegroundWindow(hWnd);
            AKI.Win32.API.User32.SetActiveWindow(hWnd);
            AKI.Win32.API.User32.SetFocus(hWnd);

            uint uiKey = AKI.Win32.API.User32.MapVirtualKey(AKI.Win32.VK.SPACE, AKI.Win32.VK.MAPVK_VK_TO_VSC);
            AKI.Win32.API.User32.keybd_event((byte)AKI.Win32.VK.SPACE, (byte)uiKey, 0, IntPtr.Zero);
            AKI.Win32.API.User32.keybd_event((byte)AKI.Win32.VK.SPACE, (byte)uiKey, AKI.Win32.VK.KEYEVENTF_KEYUP, IntPtr.Zero);

        }

        private void xpButton1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("adsfas");
        }

        private void textBox1_Event_EndKey(Keys key)
        {
            Console.WriteLine("adsfas");
        }

        private void m_buttonFolder_Click(object sender, EventArgs e)
        {
            m_folderBrowserDialog.ShowDialog();
            m_textBoxFolder.Text = m_folderBrowserDialog.SelectedPath;
        }

        private void m_buttonRun_Click(object sender, EventArgs e)
        {
            DirectoryInfo directory = new DirectoryInfo(m_textBoxFolder.Text);
            DirectoryInfo[] directories = directory.GetDirectories();

            if (null == directories || 0 == directories.Count())
                Run(directory.FullName);
            else
            {
                foreach(DirectoryInfo dir in directories)
                {
                    Run(dir.FullName);
                }
            }
        }

        private void Run(string strPath)
        {
            DirectoryInfo directory = new DirectoryInfo(strPath);
            FileInfo[] files = directory.GetFiles();
            int nTop = int.Parse(m_textBoxTop.Text);
            int nBottom = int.Parse(m_textBoxBottom.Text);
            foreach (FileInfo file in files)
            {
                Bitmap bitmap2 = null;
                using (Bitmap bitmap = new Bitmap(file.FullName))
                {
                    Image img = (Image)bitmap;

                    if (null == bitmap2)
                        bitmap2 = new Bitmap(bitmap.Size.Width, nBottom - nTop);

                    using (Graphics g = Graphics.FromImage(bitmap2))
                    {
                        g.DrawImage(img, 0, -nTop);
                    }
                    bitmap2.Save(Path.Combine(strPath, "_" + file.Name), ImageFormat.Png);
                }
                bitmap2.Dispose();
                file.Delete();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("barcode", "333333");
            postParameters.Add("ccId", "44444444");

            string postURL = @"http://192.168.0.48:8090/PNI_SERVER/contents/getContentsOn";
            string userAgent = "Someone";
            //HttpWebResponse webResponse = MultipartFormDataPost(postURL, userAgent, postParameters);
            HttpWebResponse webResponse = AKI.Communication.WebCommunication.Webnet.post(postURL)
                                    .header("accept", "application/json")
                                    .field("barcode", "eeeee")
                                    .field("ccId", "fffff")
                                    .asJson();

            if (HttpStatusCode.OK == webResponse.StatusCode)
            {
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                webResponse.Close();

                var response = JsonConvert.DeserializeObject<ㅁㅊ_GetContentsOnOffStop>(fullResponse);
                Console.WriteLine(response.response.header.statusCode);
                Console.WriteLine(response.response.body.resultMsg);
                Console.WriteLine(response.response.body.resultMsg);
            }
            else
            {
                Console.WriteLine(webResponse.StatusCode);
            }
        }

        private static readonly Encoding encoding = Encoding.UTF8;
        public static HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

            return PostForm(postUrl, userAgent, contentType, formData);
        }
        private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }

            // Set up the request properties.
            request.Method = "POST";
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;

            // You could add authentication here as well if needed:
            // request.PreAuthenticate = true;
            // request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
            // request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes("username" + ":" + "password")));

            // Send the form data to the request.
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }

            try
            {
                WebResponse response = request.GetResponse();

                return response as HttpWebResponse;
                //return request.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                return ex.Response as HttpWebResponse;
            }
        }

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {
                // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                    // Write the file data directly to the Stream, rather than serializing it to a string.
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

        public class FileParameter
        {
            public byte[] File { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] file, string filename, string contenttype)
            {
                File = file;
                FileName = filename;
                ContentType = contenttype;
            }
        }

    }
}
