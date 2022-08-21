using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtLocal.Models
{
    public class Invoice
    {
        [Key]
        public Guid InvoiceId { get; set; }

        [Required]
        [ForeignKey("ArtworkId")]
        public Guid ArtworkId { get; set; }
        public Artwork? Artwork { get; set; }

        [Required]
        [ForeignKey("CustomerId")]
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [Required]
        public DateTime DateBought { get; set; }

        [Required]
        public int SellPrice { get; set; }
    }
}
