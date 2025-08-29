using Microsoft.AspNetCore.SignalR;

namespace ServerKlocki.Providers
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst("id")?.Value;
        }
    }
}
