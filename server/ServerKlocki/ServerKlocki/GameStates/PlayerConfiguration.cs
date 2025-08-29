namespace ServerKlocki.GameStates
{
    public class PlayerConfiguration
    {
        public string PlayerId { get; set; }
        public string ConnectionId { get; set; }
        public bool IsReady { get; set; }

        public Character? character1 { get; set; }
        public Character? character2 { get; set; }
        public Character? character3 { get; set; }

        public Perk? perk1 { get; set; }
        public Perk? perk2 { get; set; }
    }
}
