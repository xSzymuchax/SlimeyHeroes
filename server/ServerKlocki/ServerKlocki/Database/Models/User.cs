using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ServerKlocki.Database.Models
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = String.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;

        [Required]
        public string Password { get; set; } = String.Empty;
    }
}
