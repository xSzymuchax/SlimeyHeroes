namespace ServerKlocki.DTOs
{
    /// <summary>
    /// Data object for user login.
    /// </summary>
    public class LoginDataDTO
    {
        /// <summary>
        /// Email of user who is logging.
        /// </summary>
        public required string Email { get; set; }
    
        /// <summary>
        /// Password of user.
        /// </summary>
        public required string Password { get;set; }
    }
}
