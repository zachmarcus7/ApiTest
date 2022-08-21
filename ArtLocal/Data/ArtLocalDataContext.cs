using ArtLocal.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtLocal.Data
{
    public class ArtLocalDataContext : DbContext
    {
        // constructor
        public ArtLocalDataContext(DbContextOptions options) : base(options)
        {

        }

        // dbsets
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<ArtStyle> ArtStyle { get; set; }
        public DbSet<Artwork> Artwork { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
    }
}
