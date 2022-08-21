using System.ComponentModel.DataAnnotations;

namespace ArtLocal.Models
{
    public class ArtStyle
    {
        [Key]
        public Guid ArtStyleId { get; set; }

        [Required]
        public string? Style { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}
