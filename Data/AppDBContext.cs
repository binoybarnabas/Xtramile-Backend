using Microsoft.EntityFrameworkCore;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Data
{
    public class AppDBContext : DbContext
    {

        public AppDBContext(DbContextOptions options):base(options) {
        
        }

        public DbSet<TBL_PROJECT> TBL_PROJECT { get; set; }

        public DbSet<TBL_PRIORITY> TBL_PRIORITY { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          //migrations if needed.
        }
    }
}
