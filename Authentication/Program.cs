using Authentication.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
         services.AddSingleton<IAuthenticationService, AuthenticationService>();
    });

var host = builder.Build();

var authenticationService = host.Services.GetRequiredService<IAuthenticationService>();
authenticationService.StartListening();

await host.RunAsync();