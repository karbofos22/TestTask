using Microsoft.EntityFrameworkCore;

namespace TestTask.Models
{
    public class Context : DbContext
    {
        public DbSet<Offer> Offers { get; set; }
        public Context (DbContextOptions<Context> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
