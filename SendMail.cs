using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BagNotify
{
    class SendMail
    {
        public static void sendEmail(string sentFrom, string fromName, string emailTo, string subject, string htmlBody,
                                      string smtp, string smtpUserName, string smtpPassword)
        {
            // Configure the client:
            var client = new System.Net.Mail.SmtpClient(smtp);
            var credentials = new System.Net.NetworkCredential(smtpUserName, smtpPassword);

            //client.Port = 587;
            client.Port = 465;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = credentials;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(sentFrom, fromName);
            mailMessage.To.Add(emailTo);
            mailMessage.Body = htmlBody;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = subject;
            client.Send(mailMessage);
        }
    }
}
