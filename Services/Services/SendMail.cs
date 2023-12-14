using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.IServices;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using Microsoft.Extensions.Configuration;
using Confluent.Kafka;
using System.Net.Mail;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Services.Services
{
    public class SendMail : ISendMail
    {
        private readonly IConfiguration _config;
        private readonly ILogger<SendMail> _logger;

        public SendMail(IConfiguration configuration,ILogger<SendMail> logger)
        {
            _config = configuration;
            _logger = logger;
        }
        public async Task<string> SendGraphAPI(List<string> toEmail, string subject, string body)
        {
            try
            {
                string? tenantId = _config["tenantId"];
                string? clientId = _config["clientId"];
                string? clientSecret = _config["clientSecret"];
                ClientSecretCredential credential = new(tenantId, clientId, clientSecret);
                GraphServiceClient graphClient = new(credential);
                //Making of Recipent
                List<Recipient> recipients = new List<Recipient>();
                foreach (var email in toEmail)
                {
                    recipients.Add(new Recipient
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = email,
                        },
                    });

                }
                var requestBody = new SendMailPostRequestBody
                {
                    Message = new Message
                    {
                        Subject = subject,
                        Body = new ItemBody
                        {
                            ContentType = BodyType.Text,
                            Content = body,
                        },
                        ToRecipients = recipients
                    },
                };
                await graphClient.Users["naman.jain@nashtechglobal.com"].SendMail.PostAsync(requestBody);
                _logger.LogInformation("Email sent successfully to", toEmail);
                return "Mail Send";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {toEmail}", toEmail);
                return "Mail Failed";
            }
        }
        public async Task<string> SendEmailSMTP(List<string> toEmails, string subject, string body)
        {
            try
            {
                // Sender's email credentials
                string senderEmail = _config["senderId"];
                string password = _config["smtppassword"];

                // SMTP client configuration for Gmail
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, password);

                // Creating and sending the email message
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(senderEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                // Adding TO recipients
                foreach (var email in toEmails)
                {
                    mailMessage.To.Add(email);
                }

                client.Send(mailMessage);

                Console.WriteLine("Email sent successfully!");
                return "Sent";
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Failed to send email. Error message: {ex.Message}");
                return ex.Message;
            }
        }

    }
}


