using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Services.IServices;

namespace Services.Services;

public class KafkaProducer:IKafkaProducer
{

    private readonly IConfiguration _configuration;
    private readonly IProducer<string, string> _producer;

    public KafkaProducer(IConfiguration configuration)
    {
        _configuration = configuration;

        // Fetch Kafka configuration values from appsettings.json
        string bootstrapServers = _configuration.GetSection("KafkaConfig:Servers").Value;
        

        var config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }
    public async Task<string> ProduceAsync(string message,string topicName)
    {
        try
        {

            var kafkaMessage = new Message<string, string>
            {
                Key = new Guid().ToString(),
                Value = message
            };

            var deliveryReport = await _producer.ProduceAsync(topicName, kafkaMessage);

            return $"Delivered message to: {deliveryReport.TopicPartitionOffset}";
        }
        catch (ProduceException<string, string> ex)
        {
           return $"Delivery failed: {ex.Error.Reason}";
        }
    }

    public void Dispose()
    {
        _producer?.Dispose();
    }

}