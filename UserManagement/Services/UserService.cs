using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
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

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;

            var client = new MongoClient(_configuration.GetConnectionString("MongoDB"));
            var database = client.GetDatabase(_configuration.GetSection("MongoDB")["DatabaseName"]);
            _usersCollection = database.GetCollection<User>("users");
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
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"] ?? "",
                UserName = _configuration["RabbitMQ:Username"] ?? "",
                Password = _configuration["RabbitMQ:Password"] ?? ""
            };

            using (var connection = await factory.CreateConnectionAsync())
             using (var channel = connection.CreateModel())
             {
                var responseQueue = $"{_queueName}-response";
                await channel.QueueDeclareAsync(queue: responseQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var correlationId = Guid.NewGuid();
                var request = new TokenValidationRequest { Token = token, CorrelationId = correlationId };
                var jsonRequest = JsonSerializer.Serialize(request);
                var body = Encoding.UTF8.GetBytes(jsonRequest);

                channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);

                var consumer = new EventingBasicConsumer(channel);
                bool isValid = false;
                var tcs = new TaskCompletionSource<bool>();

                consumer.Received += (model, ea) =>
                {
                    try
                    {
                        var responseBody = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(responseBody);
                        var tokenValidationResponse = JsonSerializer.Deserialize<TokenValidationResponse>(message);
                        if (tokenValidationResponse != null && tokenValidationResponse.CorrelationId == correlationId)
                        {
                             isValid = tokenValidationResponse.IsValid;
                            tcs.SetResult(isValid);
                        }

                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }

                };

                channel.BasicConsume(queue: responseQueue, autoAck: true, consumer: consumer);
                 await Task.WhenAny(tcs.Task, Task.Delay(5000));

                if (tcs.Task.IsCompleted)
                {
                    return tcs.Task.Result;
                }
                else
                {
                    return false;
                }
             }
        }
    }
}