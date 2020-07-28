using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace server.Hubs
{
    public class TestHub: Hub
    {
        private ILogger<TestHub> Logger { get; }

        public TestHub(ILogger<TestHub> logger)
        {
            Logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            Logger.LogInformation($"User connected to SignalR test hub: {Context.User.Identity.Name}");

            await Clients.Caller.SendAsync("MessageFromServer", "ok");

            await base.OnConnectedAsync();
        }

        public string Echo(string message) => $"\"{message}\" has {message.Length} characters";
    }
}
