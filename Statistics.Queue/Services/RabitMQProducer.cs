using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Statistics.Queue.Interfaces;

namespace Statistics.Queue.Services
{
    public class RabitMQProducer : IMessageProducer
    {
        public void SendProductMessage<T>(T message)
        {

            var factory = new ConnectionFactory() { HostName = "my-rabbit", Port = 5672 };
            factory.UserName = "default";
            factory.Password = "12345678";

            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare("q.StatisticsOfCall",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "q.StatisticsOfCall", body: body);
        }
    }
}
