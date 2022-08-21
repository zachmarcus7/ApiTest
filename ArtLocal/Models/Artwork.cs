using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;


namespace ArtLocal.Models
{
    public class Artwork
    {
        [Key]
        public Guid ArtworkId { get; set; }

        [Required(ErrorMessage = "artist id required")]
        [ForeignKey("ArtistId")]
        public Guid ArtistId { get; set; }
        public Artist? Artist { get; set; }

        [Required(ErrorMessage = "title required")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "description required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "date created required")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "price required")]
        public int Price { get; set; }

        [Required(ErrorMessage = "location required")]
        public string? ImageLocation { get; set; }

        [Required(ErrorMessage = "sold required")]
        public bool Sold { get; set; }

        [Required(ErrorMessage = "gallery id required")]
        [ForeignKey("GalleryId")]
        public Guid GalleryId { get; set; }
        public Gallery? Gallery { get; set; }

        [Required(ErrorMessage = "artstyle id required")]
        [ForeignKey("ArtStyleId")]
        public Guid ArtStyleId { get; set; }
        public ArtStyle? ArtStyle { get; set; }
    }
}
