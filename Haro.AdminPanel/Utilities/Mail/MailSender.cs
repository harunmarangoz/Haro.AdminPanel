using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Haro.AdminPanel.Models.Entities;
using Microsoft.EntityFrameworkCore.Internal;

namespace Haro.AdminPanel.Utilities.Mail
{
    public class MailSender
    {
        private string _smtpServer;
        private int _smtpPort;
        private string _fromAddress;
        private string _fromAddressTitle;
        private string _username;
        private string _password;
        private bool _enableSsl;
        private bool _useDefaultCredentials;

        public MailSender(SiteInformation siteInformation)
        {
            _smtpServer = siteInformation.SmtpServer;
            _smtpPort = siteInformation.SmtPort;
            _smtpPort = _smtpPort == 0 ? 25 : _smtpPort;
            _fromAddress = siteInformation.SmtpUserName;
            _fromAddressTitle = siteInformation.SmtpUserName;
            _username = siteInformation.SmtpUserName;
            _password = siteInformation.SmtpPassword;
            _enableSsl = siteInformation.SmtpEnableSsl;
            _useDefaultCredentials = true;
        }

        public async void Send(string toAddress, string subject, string body, bool sendAsync = false)
        {
            await Send(new List<string>() {toAddress}, subject, body, (sendAsync ? 1 : 0) != 0);
        }

        public async Task Send(
            List<string> toAddresses,
            string subject,
            string body,
            bool sendAsync = false,
            List<string> attachments = null)
        {
            var message = new MailMessage();
            foreach (var to in toAddresses)
            {
                message.To.Add(new MailAddress(to));
            }
            message.From = new MailAddress(_username);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            attachments?.ForEach(x => message.Attachments.Add(new Attachment(x)));

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = _username,
                    Password = _password
                };
                smtp.Credentials = credential;
                smtp.Host = _smtpServer;
                smtp.Port = _smtpPort;
                smtp.EnableSsl = _enableSsl;
                try
                {
                    smtp.Send(message);
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
        }
    }
}