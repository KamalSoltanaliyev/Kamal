using cay_verersen.Models;
using Microsoft.EntityFrameworkCore;

namespace cay_verersen.Contexts
{
    public class CayverersenDbContext : DbContext
    {
        public CayverersenDbContext(DbContextOptions<CayverersenDbContext> options) : base(options)
        { 
        }

        public DbSet<Slider> Sliders { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
    }
}
