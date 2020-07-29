using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace server.Hubs
{
    // An authorize attribute on a SignalR hub will require that
    // connecting users be authenticated (just like a controller)
    [Authorize]
    public class TestHub: Hub
    {
        private ILogger<TestHub> Logger { get; }

        public TestHub(ILogger<TestHub> logger)
        {
            Logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            // If an authenticated user connects, user information is available in Context.User and Context.UserIdentifier
            Logger.LogInformation($"User connected to SignalR test hub: {Context.UserIdentifier ?? "<Anonymous>"}");

            await Clients.Caller.SendAsync("MessageFromServer", "ok");

            await base.OnConnectedAsync();
        }

        public string Echo(string message) => $"\"{message}\" has {message.Length} characters";
    }
}
