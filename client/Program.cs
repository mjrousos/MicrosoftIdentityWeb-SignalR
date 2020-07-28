using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
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
