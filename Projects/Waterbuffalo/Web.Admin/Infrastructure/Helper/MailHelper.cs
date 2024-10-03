using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.WebPages;
using Unity.Common.Configuration;


namespace Web.Admin.Infrastructure.Helper
{
    public static class MailHelper
    {
        /// <summary>
        /// Send Mail
        /// </summary>
        /// <param name="toEmail">Địa chỉ email sẽ gửi đến</param>
        /// <param name="subject">Tiêu đề email</param>
        /// <param name="content">Nội dung email</param>
        /// <param name="fromEmailDisplayName">Tên email từ người gửi</param>
        /// <returns></returns>
        //public static async Task<bool> SendMail(List<string> toEmail, string subject, string content, string fromEmailDisplayName)
        //{
        //    try
        //    {
        //        var host = AppSettings.SMTPHost;
        //        var port = AppSettings.SMTPPort;
        //        var fromEmail = AppSettings.FromEmailAddress;
        //        var fromEmailAlias = AppSettings.FromEmailAliasAddress.IsEmpty() ? fromEmail : AppSettings.FromEmailAliasAddress;
        //        var password = AppSettings.FromEmailPassword;
        //        var fromName = fromEmailDisplayName;

        //        var smtpClient = new SmtpClient(host, port)
        //        {
        //            //UseDefaultCredentials = false,
        //            Credentials = new System.Net.NetworkCredential(fromEmail, password),
        //            DeliveryMethod = SmtpDeliveryMethod.Network,
        //            EnableSsl = AppSettings.EnabledSSL,
        //            Timeout = 100000
        //        };

        //        var mail = new MailMessage
        //        {
        //            Body = content,
        //            Subject = subject,
        //            From = new MailAddress(fromEmailAlias, fromName)
        //        };
        //        foreach (var item in toEmail)
        //        {
        //            mail.Bcc.Add(new MailAddress(item));
        //        }
        //        mail.BodyEncoding = System.Text.Encoding.UTF8;
        //        mail.IsBodyHtml = true;
        //        mail.Priority = MailPriority.High;

        //        await smtpClient.SendMailAsync(mail);

        //        return true;
        //    }
        //    catch (SmtpException smex)
        //    {
        //        var ex = smex.Message;
        //        return false;
        //    }
        //}

        public static async Task<bool> AmazonSendMail(string toEmail, string subject, string content, string fromEmailDisplayName)
        {
            // Replace sender@example.com with your "From" address. 
            // This address must be verified with Amazon SES.
            String FROM = AppSettings.FromEmailAddress;
            String FROMNAME = fromEmailDisplayName;

            // Replace recipient@example.com with a "To" address. If your account 
            // is still in the sandbox, this address must be verified.
            String TO = toEmail;

            // Replace smtp_username with your Amazon SES SMTP user name.
            String SMTP_USERNAME = AppSettings.SMTPUsername;

            // Replace smtp_password with your Amazon SES SMTP user name.
            String SMTP_PASSWORD = AppSettings.SMTPPassword;

            // (Optional) the name of a configuration set to use for this message.
            // If you comment out this line, you also need to remove or comment out
            // the "X-SES-CONFIGURATION-SET" header below.
            //String CONFIGSET = "ConfigSet";

            // If you're using Amazon SES in a region other than US West (Oregon), 
            // replace email-smtp.us-west-2.amazonaws.com with the Amazon SES SMTP  
            // endpoint in the appropriate AWS Region.
            String HOST = AppSettings.SMTPHost;

            // The port you will connect to on the Amazon SES SMTP endpoint. We
            // are choosing port 587 because we will use STARTTLS to encrypt
            // the connection.
            int PORT = AppSettings.SMTPPort;

            // The subject line of the email
            String SUBJECT = subject;

            // The body of the email
            String BODY = content;

            // Create and build a new MailMessage object
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(FROM, FROMNAME);
            message.To.Add(new MailAddress(TO));
            message.Subject = SUBJECT;
            message.Body = BODY;
            // Comment or delete the next line if you are not using a configuration set
            //message.Headers.Add("X-SES-CONFIGURATION-SET", CONFIGSET);

            using (var client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                // Pass SMTP credentials
                client.Credentials =
                    new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

                // Time out
                client.Timeout = 10000;

                // Enable SSL encryption
                client.EnableSsl = AppSettings.EnabledSSL;

                // Try to send the message. Show status in console.
                try
                {
                    client.Send(message);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
