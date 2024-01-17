using Microsoft.EntityFrameworkCore;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Data
{
    public class AppDBContext : DbContext
    {

        public AppDBContext(DbContextOptions options):base(options) {
        
        }

        public DbSet<TBL_INVOICE> TBL_INVOICE { get; set; }

        public DbSet<TBL_EXPENSE> TBL_EXPENSE { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          //migrations if needed.
        }
    }
}
