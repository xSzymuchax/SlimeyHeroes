using System.ComponentModel.DataAnnotations;

namespace ServerKlocki.Database.Models
{
    public class Rarity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = String.Empty;
    }
}
