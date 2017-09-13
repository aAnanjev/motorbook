using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;
using System.Net.Mime;

namespace CarFixed.Core
{
    public class EmailSender
    {
        #region Properties

        public string Username { get; set; }
        public string Password { get; set; }

        public string SmtpServer { get; set; }

        #endregion Properties

        #region Construction

        public EmailSender()
        {
            this.SmtpServer = ConfigurationManager.AppSettings["EmailSmtpServer"];
            this.Username = ConfigurationManager.AppSettings["EMailUsername"];
            this.Password = ConfigurationManager.AppSettings["EMailPassword"];
        }

        public EmailSender(string smtpServer, string username, string password)
        {
            this.SmtpServer = smtpServer;
            this.Username = username;
            this.Password = password;
        }

        #endregion Construction

        #region Public Methods

        public void SendMail(
            string to,
            string fromEmail,
            string subject,
            string body,
            bool isHtml,
            string fromName = null,
            string bcc = null,
            Dictionary<string, string> textSubs = null,
            List<DateTime> sendSchedule = null)
        {
            try
            {
                MailMessage mailMsg = new MailMessage();
                string html = null;
                // To
                //mailMsg.To.Add(to.ToString());
                mailMsg.To.Add(to);

                if (!String.IsNullOrEmpty(bcc))
                    mailMsg.Bcc.Add(bcc);

                if (fromName == null)
                    mailMsg.From = new MailAddress(fromEmail);
                else
                    mailMsg.From = new MailAddress(fromEmail, fromName);

                if (textSubs != null)
                {
                    foreach (KeyValuePair<string, string> kvp in textSubs)
                        body = body.Replace(kvp.Key, kvp.Value);
                }

                //System.IO.File.ReadAllText();

                mailMsg.Subject = subject;
                //mailMsg.IsBodyHtml = isHtml;
                if (isHtml)
                    html = body;

                //html = @"<div> Dear" + toName + "</div><br /><div><p>" + body + "</p></div>";
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                // Init SmtpClient and send
                //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(25));
                SmtpClient smtpClient = new SmtpClient(this.SmtpServer);
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(this.Username, this.Password);
                smtpClient.Credentials = credentials;
                //smtpClient.EnableSsl = true;
                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        #endregion Public Methods
    }
}
