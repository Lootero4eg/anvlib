using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace anvlib.Network
{
    /// <summary>
    /// Вспомогательный класс, умеющий посылать электронную почту
    /// </summary>
    public static class MailNotifycator
    {
        public static void SendMail(string smtpServer, string DisplayName,
                string from, string password,
                string mailto, string caption, string message,
                bool IsImportant,
                bool IsSSLConnection, string attachFile = null)
        {
            try
            {
                MailMessage mail = new MailMessage();
                if (!string.IsNullOrEmpty(DisplayName))
                    mail.From = new MailAddress(from, DisplayName);
                else
                    mail.From = new MailAddress(from);
                if (mailto.IndexOf(';') > 0)
                {
                    string[] tmpst = mailto.Split(';');
                    foreach (var st in tmpst)
                        mail.To.Add(new MailAddress(st));
                }
                else
                    mail.To.Add(new MailAddress(mailto));
                mail.Subject = caption;
                mail.Body = message;                
                if (!string.IsNullOrEmpty(attachFile))
                    mail.Attachments.Add(new Attachment(attachFile));

                //--Приоритет письма!
                if (IsImportant)
                    mail.Priority = MailPriority.High;

                SmtpClient client = new SmtpClient();
                client.Host = smtpServer;
                if (IsSSLConnection)
                {
                    client.EnableSsl = true;
                    client.Port = 587;
                }
                else
                    client.EnableSsl = false;
                client.Credentials = new NetworkCredential(from.Split('@')[0], password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                
                client.Send(mail);
                mail.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
        }
    }
}
