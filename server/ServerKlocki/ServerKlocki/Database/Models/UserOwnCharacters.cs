using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerKlocki.Database.Models
{
    public class UserOwnCharacters
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int SoulsAmount { get; set; }

        [Required]
        public int CharacterLevel { get; set; }


        // foreign keys
        [Required]
        [ForeignKey(nameof(Character))]
        public int CharacterId { get; set; }
        public virtual Character? Character { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
