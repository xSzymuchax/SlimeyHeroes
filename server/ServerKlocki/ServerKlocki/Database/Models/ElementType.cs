using System.ComponentModel.DataAnnotations;

namespace ServerKlocki.Database.Models
{
    public class ElementType
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Type { get; set; } = String.Empty;
    }
}
