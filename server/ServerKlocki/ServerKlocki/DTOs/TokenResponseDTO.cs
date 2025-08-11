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
        public string message;

        /// <summary>
        /// Authorization Token.
        /// </summary>
        public string token;
    }
}
