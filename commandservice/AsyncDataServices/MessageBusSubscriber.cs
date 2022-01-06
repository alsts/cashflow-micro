using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandsService.EventProcessing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandsService.AsyncDataServices
{
    // Background service
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly IEventProcessor eventProcessor;
        private IConnection connection;
        private IModel chanel;
        private string queueName;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            this.configuration = configuration;
            this.eventProcessor = eventProcessor;
            
            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQHost"],
                Port = int.Parse(configuration["RabbitMQPort"])
            };

            try
            {
                connection = factory.CreateConnection();
                chanel = connection.CreateModel();
                chanel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                queueName = chanel.QueueDeclare().QueueName;
                chanel.QueueBind(
                    queue: queueName,
                    exchange: "trigger",
                    routingKey: "");
                
                Console.WriteLine($"---> Listening to RabbitMQ");
                connection.ConnectionShutdown += RabbitMqConnectionShutDown;
            }
            catch (Exception e)
            {
                Console.WriteLine($"---> Could not connect to RabbitMQ: {e}");
            }
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(chanel);
            consumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("---> Event received");
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                eventProcessor.ProcessEvent(notificationMessage);
            };
            chanel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }
        
        public override void Dispose()
        {
            Console.WriteLine("---> RabbitMQ disposed");
            if (chanel.IsOpen)
            {
                chanel.Close();
                connection.Close();
            }

            base.Dispose();
        }

        public void RabbitMqConnectionShutDown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"---> RabbitMQ connection shut down");
        }
    }
}
