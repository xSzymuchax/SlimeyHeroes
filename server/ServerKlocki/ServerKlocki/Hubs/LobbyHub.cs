using Microsoft.AspNetCore.SignalR;
using ServerKlocki.GameStates;

namespace ServerKlocki.Hubs
{

    public class LobbyHub : Hub
    {
        public static Dictionary<string, LobbyState> lobbies = new();

        public async Task JoinLobby(string matchId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, matchId);
        }

        public async Task SetCharacters(string matchId, int character1, int character2, int character3)
        {

        }

        public async Task SetPerks(string matchId, int perk1, int perk2)
        {

        }

        public async Task PlayerReady(string matchId)
        {
            
        }


    }
}
