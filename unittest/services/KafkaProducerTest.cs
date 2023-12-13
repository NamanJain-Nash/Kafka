using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Moq;
using Services.IServices;
using Services.Services;
using Xunit;

namespace unittest.services
{
    public class KafkaProducerTests
    {
        [Fact]
        public async Task ProduceAsync_Success()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c.GetSection("KafkaConfig:Servers").Value).Returns("localhost:9092");

            var producer = new KafkaProducer(mockConfiguration.Object);
            var message = "Test message";
            var topicName = "test_topic";

            // Act
            var result = await producer.ProduceAsync(message, topicName);
            // Assert
            Assert.Equal($"Delivered message to: test_topic", result);
        }


        [Fact]
        public async Task ProduceAsync_Failure()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(conf => conf.GetSection("KafkaConfig:Servers").Value).Returns("localhost:9092");
            var kafkaProducer = new KafkaProducer(configurationMock.Object);
            var message = "Test message";
            var topicName = "test_topic";

            // Act
            var result = await kafkaProducer.ProduceAsync(message, topicName);

            // Assert
            Assert.Contains("Delivery failed", result);
        }

        [Fact]
        public void Dispose_Called()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(conf => conf.GetSection("KafkaConfig:Servers").Value).Returns("localhost:9092");

            var producerMock = new Mock<IProducer<string, string>>();

            var kafkaProducer = new KafkaProducer(configurationMock.Object);
            kafkaProducer.GetType().GetProperty("_producer").SetValue(kafkaProducer, producerMock.Object);

            // Act
            kafkaProducer.Dispose();

            // Assert
            producerMock.Verify(p => p.Dispose(), Times.Once);
        }
    }
}
