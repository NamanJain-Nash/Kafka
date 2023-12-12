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
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(conf => conf.GetSection("KafkaConfig:Servers").Value).Returns("localhost:9092");

            var producerMock = new Mock<IProducer<string, string>>();
            producerMock
                .Setup(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<string, string>>()))
                .ReturnsAsync(new DeliveryResult<string, string>
                {
                    TopicPartitionOffset = new TopicPartitionOffset("topic", new Partition(0), Offset.End)
                });

            var kafkaProducer = new KafkaProducer(configurationMock.Object);
            var message = "Test message";
            var topicName = "test_topic";

            // Act
            var result = await kafkaProducer.ProduceAsync(message, topicName);

            // Assert
            Assert.Contains("Delivered message to", result);
        }

        [Fact]
        public async Task ProduceAsync_Failure()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(conf => conf.GetSection("KafkaConfig:Servers").Value).Returns("localhost:9092");

            var producerMock = new Mock<IProducer<string, string>>();
            producerMock
                .Setup(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<string, string>>()))
                .ThrowsAsync(new ProduceException<string, string>(new Error(ErrorCode.Local_Failure), new Message<string, string>()));

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
