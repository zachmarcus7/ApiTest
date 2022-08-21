using System.ComponentModel.DataAnnotations;

namespace ArtLocal.Models
{
    public class Gallery
    {
        [Key]
        public Guid GalleryId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? State { get; set; }

        [Required]
        public int PostalCode { get; set; }

        [Required]
        public string? Country { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }
    }
}
