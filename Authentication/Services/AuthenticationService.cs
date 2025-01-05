using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using Authentication.Models;

namespace Authentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly string _queueName = "auth-events";

        public AuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void StartListening()
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"],
                UserName = _configuration["RabbitMQ:Username"],
                Password = _configuration["RabbitMQ:Password"]
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var tokenValidationRequest = JsonSerializer.Deserialize<TokenValidationRequest>(message);

                    bool isValid = ValidateToken(tokenValidationRequest.Token);
                    var response = new TokenValidationResponse { IsValid = isValid, CorrelationId = tokenValidationRequest.CorrelationId };

                    //Publicar a resposta de volta para o UserManagement
                    PublishResponse(channel, response);
                };

                channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
                Console.WriteLine(" [*] Waiting for messages.");
                Console.ReadLine();
            }
        }
        private void PublishResponse(IModel channel, TokenValidationResponse response)
        {
            var responseQueue = $"{_queueName}-response";
            channel.QueueDeclare(queue: responseQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var jsonResponse = JsonSerializer.Serialize(response);
            var body = Encoding.UTF8.GetBytes(jsonResponse);

            channel.BasicPublish(exchange: "", routingKey: responseQueue, basicProperties: null, body: body);
        }

        public bool ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}