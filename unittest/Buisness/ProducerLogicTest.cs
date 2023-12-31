using System;
using System.Threading.Tasks;
using BuisnessLayer.Repository;
using DatabaseLayer.Data;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Services.IServices;


namespace unittest.Buisness
{
    public class ProducerLogicTests
    {
        private ProducerLogic _producerLogic;
        private Mock<IKafkaProducer> _mockKafkaProducer;
        private Mock<ILogger<ProducerLogic>> _mockLogger;

        public ProducerLogicTests()
        {
            _mockKafkaProducer = new Mock<IKafkaProducer>();
            _mockLogger = new Mock<ILogger<ProducerLogic>>();

            _producerLogic = new ProducerLogic(_mockKafkaProducer.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task AddProducerAsync_SuccessfullyAddsProducer()
        {
            // Arrange
            var producer = new Producer(); // Provide necessary data for producer
            string topic = "TestTopic";
            string expectedResult = "Success";

            // Mock the KafkaProducer's ProduceAsync method to return a success message
            _mockKafkaProducer.Setup(k => k.ProduceAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _producerLogic.AddProducerAsync(producer, topic);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockKafkaProducer.Verify(k => k.ProduceAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockKafkaProducer.VerifyNoOtherCalls();
            _mockLogger.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddProducerAsync_ExceptionOccurs_LogsErrorAndReturnsErrorMessage()
        {
            // Arrange
            var producer = new Producer(); // Provide necessary data for producer
            string topic = "TestTopic";
            var expectedErrorMessage = "Simulated error message";
            var expectedException = new Exception(expectedErrorMessage);

            // Mock the KafkaProducer's ProduceAsync method to throw an exception
            _mockKafkaProducer.Setup(k => k.ProduceAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(expectedException);

            // Act
            var result = await _producerLogic.AddProducerAsync(producer, topic);

            // Assert
            Assert.Equal($"Error : {expectedErrorMessage}", result);
            _mockKafkaProducer.Verify(k => k.ProduceAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockKafkaProducer.VerifyNoOtherCalls();
            _mockLogger.Verify(
                x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()),
                Times.Once);
            _mockLogger.VerifyNoOtherCalls();
        }
    }
}
