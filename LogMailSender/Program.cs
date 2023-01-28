using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;

namespace LogMailSender
{
    class Program
    {
        static void Main(string[] args)
        {
            if (null == args || 1 > args.Length)
            {
                Console.WriteLine("Run the batch file");                
            }
            else if (1 == args.Length && "/?" == args[0])
            {
                Console.WriteLine("LogFilePath MailAddress Password");
            }
            else if (3 == args.Length)
            {
                IPHostEntry iphe = Dns.GetHostEntry(Dns.GetHostName());
                string strMachine = string.Empty;
                foreach (IPAddress ip in iphe.AddressList)
                {
                    if ("192.168.10.10" == ip.ToString())
                        strMachine = "NhaTrang_HelloIgloo_GreateWall";
                    else if ("192.168.10.20" == ip.ToString())
                        strMachine = "NhaTrang_HelloIgloo_Rollercoaster";
                    else
                        strMachine = "Unknown";
                }

                DirectoryInfo info = new DirectoryInfo(args[0]);
                FileInfo[] aFiles = info.GetFiles("*.log");
                if (null != aFiles)
                {
                    AKI.Util.MailUtil mail = new AKI.Util.MailUtil();
                    mail.CredentialsUserName = args[1];
                    mail.CredentialsPassword = args[2];
                    for (int i = 0; i < aFiles.Length; i++)
                    {
                        if (".log" == aFiles[i].Extension)
                        {
                            string strDate = DateTime.Now.ToString("yyyyMMdd") + ".log";
                            if (strDate == aFiles[i].Name)
                                continue;

                            string fileName = args[0] + "\\" + aFiles[i].Name + ".txt";
                            aFiles[i].CopyTo(fileName, true);
                            aFiles[i].Delete();

                            string strResult = mail.SendMail(string.Format("{0}_{1}", strMachine, aFiles[i].Name), aFiles[i].FullName, fileName);
                            if (string.Empty == strResult)
                            {
                                Console.WriteLine("Send Success = {0}", aFiles[i].Name);
                                //string fileName = args[0] + "\\" + aFiles[i].Name + "." + "txt";
                                //aFiles[i].CopyTo(fileName);
                                //aFiles[i].Delete();

                            }
                            else
                                Console.WriteLine("Send Fail = {0}", aFiles[i].Name);
                        }
                    }
                }
            }
            else if (3 < args.Length)
            {
                DirectoryInfo info = new DirectoryInfo(args[0]);
                FileInfo[] aFiles = info.GetFiles("*.log");
                if (null != aFiles)
                {
                    AKI.Util.MailUtil mail = new AKI.Util.MailUtil();
                    mail.CredentialsUserName = args[1];
                    mail.CredentialsPassword = args[2];
                    for (int i = 0; i < aFiles.Length; i++)
                    {
                        if (".log" == aFiles[i].Extension)
                        {
                            string strDate = DateTime.Now.ToString("yyyyMMdd") + ".log";
                            if (strDate == aFiles[i].Name)
                                continue;

                            string fileName = args[0] + "\\" + aFiles[i].Name + ".txt";
                            aFiles[i].CopyTo(fileName, true);
                            aFiles[i].Delete();

                            string strResult = mail.SendMail(string.Format("{0}_{1}", args[3], aFiles[i].Name), aFiles[i].FullName, fileName);
                            if (string.Empty == strResult)
                            {
                                Console.WriteLine("Send Success = {0}", aFiles[i].Name);
                                //string fileName = args[0] + "\\" + aFiles[i].Name + "." + "txt";
                                //aFiles[i].CopyTo(fileName);
                                //aFiles[i].Delete();

                            }
                            else
                                Console.WriteLine("Send Fail = {0}", aFiles[i].Name);
                        }
                    }
                }
            }
            Console.WriteLine("Complete");
            //Console.ReadLine();

            //System.Diagnostics.ProcessStartInfo stProcessStartInfo = new System.Diagnostics.ProcessStartInfo("LogMailSender.exe");
            ////stProcessStartInfo.WorkingDirectory = WebCommunication.g_log.LogFilePath;

            //stProcessStartInfo.CreateNoWindow = false;
            //stProcessStartInfo.RedirectStandardOutput = true;
            //stProcessStartInfo.UseShellExecute = false;
            //stProcessStartInfo.FileName = "LogMailSender.exe";
            //stProcessStartInfo.Arguments = string.Format("{0} {1} {2}", WebCommunication.g_log.LogFilePath, UserData.MAIL, "p&ipassword");
            //System.Diagnostics.Process pi = System.Diagnostics.Process.Start(stProcessStartInfo);

        }
    }
}
