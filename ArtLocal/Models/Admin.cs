using System.ComponentModel.DataAnnotations;

namespace ArtLocal.Models
{
    public class Admin
    {
        [Key]
        public Guid AdminId { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

    }
}
