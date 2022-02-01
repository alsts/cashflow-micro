using System;
using System.Text;
using System.Text.Json;
using AccountService.Dtos;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace AccountService.EventBus.Publisher
{
    public class MessageBusPublisher : IMessageBusPublisher
    {
        private readonly IConfiguration configuration;
        private readonly IConnection connection;
        private readonly IModel chanel;

        public MessageBusPublisher(IConfiguration configuration)
        {
            this.configuration = configuration;

            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQSettings:Host"],
                Port = int.Parse(configuration["RabbitMQSettings:Port"])
            };

            try
            {
                connection = factory.CreateConnection();
                chanel = connection.CreateModel();
                chanel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                connection.ConnectionShutdown += RabbitMqConnectionShutDown;
                Console.WriteLine($"---> Connected to RabbitMQ");
            }
            catch (Exception e)
            {
                Console.WriteLine($"---> Could not connect to RabbitMQ: {e}");
                throw;
            }
        }

        public void PublishNewUser(UserPublishedDto userPublishedDto)
        {
            var message = JsonSerializer.Serialize(userPublishedDto);
            if (connection.IsOpen)
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
                exchange:"trigger", 
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
