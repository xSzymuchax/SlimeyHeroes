namespace ServerKlocki.GameStates
{
    public class LobbyState
    {
        public string MatchId { get; set; }
        public List<PlayerConfiguration> Players { get; set; } = new();
    }
}
