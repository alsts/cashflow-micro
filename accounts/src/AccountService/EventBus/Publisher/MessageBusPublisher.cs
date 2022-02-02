using System;
using System.Text;
using System.Text.Json;
using AccountService.Dtos;
using AccountService.Util.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using RabbitMQ.Client;

namespace AccountService.EventBus.Publisher
{
    public class MessageBusPublisher : IMessageBusPublisher, IDisposable
    {
        private readonly IConfiguration configuration;
        private IConnection? connection;
        private IModel chanel;
        private readonly IWebHostEnvironment env;
        private readonly ILogger<MessageBusPublisher> logger;

        public MessageBusPublisher(
            IConfiguration configuration,
            IWebHostEnvironment env,
            ILogger<MessageBusPublisher> logger)
        {
            this.configuration = configuration;
            this.env = env;
            this.logger = logger;

            if (env.IsProduction())
            {
                var factory = new ConnectionFactory()
                {
                    HostName = configuration["RabbitMQSettings:Host"],
                    Port = int.Parse(configuration["RabbitMQSettings:Port"])
                };
                try
                {
                    Utils.TryConnecting<MySqlException>(5, 3,
                        () => {
                            connection = factory.CreateConnection();
                            chanel = connection.CreateModel();
                            chanel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                            connection.ConnectionShutdown += RabbitMqConnectionShutDown;
                            logger.LogInformation($"---> Connected to RabbitMQ");
                        },
                        retryCount => logger.LogInformation("---> Retrying to connect with RabbitMQ: " + retryCount),
                        () => logger.LogError("--->  Could not connect to RabbitMQ"));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"---> Could not connect to RabbitMQ: {e}");
                    throw;
                }
            }
        }

        public void PublishNewUser(UserPublishedDto userPublishedDto)
        {
            var message = JsonSerializer.Serialize(userPublishedDto);
            if (connection is { IsOpen: true })
            {
                Console.WriteLine("---> RabbitMQ connection open, sending message");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("---> RabbitMQ connection open, not sending message");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            chanel.BasicPublish(
                exchange: "trigger",
                routingKey: "",
                basicProperties: null,
                body: body);
            Console.WriteLine($"---> RabbitMQ message sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("---> RabbitMQ disposed");
            if (chanel.IsOpen)
            {
                chanel.Close();
                connection.Close();
            }
        }

        public void RabbitMqConnectionShutDown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"---> RabbitMQ connection shut down");
        }
    }
}
