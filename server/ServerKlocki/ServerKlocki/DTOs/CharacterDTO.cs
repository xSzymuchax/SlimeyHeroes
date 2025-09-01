namespace ServerKlocki.DTOs
{
    /// <summary>
    /// This is regular response for the characters API. 
    /// </summary>
    public class CharacterDTO
    {
        /// <summary>
        /// Character's Id.
        /// </summary>
        public int CharacterId { get; set; }

        /// <summary>
        /// Character's Level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Amount of character's souls.
        /// </summary>
        public int SoulsAmount { get; set; }

    }
}
