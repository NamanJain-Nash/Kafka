using BuisnessLayer.IRepository;
using DatabaseLayer.Data;
using Microsoft.Extensions.Logging;
using Services.IServices;
using System;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BuisnessLayer.Repository
{
    public class ProducerLogic : IProducerLogic
    {
        private readonly IKafkaProducer kafkaProducer;
        private readonly ILogger<ProducerLogic> logger;

        public ProducerLogic(IKafkaProducer kafkaProducer, ILogger<ProducerLogic> logger)
        {
            this.kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> AddProducerAsync(Producer producer, string Topic)
        {
            try
            {
                var (validEmails, invalidEmails) = CorrectEmailAsync(producer.to);
                if(validEmails != null)
                {
                    return "No Email is Correct";
                }
                producer.to = validEmails;
                var serializedProducer = JsonSerializer.Serialize(producer);

                // Encoding
                string encodedMessage = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(serializedProducer));

                // For example, produce a message using KafkaProducer
                string result = await kafkaProducer.ProduceAsync(encodedMessage, Topic);

                // Return a success message or any necessary result
                var missedEmails = string.Join("\n", invalidEmails);
                return $"{result} Some Missed emails are: \n{missedEmails}";
            }
            catch (Exception ex)
            {
                // Log any exceptions
                kafkaProducer.Dispose();
                logger.LogError(ex, "Error occurred while adding a producer.");
                return $"Error : {ex.Message}";
            }
        }
        private (List<string>, List<string>) CorrectEmailAsync(List<string> mails)
        {
            const string emailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
            var validEmails = new List<string>();
            var invalidEmails = new List<string>();

            foreach (var mail in mails)
            {
                if (!Regex.IsMatch(mail, emailRegex))
                {
                    invalidEmails.Add(mail);
                }
                else
                {
                    validEmails.Add(mail);
                }
            }

            return (validEmails, invalidEmails);
        }
    }
}
