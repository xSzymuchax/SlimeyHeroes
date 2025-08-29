using Microsoft.AspNetCore.SignalR;

namespace ServerKlocki.Hubs
{
    public class MatchmakingHub : Hub
    {
        private static Queue<string> waitingPlayers = new Queue<string>();
        private static readonly object _lock = new object();

        public override async Task OnConnectedAsync()
        {
            var playerId = Context.UserIdentifier;
            Console.WriteLine($"Player: {playerId} connected");

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Matchmaking method, it pairs players in 2, then 
        /// it creates match between them.
        /// </summary>
        /// <returns></returns>
        public async Task FindMatch()
        {
            string? player1 = null;
            string? player2 = null;

            lock (_lock)
            {
                if (waitingPlayers.Count >= 1)
                {
                    player1 = waitingPlayers.Dequeue();
                    player2 = Context.ConnectionId;
                }
                else
                {
                    waitingPlayers.Enqueue(Context.ConnectionId);
                }
            }

            if (player1 != null && player2 != null)
            {
                string matchId = Guid.NewGuid().ToString();
                await Clients.Client(player1).SendAsync("MatchFound", matchId);
                await Clients.Client(player2).SendAsync("MatchFound", matchId);
            }
        }

        public async Task CancelMatchmaking()
        {
            string connectionId = Context.ConnectionId;

            lock (_lock)
            {
                waitingPlayers = new Queue<string>(
                    waitingPlayers.Where(id => id != connectionId));
            }

            // potwierdzenie
            await Clients.Caller.SendAsync("MatchmakingCancelled");
        }
    }
}
