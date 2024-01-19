﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Data
{
    public class AppDBContext : DbContext
    {

        public AppDBContext(DbContextOptions options):base(options) {
        
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

        public DbSet<TBL_TRAVEL_TYPE>TBL_TRAVEL_TYPE  { get; set; }

        public DbSet<TBL_TRAVEL_MODE> tBL_TRAVEL_MODE { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          //migrations if needed.
        }
    }
}
