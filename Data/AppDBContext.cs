using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Data
{
    public class AppDBContext : DbContext
    {

        public AppDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<TBL_PROJECT> TBL_PROJECT { get; set; }

        public DbSet<TBL_PRIORITY> TBL_PRIORITY { get; set; }

        public DbSet<TBL_DEPARTMENT> TBL_DEPARTMENT { get; set; }

        public DbSet<TBL_INVOICE> TBL_INVOICE { get; set; }

        public DbSet<TBL_EXPENSE> TBL_EXPENSE { get; set; }

        public DbSet<TBL_EMPLOYEE> TBL_EMPLOYEE { get; set; }

        public DbSet<TBL_ROLES> TBL_ROLES { get; set; }

        public DbSet<TBL_COUNTRY> TBL_COUNTRY { get; set; }

        public DbSet<TBL_PER_DIUM> TBL_PER_DIUM { get; set; }

        public DbSet<TBL_STATUS> TBL_STATUS { get; set; }

        public DbSet<TBL_FILE_TYPE> TBL_FILE_TYPE { get; set; }

        public DbSet<TBL_REASON> TBL_REASON { get; set; }

        public DbSet<TBL_REQUEST> TBL_REQUEST { get; set; }

        public DbSet<TBL_TRAVEL_TYPE>TBL_TRAVEL_TYPE  { get; set; }

        public DbSet<TBL_TRAVEL_MODE> tBL_TRAVEL_MODE { get; set; }

        public DbSet<TBL_AVAIL_OPTION> TBL_AVAIL_OPTION { get; set; }

        public DbSet<TBL_CATEGORY> TBL_CATEGORY { get; set; }

        public DbSet<TBL_REQ_APPROVE> TBL_REQ_APPROVE { get; set; }

        public DbSet<TBL_PROJECT_MAPPING> TBL_PROJECT_MAPPING { get; set; }

        public DbSet<TBL_REQ_MAPPING> TBL_REQ_MAPPING { get; set; }


        public DbSet<TBL_USER> TBL_USER { get; set; }

        public DbSet<TBL_FILE_METADATA> TBL_FILE_METADATA { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //migrations if needed.
        }
    }
}
