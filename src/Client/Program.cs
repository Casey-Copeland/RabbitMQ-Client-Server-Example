using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Client
{
    public class Program
    {
        public static void Main()
        {
            var connectionFactory = new ConnectionFactory() { HostName = "10.5.5.224" };

            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "alaska", type: "direct");

                    var queueName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queue: queueName,
                                        exchange: "alaska",
                                        routingKey: "");

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (sender, eventArgs) =>
                    {
                        var messageBytes = eventArgs.Body;
                        var message = Encoding.UTF8.GetString(messageBytes);

                        Console.WriteLine($"Received message: {message}");

                        channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);
                    };

                    channel.BasicConsume(queue: queueName,
                                         autoAck: false,
                                         consumer: consumer);

                    Console.WriteLine("Press Ctrl+C to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}