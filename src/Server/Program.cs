using RabbitMQ.Client;
using System;
using System.Text;

namespace Server
{
    public class Program
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "10.5.5.224" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "alaska", type: "direct");

                    string message = "Hello from Texas!";
                    var messageBytes = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "alaska",
                                         routingKey: "",
                                         basicProperties: null,
                                         body: messageBytes);

                    Console.WriteLine($"Sent message: {message}");
                }
            }

            Console.WriteLine("Press Ctrl+C to exit.");
            Console.ReadLine();
        }
    }
}