using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace UserManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IConfiguration _configuration;
        private readonly string _queueName = "auth-events";
        private readonly IConnection _rabbitConnection;

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;

            var client = new MongoClient(_configuration.GetConnectionString("MongoDB"));
            var database = client.GetDatabase(_configuration.GetSection("MongoDB")["DatabaseName"]);
            _usersCollection = database.GetCollection<User>("users");

            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"],
                UserName = _configuration["RabbitMQ:Username"],
                Password = _configuration["RabbitMQ:Password"]
            };
            _rabbitConnection = factory.CreateConnection();
        }

        public async Task<User> CreateUser(User user)
        {
            await _usersCollection.InsertOneAsync(user);
            return user;
        }

        public async Task<User> GetUserById(string id)
        {
            return await _usersCollection.Find(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _usersCollection.Find(Builders<User>.Filter.Empty).ToListAsync();
        }

        public async Task<User> UpdateUser(string id, User updatedUser)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Id, id);
            var update = Builders<User>.Update
                .Set(user => user.FirstName, updatedUser.FirstName)
                .Set(user => user.LastName, updatedUser.LastName)
                .Set(user => user.Email, updatedUser.Email)
                .Set(user => user.Password, updatedUser.Password);

            await _usersCollection.UpdateOneAsync(filter, update);
            return await GetUserById(id);
        }

        public async Task<bool> DeleteUser(string id)
        {
            var result = await _usersCollection.DeleteOneAsync(user => user.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<bool> ValidateToken(string token)
        {
            using (var channel = _rabbitConnection.CreateModel())
            {
                var responseQueue = $"{_queueName}-response";
                channel.QueueDeclare(queue: responseQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var correlationId = Guid.NewGuid().ToString();
                var request = new TokenValidationRequest { Token = token, CorrelationId = correlationId };
                var jsonRequest = JsonSerializer.Serialize(request);
                var body = Encoding.UTF8.GetBytes(jsonRequest);

                var properties = channel.CreateBasicProperties();
                properties.CorrelationId = correlationId;
                properties.ReplyTo = responseQueue;

                channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: properties, body: body);

                var consumer = new EventingBasicConsumer(channel);
                var tcs = new TaskCompletionSource<bool>();

                consumer.Received += (model, ea) =>
                {
                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        var response = JsonSerializer.Deserialize<TokenValidationResponse>(message);
                        tcs.TrySetResult(response?.IsValid ?? false);
                    }
                };

                channel.BasicConsume(queue: responseQueue, autoAck: true, consumer: consumer);

                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    try
                    {
                        return await tcs.Task.WaitAsync(cts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        return false; // Timeout
                    }
                }
            }
        }
    }
}
