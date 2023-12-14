using BuisnessLayer.IRepository;
using BuisnessLayer.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.IServices;
using Services.Services;
using System;
using System.IO;

internal class Program
{
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddEnvironmentVariables(prefix: "PREFIX_");
                    config.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IKafkaConsumer, KafkaConsumer>();
                    services.AddSingleton<IConsumerLogic, ConsumerLogic>();
                    services.AddSingleton<ISendMail,SendMail>();
                    services.AddSingleton(provider =>
                    {
                        IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
                        return configuration.GetSection("KafkaConfig");
                    });
                })
                .Build();

            var kafkaService = host.Services.GetRequiredService<IKafkaConsumer>();
        kafkaService.StartConsumer("first-topic"); // Replace 'your_topic_name' with your Kafka topic

        kafkaService.MessageReceived += (sender, message) =>
            {
                Console.WriteLine($"Received message: {message}");
                // Process the received message as needed
            };

            kafkaService.ErrorOccurred += (sender, error) =>
            {
                Console.WriteLine($"Error occurred: {error}");
                // Handle the error as needed
            };

            // To stop the consumer after a certain period or condition, add your logic here
            // e.g., Wait for user input to stop
            Console.WriteLine("Press 'Q' to stop the consumer.");
            while (Console.ReadKey().Key != ConsoleKey.Q) { }

            kafkaService.StopConsumer();
        }
}