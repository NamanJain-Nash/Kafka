using AutoMapper;
using BuisnessLayer.IRepository;
using DatabaseLayer.DTOs;
using DatabaseLayer.Data;
using Microsoft.Extensions.Logging;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.Mapper;
using System.Text.Json;

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
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProducerMappingProfile>(); // Use the defined mapping profile
            });
            this.mapper = mapper ?? config.CreateMapper();
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<string> AddProducerAsync(ProducerDTO producerDTO,string Topic)
        {
            try
            {
                // Mapping DTO to actual Producer entity
                Producer producer = mapper.Map<Producer>(producerDTO);
                var serializedProducer = JsonSerializer.Serialize(producer);

                // Encoding
                string encodedMessage = Encoding.UTF8.GetBytes(serializedProducer);

                // For example, produce a message using KafkaProducer
                string result =await kafkaProducer.ProduceAsync(encodedMessage, Topic);

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
