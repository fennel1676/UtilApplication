using AKI.Communication.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {
        public static string SerializeString(string str)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('\"');

            char[] charArray = str.ToCharArray();
            foreach (var c in charArray)
            {
                switch (c)
                {
                    case '"':
                        builder.Append("\\\"");
                        break;
                    case '\\':
                        builder.Append("\\\\");
                        break;
                    case '\b':
                        builder.Append("\\b");
                        break;
                    case '\f':
                        builder.Append("\\f");
                        break;
                    case '\n':
                        builder.Append("\\n");
                        break;
                    case '\r':
                        builder.Append("\\r");
                        break;
                    case '\t':
                        builder.Append("\\t");
                        break;
                    default:
                        int codepoint = Convert.ToInt32(c);
                        if ((codepoint >= 32) && (codepoint <= 126))
                        {
                            builder.Append(c);
                        }
                        else
                        {
                            builder.Append("\\u");
                            builder.Append(codepoint.ToString("x4"));
                        }
                        break;
                }
            }

            builder.Append('\"');

            return builder.ToString();
        }
        
        //static void Main(string[] args)
        //{
        //    Console.WriteLine(Program.SerializeString("Fantasy & Action"));
        //    Console.WriteLine(Program.SerializeString("The attack of the dinosaurs in the center of the city!!!"));
        //    Console.WriteLine(Program.SerializeString("Tour bike rickshaw with fantastic scenery of Great Wall of China!!!"));
        //    ResponseContentsStatus_SW xml = new ResponseContentsStatus_SW();
        //    string strXml = null;

        //    xml.Head = new ResponseStatus_Head();
        //    xml.Head.ID = 100;
        //    xml.Body = new ResponseContentsStatus_Body();
        //    xml.Body.RespStatus = 3;
        //    xml.Body.VrMode = 3;
        //    xml.Body.ContentsName = "";
        //    xml.Body.ApplicationName = "";
        //    xml.Alarm.Add(new ResponseStatus_Alarm(1, "EXX01"));
        //    xml.Alarm.Add(new ResponseStatus_Alarm(3, "BXX01"));
        //    AKI.Util.XmlUtil.SaveXml<ResponseContentsStatus_SW>(xml, out strXml);
        //    GeneralPacketWrapper packet = new GeneralPacketWrapper(1, strXml);
        //    byte[] aData = packet.Struct2Bytes();

        //    ResponseSimulatorStatus_HW xml2 = new ResponseSimulatorStatus_HW();
        //    string strXml2 = null;

        //    xml2.Head = new ResponseStatus_Head();
        //    xml2.Head.ID = 100;
        //    xml2.Head.CommandType = 210;
        //    xml2.Body = new ResponseSimulatorStatus_Body();
        //    xml2.Body.Di = 0x41;
        //    xml2.Body.Do = 0x41;
        //    xml2.Body.MotionSpeed = 3;
        //    xml2.Alarm.Add(new ResponseStatus_Alarm(1, "SSEXX01"));
        //    xml2.Alarm.Add(new ResponseStatus_Alarm(3, "SSBXX01"));
        //    AKI.Util.XmlUtil.SaveXml<ResponseSimulatorStatus_HW>(xml2, out strXml2);
        //    GeneralPacketWrapper packet2 = new GeneralPacketWrapper(1, strXml2);
        //    byte[] aData2 = packet.Struct2Bytes();

        //    byte[] data = new byte[1024];
        //    string input, stringData;
        //    IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.0.18"), 30000);

        //    Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //    try
        //    {
        //        server.Connect(ipep);
        //    }
        //    catch (SocketException e)
        //    {
        //        Console.WriteLine("Unable to connect to server.");
        //        Console.WriteLine(e.ToString());
        //        return;
        //    }

        //    int recv = 0;

        //    while (true)
        //    {
        //        input = Console.ReadLine();
        //        if (input == "exit")
        //            break;
        //        else if (input == "a")
        //        {
        //            xml.Alarm.Clear();
        //            xml.Alarm.Add(new ResponseStatus_Alarm(1, "EXX01"));
        //            xml.Alarm.Add(new ResponseStatus_Alarm(2, "AXX01"));
        //            xml.Alarm.Add(new ResponseStatus_Alarm(3, "BXX01"));
        //            AKI.Util.XmlUtil.SaveXml<ResponseContentsStatus_SW>(xml, out strXml);
        //            packet = new GeneralPacketWrapper(110, strXml);
        //            aData = packet.Struct2Bytes();
        //            server.Send(aData, packet.FullSize, SocketFlags.None);
        //        }
        //        else if (input == "b")
        //        {
        //            xml.Alarm.Clear();
        //            xml.Alarm.Add(new ResponseStatus_Alarm(1, "EXX01"));
        //            xml.Alarm.Add(new ResponseStatus_Alarm(3, "BXX01"));
        //            AKI.Util.XmlUtil.SaveXml<ResponseContentsStatus_SW>(xml, out strXml);
        //            packet = new GeneralPacketWrapper(110, strXml);
        //            aData = packet.Struct2Bytes();
        //            server.Send(aData, packet.FullSize, SocketFlags.None);
        //        }
        //        else if (input == "c")
        //        {
        //            xml.Alarm.Clear();
        //            xml.Alarm.Add(new ResponseStatus_Alarm(3, "BXX01"));
        //            AKI.Util.XmlUtil.SaveXml<ResponseContentsStatus_SW>(xml, out strXml);
        //            packet = new GeneralPacketWrapper(110, strXml);
        //            aData = packet.Struct2Bytes();
        //            server.Send(aData, packet.FullSize, SocketFlags.None);
        //        }
        //        else if (input == "d")
        //        {
        //            xml2.Alarm.Clear();
        //            xml2.Alarm.Add(new ResponseStatus_Alarm(1, "SSEXX01"));
        //            xml2.Alarm.Add(new ResponseStatus_Alarm(2, "SSAXX01"));
        //            xml2.Alarm.Add(new ResponseStatus_Alarm(3, "SSBXX01"));
        //            xml2.Body.Di = 0x41;
        //            xml2.Body.Do = 0x41;
        //            AKI.Util.XmlUtil.SaveXml<ResponseSimulatorStatus_HW>(xml2, out strXml2);
        //            packet2 = new GeneralPacketWrapper(210, strXml2);
        //            aData2 = packet2.Struct2Bytes();
        //            server.Send(aData2, packet2.FullSize, SocketFlags.None);
        //        }
        //        else if (input == "e")
        //        {
        //            xml2.Alarm.Clear();
        //            xml2.Alarm.Add(new ResponseStatus_Alarm(1, "SSEXX01"));
        //            xml2.Alarm.Add(new ResponseStatus_Alarm(3, "SSBXX01"));
        //            xml2.Body.Di = 0x00;
        //            xml2.Body.Do = 0x01;
        //            AKI.Util.XmlUtil.SaveXml<ResponseSimulatorStatus_HW>(xml2, out strXml2);
        //            packet2 = new GeneralPacketWrapper(210, strXml2);
        //            aData2 = packet2.Struct2Bytes();
        //            server.Send(aData2, packet2.FullSize, SocketFlags.None);
        //        }
        //        else if (input == "f")
        //        {
        //            xml2.Alarm.Clear();
        //            xml2.Alarm.Add(new ResponseStatus_Alarm(3, "SSBXX01"));
        //            xml2.Body.Di = 0x01;
        //            xml2.Body.Do = 0x00;
        //            AKI.Util.XmlUtil.SaveXml<ResponseSimulatorStatus_HW>(xml2, out strXml2);
        //            packet2 = new GeneralPacketWrapper(210, strXml2);
        //            aData2 = packet2.Struct2Bytes();
        //            server.Send(aData2, packet2.FullSize, SocketFlags.None);
        //        }
        //        else if (input == "g")
        //        {
        //            xml2.Alarm.Clear();
        //            xml2.Body.Di = 0x00;
        //            xml2.Body.Do = 0x00;
        //            AKI.Util.XmlUtil.SaveXml<ResponseSimulatorStatus_HW>(xml2, out strXml2);
        //            packet2 = new GeneralPacketWrapper(210, strXml2);
        //            aData2 = packet2.Struct2Bytes();
        //            server.Send(aData2, packet2.FullSize, SocketFlags.None);
        //        }
        //        else if (input == "h")
        //        {
        //            ResponseContentsStatus_SW xml3 = new ResponseContentsStatus_SW();
        //            string strXml3 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><ResponseContentsStatus_SW><Head ID=\"0\"/><Body RespStatus=\"1\" VrMode=\"1\" ApplicationName=\"Nolimits2\" ContentsName=\"Ferrir\"/></ResponseContentsStatus_SW>";

        //            GeneralPacketWrapper packet3 = new GeneralPacketWrapper(1, strXml3);
        //            byte[] aData3 = packet3.Struct2Bytes();
        //            server.Send(aData3, packet3.FullSize, SocketFlags.None);
        //        }
        //        //data = new byte[1024];
        //        //recv = server.Receive(data);
        //        //stringData = Encoding.ASCII.GetString(data, 0, recv);
        //        //Console.WriteLine(stringData);
        //    }
        //    Console.WriteLine("Disconnecting from server...");
        //    server.Shutdown(SocketShutdown.Both);
        //    server.Close();
        //}

        static void Main(string[] args)
        {            
            AvSimDllMotionExternCsharp.MotionControl av = new AvSimDllMotionExternCsharp.MotionControl();
            
            AvSimDllMotionExternCsharp.EQUIPMENT_EXTEND_DATA pEQDataEx2 = null;
            AvSimDllMotionExternCsharp.EQUIPMENT_DATA pEQData2 = null;

            Console.WriteLine("Initial = {0}", av.Initial("192.168.1.1", 4001, AvSimDllMotionExternCsharp.eOS.PC_Soket));
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
            int nFailCont = 0;
            bool isResult = false;
            Console.WriteLine("State = {0}", av.State());

            try
            {
                for (int i = 0; i < 5000000; i++)
                {
                    if (!av.State())
                        Console.WriteLine("Initial = {0}", av.Initial("192.168.1.1", 4001, AvSimDllMotionExternCsharp.eOS.PC_Soket));

                    System.Threading.Thread.Sleep(50);
                    if (!(isResult = av.V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm(40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, out pEQDataEx2)))
                        nFailCont++;
                    Console.WriteLine("{0} = V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm = {1}", i, isResult);
                    System.Threading.Thread.Sleep(50);
                    if (!(isResult = av.V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm(1040, 1041, 1042, 1043, 1044, 1045, 1046, 1047, 1048, 1049, 1050, 1051, 1052, 1053, 1054, 1055, 1056, out pEQDataEx2)))
                        nFailCont++;
                    Console.WriteLine("{0} = V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm = {1}", i, isResult);
                    //System.Threading.Thread.Sleep(100);
                    //if (!(isResult = av.DOF_and_Blower(2000, 2001, 2002, 2003, 2004, 2005, 2006, 2007)))
                    //    nFailCont++;
                    //Console.WriteLine("{0} = DOF_and_Blower = {0}", i, isResult);
                    //System.Threading.Thread.Sleep(100);
                    //if (!(isResult = av.V2_DOF_and_Blower(1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007)))
                    //    nFailCont++;
                    //Console.WriteLine("{0} = V2_DOF_and_Blower = {0}", i, isResult);
                    //System.Threading.Thread.Sleep(100);
                    //if (!(isResult = av.V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis(20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, out pEQData2)))
                    //    nFailCont++;
                    //Console.WriteLine("{0} = V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis = {1}", i, isResult);

                    System.Threading.Thread.Sleep(1);
                    Console.WriteLine("Fail Count = {0}", nFailCont);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Destroy = {0}", av.Destroy());

            Console.ReadLine();
        }
    }
}
