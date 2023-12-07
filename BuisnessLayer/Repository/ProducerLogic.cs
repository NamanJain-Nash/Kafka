using BuisnessLayer.IRepository;
using Microsoft.Extensions.Logging;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace BuisnessLayer.Repository
{
    public class ProducerLogic:IProducerLogic
    {
        private readonly IKafkaProducer kafkaProducer;
        private readonly IMapper mapper;
        private readonly ILogger<ProducerLogic> logger;

        public ProducerLogic(IKafkaProducer kafkaProducer, IMapper mapper, ILogger<ProducerLogic> logger)
        {
            this.kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<string> AddProducerAsync(ProducerDTO producerDTO,string Topic)
        {
            try
            {
                // Mapping DTO to actual Producer entity
                var producer = mapper.Map<Producer>(producerDTO);

                // Logic to process the Producer entity and perform necessary actions

                // For example, produce a message using KafkaProducer
                string result=await kafkaProducer.ProduceAsync(producer, Topic);

                // Return a success message or any necessary result
                return result;
            }
            catch (Exception ex)
            {
                // Log any exceptions
                logger.LogError(ex, "Error occurred while adding a producer.");
                throw; // You might want to handle or transform exceptions based on your requirements.
            }
        }

    }
}
