using Newtonsoft.Json;
using Producor_Web_API.Interface;
using RabbitMQ.Client;
using System.Text;

namespace Producor_Web_API.Service
{
    

    public class RabbitMQService : IRabitMQProducer
    {
        public void SendProductMessage<T>(T message)
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            var rabbitMQConfig = configuration.GetSection("RabbitMQ");
            var factory = new ConnectionFactory
            {
                HostName = rabbitMQConfig["Host"],
                Port = int.Parse(rabbitMQConfig["Port"]),
                UserName = rabbitMQConfig["UserName"],
                Password = rabbitMQConfig["Password"],
                VirtualHost = rabbitMQConfig["VirtualHost"]
            };
          
            //Create the RabbitMQ connection using connection factory details as i mentioned above
            var connection = factory.CreateConnection();
            //Here we create channel with session and model
            using var channel = connection.CreateModel();
            //declare the queue after mentioning name and a few property related to that
            channel.QueueDeclare(queue: rabbitMQConfig["QueueName"],
                               durable: false,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);
            //Serialize the message
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            //put the data on to the product queue
            channel.BasicPublish(exchange: "", routingKey: "model", body: body);
        }
    }
    }


