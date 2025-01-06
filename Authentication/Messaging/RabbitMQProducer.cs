using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Authentication.Messaging
{
    public class RabbitMQProducer
    {
        private readonly IConfiguration _configuration;
        private readonly string _queueName;

        public RabbitMQProducer(IConfiguration configuration, string queueName)
        {
            _configuration = configuration;
            _queueName = queueName;
        }

        public async Task SendMessageAsync(string message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"] ?? "",
                UserName = _configuration["RabbitMQ:Username"] ?? "",
                Password = _configuration["RabbitMQ:Password"] ?? ""
            };

            try
            {
                using (var connection = await factory.CreateConnectionAsync())
                {
                     using(var channel = connection.CreateModel())
                     {
                       await channel.QueueDeclareAsync(queue: _queueName,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                         string jsonString = JsonSerializer.Serialize(new { Message = message });
                          var body = Encoding.UTF8.GetBytes(jsonString);

                          channel.BasicPublish(exchange: "",
                                            routingKey: _queueName,
                                            basicProperties: null,
                                            body: body);
                     }

                 }
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"Erro ao enviar mensagem para RabbitMQ: {ex.Message}");
                    throw; // Relança a exceção para o chamador saber que ocorreu um problema.
            }
        }
    }
}