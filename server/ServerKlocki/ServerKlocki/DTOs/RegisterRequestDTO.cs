namespace ServerKlocki.DTOs
{
    /// <summary>
    /// User register parameters.
    /// </summary>
    public class RegisterRequestDTO
    {
        /// <summary>
        /// Name of new user.
        /// </summary>
        public required string Username { get; set; }
        
        /// <summary>
        /// New user email adress.
        /// </summary>
        public required string Email { get; set; }
        
        /// <summary>
        /// New user password.
        /// </summary>
        public required string Password { get; set; }
        
        /// <summary>
        /// Confirmation of password.
        /// </summary>
        public required string Password2 { get; set; }
    }
}
