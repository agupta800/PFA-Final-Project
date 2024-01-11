using Microsoft.AspNetCore.DataProtection;
using PFA.Repository.Interface;
using PFA.ViewModel.Email;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace PFA.Repository.Service
{

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;

        public EmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<bool> EmailSenderAsync(string email, string Subject, string message)
        {
            bool status = false;
            try
            {
                GetEmailSetting getEmailSetting = new GetEmailSetting()
                {
                   SecretKey  = configuration.GetValue<string>("AppSettings:SecretKey"),
                        From = configuration.GetValue<string>("AppSettings:EmailSettings:From"),
                        SmtpServer = configuration.GetValue<string>("AppSettings:EmailSettings:SmtpServer"),
                        Port = configuration.GetValue<int>("AppSettings:EmailSettings:Port"),
                        EnableSSl = configuration.GetValue<bool>("AppSettings:EmailSettings:EnableSSl"),
                };
                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(getEmailSetting.From),
                    Subject = Subject,
                    Body = message


                };
                mailMessage.To.Add(email);
                SmtpClient smtpClient = new SmtpClient(getEmailSetting.SmtpServer)
                {
                    Port = getEmailSetting.Port,
                    Credentials = new NetworkCredential(getEmailSetting.From, getEmailSetting.SecretKey),
                    EnableSsl = getEmailSetting.EnableSSl
                };
                await smtpClient.SendMailAsync(mailMessage);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
    }
}
