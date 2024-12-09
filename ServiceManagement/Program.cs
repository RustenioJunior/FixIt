using Data;
using Microsoft.EntityFrameworkCore;

<<<<<<< HEAD

var builder = WebApplication.CreateBuilder(args);

=======
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
>>>>>>> 543153a398194512440a6ef947499a4ba85bed59
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;

<<<<<<< HEAD
builder.Services.AddDbContext<AppointmentContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<Machine_typeContext>(options =>
options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<StatusContext>(options =>
options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<PartsContext>(options =>
options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<CompanyContext>(options =>
options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<Machine_modContext>(options =>
options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<MachineContext>(options =>
options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<ServiceContext>(options =>
options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<ReviewContext>(options =>
options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

=======
var app = builder.Build();

// Configure the HTTP request pipeline.
>>>>>>> 543153a398194512440a6ef947499a4ba85bed59
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
<<<<<<< HEAD
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServiceManagement v1"); //
        c.RoutePrefix = string.Empty; // Serve swagger UI at the root
=======
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Serve the Swagger UI at the app's root
>>>>>>> 543153a398194512440a6ef947499a4ba85bed59
    });
}

app.UseHttpsRedirection();
<<<<<<< HEAD
app.UseAuthorization();
app.MapControllers();

app.Run("http://0.0.0.0:80");
=======

app.UseAuthorization();

app.MapControllers();

app.Run(); 
>>>>>>> 543153a398194512440a6ef947499a4ba85bed59
