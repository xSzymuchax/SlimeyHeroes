namespace ServerKlocki.DTOs
{
    /// <summary>
    /// Its response for login. It contains token for further authorizations.
    /// </summary>
    public class TokenResponseDTO
    {
        /// <summary>
        /// Response message.
        /// </summary>
        public required string message { get; set; }

        /// <summary>
        /// Authorization Token.
        /// </summary>
        public required string token { get; set; }
    }
}
