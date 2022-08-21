using System.ComponentModel.DataAnnotations;

namespace ArtLocal.Models
{
    public class Artist
    {
        [Key]
        public Guid ArtistId { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

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

        [Required]
        public string? Description { get; set; }

    }
}
