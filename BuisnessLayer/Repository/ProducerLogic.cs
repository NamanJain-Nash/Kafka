using BuisnessLayer.IRepository;
using DatabaseLayer.Data;
using Microsoft.Extensions.Logging;
using Services.IServices;
using System;
using System.Text;
using System.Text.Json;
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
                var serializedProducer = JsonSerializer.Serialize(producer);

                // Encoding
                string encodedMessage = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(serializedProducer));

                // For example, produce a message using KafkaProducer
                string result = await kafkaProducer.ProduceAsync(encodedMessage, Topic);

                // Return a success message or any necessary result
                return result;
            }
            catch (Exception ex)
            {
                // Log any exceptions
                kafkaProducer.Dispose();
                logger.LogError(ex, "Error occurred while adding a producer.");
                return $"Error : {ex.Message}";
            }
        }
    }
}
