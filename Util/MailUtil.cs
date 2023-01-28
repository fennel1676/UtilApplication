using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace AKI.Util
{
    public class MailUtil
    {
        private string m_strHost = "smtp.gmail.com";
        private int m_nPort = 587;

        private string m_strCredentialsUserName;
        private string m_strCredentialsPassword;
        private bool m_isUseDefaultCredentials = true;
        private bool m_isEnableSsl = true;
        private List<string> m_listTo = new List<string>();
        private List<string> m_listCc = new List<string>();
        private List<string> m_listBcc = new List<string>();

        public string Host { get { return m_strHost; } set { m_strHost = value; } }
        public int Port { get { return m_nPort; } set { m_nPort = value; } }
        public string CredentialsUserName { set { m_strCredentialsUserName = value; } }
        public string CredentialsPassword { set { m_strCredentialsPassword = value; } }
        public List<string> TO { get { return m_listTo; } set { m_listTo = value; } }
        public List<string> CC { get { return m_listCc; } set { m_listCc = value; } }
        public List<string> BCC { get { return m_listBcc; } set { m_listBcc = value; } }
        public bool UseDefaultCredentials { get { return m_isUseDefaultCredentials; } set { m_isUseDefaultCredentials = value; } }
        public bool EnableSsl { get { return m_isEnableSsl; } set { m_isEnableSsl = value; } }

        public string SendMail(string strFrom, string strTo, string strTitle, string strBody, string strAttachFile)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            string msg = string.Empty;
            try
            {
                message.From = new MailAddress(strFrom);
                bool isTo = false;
                if (null != m_listTo && 0 < m_listTo.Count)
                {
                    foreach (string temp in m_listTo)
                    {
                        if (temp == strTo)
                            isTo = true;

                        message.To.Add(temp);
                    }

                    if (!isTo)
                        message.To.Add(strTo);
                }
                else
                    message.To.Add(strTo);

                if (null != m_listCc && 0 < m_listCc.Count)
                {
                    foreach (string temp in m_listCc)
                    {
                        message.CC.Add(temp);
                    }
                }

                if (null != m_listBcc && 0 < m_listBcc.Count)
                {
                    foreach (string temp in m_listBcc)
                    {
                        message.Bcc.Add(temp);
                    }
                }
                
                message.Subject = strTitle;
                message.IsBodyHtml = true;
                message.Body = strBody;

                if (null != strAttachFile && string.Empty != strAttachFile && File.Exists(strAttachFile))
                {
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(strAttachFile);
                    message.Attachments.Add(attachment);
                }

                smtpClient.Host = m_strHost;
                smtpClient.Port = m_nPort;
                smtpClient.EnableSsl = m_isEnableSsl;
                smtpClient.UseDefaultCredentials = m_isUseDefaultCredentials;
                smtpClient.Credentials = new System.Net.NetworkCredential(m_strCredentialsUserName, m_strCredentialsPassword);
                smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                

                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        public string SendMail(string strTo, string strTitle, string strBody, string strAttachFile)
        {
            return SendMail(m_strCredentialsUserName, strTo, strTitle, strBody, strAttachFile);
        }

        public string SendMail(string strTitle, string strBody, string strAttachFile)
        {
            return SendMail(m_strCredentialsUserName, m_strCredentialsUserName, strTitle, strBody, strAttachFile);
        }
    }
}
