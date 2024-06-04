using Microsoft.EntityFrameworkCore;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using AvailableOption = XtramileBackend.Models.EntityModels.AvailableOption;

namespace XtramileBackend.Data
{
    public class AppDBContext : DbContext
    {

        public AppDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Project> TBL_PROJECT { get; set; }

        public DbSet<Priority> TBL_PRIORITY { get; set; }

        public DbSet<Department> TBL_DEPARTMENT { get; set; }

        public DbSet<Invoice> TBL_INVOICE { get; set; }

        public DbSet<Expense> TBL_EXPENSE { get; set; }

        public DbSet<Employee> TBL_EMPLOYEE { get; set; }

        public DbSet<Roles> TBL_ROLES { get; set; }

        public DbSet<Country> TBL_COUNTRY { get; set; }

        public DbSet<PerDiem> TBL_PER_DIEM { get; set; }

        public DbSet<Status> TBL_STATUS { get; set; }

        public DbSet<FileFormat> TBL_FILE_TYPE { get; set; }

        public DbSet<Reason> TBL_REASON { get; set; }

        public DbSet<Request> TBL_REQUEST { get; set; }

        public DbSet<TravelType> TBL_TRAVEL_TYPE { get; set; }

        public DbSet<TravelMode> TBL_TRAVEL_MODE { get; set; }

        public DbSet<AvailableOption> TBL_AVAIL_OPTION { get; set; }

        public DbSet<Category> TBL_CATEGORY { get; set; }

        public DbSet<RequestApprove> TBL_REQ_APPROVE { get; set; }

        public DbSet<ProjectEmployeeMap> TBL_PROJECT_MAPPING { get; set; }

        public DbSet<TBL_REQ_MAPPING> TBL_REQ_MAPPING { get; set; }

        public DbSet<FileMetaData> TBL_FILE_METADATA { get; set; }

        public DbSet<TravelOption> TBL_TRAVEL_OPTIONS { get; set; }

        public DbSet<TravelOptionMap> TBL_TRAVEL_OPTION_MAPPING { get; set; }

        public DbSet<TravelDocumentFileDataModel> TravelDocumentFileData { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<User> TBL_USER { get; set; }
        public DbSet<Ticket> TBL_TICKET { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //migrations if needed.
        }
    }
}
