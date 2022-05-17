using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using GemCare.API.Interfaces;
using GemCare.API.Common;

namespace GemCare.API.Services
{
    public class EmailService : IEmailService
    {
        private const string smtpConfigName = "SmtpConfiguration";
        private string smtpServer;
        private int smtpPort;
        private string password;
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            InitSettings();
        }
        private void InitSettings()
        {
            smtpServer = _configuration.GetSection(smtpConfigName).GetChildren().FirstOrDefault(x => x.Key == "SmtpServer").Value;
            smtpPort = int.Parse(_configuration.GetSection(smtpConfigName).GetChildren().FirstOrDefault(x => x.Key == "SmtpPort").Value);
            password = _configuration.GetSection(smtpConfigName).GetChildren().FirstOrDefault(x => x.Key == "Password").Value;
        }

        public bool SendLoginCode(string toEmail, string EmailCode)
        {
            var builder = new StringBuilder();
            bool isEmailSent = false;
            try
            {
                using (var reader = File.OpenText("./Templates/Emails/LoginEmailTemplate.html"))
                {
                    builder.Append(reader.ReadToEnd());
                }
                builder.Replace("[OTP_CODE]", EmailCode);
                var body = builder.ToString();
                using var mailMessage = new MailMessage();
                using var client = new SmtpClient(smtpServer, smtpPort)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(AppConstants.SUPPORT_EMAIL, password),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 20000
                };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                mailMessage.From = new MailAddress(AppConstants.SUPPORT_EMAIL);
                mailMessage.To.Insert(0, new MailAddress(toEmail));
                mailMessage.Subject = "Reset Password";
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                //client.SendMailAsync(mailMessage);
                client.Send(mailMessage);
                isEmailSent = true;
            }
            catch
            {
                throw;
            }

            return isEmailSent;
        }
    }
}
