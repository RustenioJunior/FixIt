using UserManagement.Models;
using UserManagement.Repositories;
using UserManagement.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MongoDB settings and GuidRepresentation
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Configure GuidRepresentation globally to Standard
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard)); // Use Standard for consistent Guid representation

// Use Standard Guid Representation
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
if (mongoDbSettings == null)
{
    throw new InvalidOperationException("MongoDbSettings section is missing in the configuration.");
}
var clientSettings = MongoClientSettings.FromConnectionString(mongoDbSettings.ConnectionString);

builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(clientSettings));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Serve the Swagger UI at the app's root
    });
}

// Comment out or remove this line to disable HTTPS redirection
// app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run("http://0.0.0.0:80");