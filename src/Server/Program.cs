using Newtonsoft.Json;
using RabbitMQ.Client;
using Shared;
using System;
using System.Text;

namespace Server
{
    public class Program
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "serverip", UserName = "username", Password = "password" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "alaska", type: "direct");

                    var helloMessage = new HelloMessage { Message = "Hello from Texas!" };
                    var messageJsonString = JsonConvert.SerializeObject(helloMessage);
                    var messageBytes = Encoding.UTF8.GetBytes(messageJsonString);

                    channel.BasicPublish(exchange: "alaska",
                                         routingKey: "",
                                         basicProperties: null,
                                         body: messageBytes);

                    Console.WriteLine($"Sent message: {helloMessage.Message}");
                }
            }

            Console.WriteLine("Press Ctrl+C to exit.");
            Console.ReadLine();
        }
    }
}