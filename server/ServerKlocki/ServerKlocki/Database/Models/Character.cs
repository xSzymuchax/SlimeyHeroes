using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerKlocki.Database.Models
{
    public class Character
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = String.Empty;

        [Required]
        public string Description { get; set; } = String.Empty;


        // relations
        [Required]
        [ForeignKey(nameof(Rarity))]
        public int RarityId { get; set; }
        public virtual Rarity? Rarity { get; set; }

        [Required]
        [ForeignKey(nameof(ElementType))]
        public int ElementTypeId { get; set; }
        public virtual ElementType? ElementType { get; set; } = null!;
    }
}
