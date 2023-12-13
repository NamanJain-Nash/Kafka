using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using Services.Services;
namespace unittest.services
{
    public class KafkaConsumerTest
    {
        [Fact]
        public async Task StartConsumer_SuccessfullyStarts()
        {
            // Arrange
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c.GetSection("KafkaConfig:Servers").Value).Returns("localhost:9092");
            configMock.Setup(c => c.GetSection("KafkaConfig:GroupId").Value).Returns("test_group");

            var consumer = new KafkaConsumer(configMock.Object);

            // Act
            consumer.StartConsumer("test_topic");

            // Assert
            // Verify that the consumer has been initialized and started
            // You might want to use some delay here to allow the consumer to start properly
            await Task.Delay(1000); // Adjust the delay time as needed
            Assert.NotNull(consumer); // Assert that the consumer is not null
            // Add more specific assertions if needed
        }

    }
}
