using Microsoft.AspNetCore.SignalR;
using ServerKlocki.GameStates;
using System.Diagnostics;

namespace ServerKlocki.Hubs
{

    public class LobbyHub : Hub
    {
        public static Dictionary<string, LobbyState> lobbies = new();
        public static Object _lock = new object();

        public override async Task OnConnectedAsync()
        {
            var playerId = Context.UserIdentifier;
            Console.WriteLine($"Player: {playerId} connected");

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Allows players to join lobby. If two players join, then it sends signal
        /// to start countdown and get ready.
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns></returns>
        public async Task JoinLobby(string matchId)
        {
            // add connection to broadcast group
            await Groups.AddToGroupAsync(Context.ConnectionId, matchId);

            await Task.Delay(50);

            // create lobby
            LobbyState lobby;
            if (!lobbies.ContainsKey(matchId))
            {
                lock (_lock)
                {
                    lobbies.Add(matchId, new LobbyState() { MatchId = matchId });
                }
                    
            }

            lobby = lobbies[matchId];

            // add player
            PlayerConfiguration pc = new PlayerConfiguration() 
            { 
                ConnectionId = Context.ConnectionId,
                PlayerId = Context.UserIdentifier!
            };

            lock (_lock)
            {
                lobby.Players.Add(pc);
            }

            // wait for 2nd player, then start countdown
            if (lobby.Players.Count == 2)
            {
                // start countdown
                _ = StartCancelCooldown(matchId, 35);

                // send start countdown event to clients
                Debug.WriteLine("sending startcountdown");
                await Clients.Group(matchId).SendAsync("StartCountdown", 30);
            }
        }

        /// <summary>
        /// method counting down for game cancellation, if other player left or didnt get ready
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="seconds">time in seconds, before lobby will get destroyed</param>
        /// <returns></returns>
        private async Task StartCancelCooldown(string matchId, int seconds)
        {
            await Task.Delay(seconds * 1000);

            lock (_lock)
            {
                if (!lobbies.ContainsKey(matchId))
                    return;

                LobbyState lobby = lobbies[matchId];

                if (lobby.Players[0].IsReady && lobby.Players[1].IsReady)
                    return;

                lobbies.Remove(matchId);
            }


        }

        public async Task SetCharacters(string matchId, int character1, int character2, int character3)
        {
            // set player's corresponding characters
        }

        public async Task SetPerks(string matchId, int perk1, int perk2)
        {
            // set player's corresponding perks
        }

        public async Task PlayerReady(string matchId)
        {
            // if both players ready, start game -> redirect to gameHub
            string playerId = Context.UserIdentifier!;

            if (!lobbies.ContainsKey(matchId))
            {
                return;
            }

            LobbyState lobby = lobbies[matchId];

            lock (_lock)
            {
                PlayerConfiguration pc = lobby.Players.Find(x => playerId == x.PlayerId);
                
                if (pc != null)
                {
                    pc.IsReady = true;
                }
            }

            if (lobby.Players[0].IsReady && lobby.Players[1].IsReady)
                await Clients.Group(matchId).SendAsync("GameStart");
        }


    }
}
