using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        private static IConfiguration Configuration { get; } = GetConfiguration();

        private static async Task Main()
        {
            Console.WriteLine("Calling protected web API...");
            await CallWebApiAsync();

            Console.WriteLine("Connecting to protected SignalR hub...");
            await CallSignalRAsync();

            Console.WriteLine("Done");
        }

        private static async Task CallWebApiAsync()
        {
            var client = new HttpClient();
            var message = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5001/WeatherForecast");
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Configuration["jwt"]);
            var response = await client.SendAsync(message);
            Console.WriteLine($"Response: {response.StatusCode}");
        }

        private static async Task CallSignalRAsync()
        {
            var connection = new HubConnectionBuilder()
                // If cookie authentication is used, cookies from a web site will automatically be 
                // used to authenticate SignalR connections. In bearer token authentication scenarios,
                // the token is specified using an access token provider.
                // https://docs.microsoft.com/aspnet/core/signalr/authn-and-authz
                .WithUrl("https://localhost:5001/TestHub", connectionOptions => connectionOptions.AccessTokenProvider = () => Task.FromResult(Configuration["jwt"]))
                .Build();

            connection.On<string>("MessageFromServer", message => Console.WriteLine($"SignalR message receive status: {message}"));

            await connection.StartAsync();
            var echoResult = await connection.InvokeAsync<string>("Echo", "Test message");
            if (!string.IsNullOrEmpty(echoResult) && echoResult.Equals("\"Test message\" has 12 characters"))
            {
                Console.WriteLine("SignalR message send status: ok");
            }
            else
            {
                Console.WriteLine($"SignalR message send FAILED (received {echoResult})");
            }
        }

        private static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>()
                .Build();
        }
    }
}
