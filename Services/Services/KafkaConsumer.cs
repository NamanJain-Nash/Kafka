using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public  class KafkaConsumer : IKafkaConsumer
    {
        private readonly IConfiguration _configuration;
        private IConsumer<Ignore, string> _consumer;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private Task _consumeTask;

        public event EventHandler<string> MessageReceived;
        public event EventHandler<string> ErrorOccurred;

        public KafkaConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void StartConsumer(string topicName)
        {
            string server = _configuration.GetSection("KafkaConfig:Servers").Value?? "localhost:9092";
            string groupId= _configuration.GetSection("KafkaConfig:GroupId").Value?? "test_group";
            var config = new ConsumerConfig
            {
                BootstrapServers = server,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            _consumer.Subscribe(topicName);

            _consumeTask = Task.Run(async () => await ConsumeMessages());
        }

        private async Task ConsumeMessages()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(_cancellationTokenSource.Token);
                    var message = consumeResult.Message.Value;

                    MessageReceived?.Invoke(this, message); // Send the message to the user

                    // Process the message...

                    // For illustration, here we're assuming the message is processed,
                    // and if a specific condition is met (e.g., an error), the consumer will stop.
                    if (message == "stop")
                    {
                        StopConsumer();
                        return;
                    }
                }
                catch (OperationCanceledException)
                {
                    // Cancellation requested, exit loop
                    break;
                }
                catch (ConsumeException e)
                {
                    ErrorOccurred?.Invoke(this, e.Error.Reason); // Notify about the error
                    StopConsumer(); // Close the connection on error
                }
            }

            _consumer.Close();
        }

        public void StopConsumer()
        {
            _cancellationTokenSource.Cancel();
            _consumeTask?.Wait(); // Ensure the consuming task is completed before closing the consumer
            _consumer?.Close();
        }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
            _consumer?.Dispose();
        }
    }
}
