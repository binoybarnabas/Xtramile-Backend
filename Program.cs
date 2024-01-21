using Microsoft.EntityFrameworkCore;
using XtramileBackend.Data;
using XtramileBackend.Repositories;
using XtramileBackend.Repositories.CountryRepository;
using XtramileBackend.Repositories.DepartmentRepository;
using XtramileBackend.Repositories.EmployeeRepository;
using XtramileBackend.Repositories.ExpenseRepository;
using XtramileBackend.Repositories.InvoiceRepository;
using XtramileBackend.Repositories.PerdiumRepository;
using XtramileBackend.Repositories.PriorityRepository;
using XtramileBackend.Repositories.ProjectRepository;
using XtramileBackend.Repositories.RoleRepository;
using XtramileBackend.Repositories.FileTypeRepository;
using XtramileBackend.Repositories.ReasonRepository;
using XtramileBackend.Repositories.RequestRepository;
using XtramileBackend.Repositories.StatusRepository;
using XtramileBackend.Services.FileTypeService;
using XtramileBackend.Repositories.TravelModeRepository;
using XtramileBackend.Repositories.TravelTypeRepository;
using XtramileBackend.Services.CountryService;
using XtramileBackend.Services.DepartmentService;
using XtramileBackend.Services.EmployeeService;
using XtramileBackend.Services.ExpenseService;
using XtramileBackend.Services.InvoiceService;
using XtramileBackend.Services.PerdiumService;
using XtramileBackend.Services.PriorityService;
using XtramileBackend.Services.ProjectService;
using XtramileBackend.Services.RolesService;
using XtramileBackend.Services.ReasonService;
using XtramileBackend.Services.RequestService;
using XtramileBackend.Services.StatusService;
using XtramileBackend.Services.TravelModeService;
using XtramileBackend.Services.TravelTypeService;
using XtramileBackend.UnitOfWork;
using XtramileBackend.Repositories.CategoryRepository;
using XtramileBackend.Repositories.AvailableOptionRepository;
using XtramileBackend.Services.AvailableOptionService;
using XtramileBackend.Services.CategoryService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDBContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DB_KEY")));
// Add services to the container.

builder.Services.AddControllers();

//Dependency injections
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IPriorityRepository, PriorityRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IPerdiumRepository, PerdiumRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IReasonRepository, ReasonRepository>();
builder.Services.AddScoped<IFileTypeRepository, FileTypeRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<ITravelTypeRepository, TravelTypeRepository>();
builder.Services.AddScoped<ITravelModeRepository, TravelModeRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAvailableOptionRepository, AvailableOptionRepository>();

builder.Services.AddScoped<IPriorityServices, PriorityServices>();
builder.Services.AddScoped<IProjectServices, ProjectServices>();
builder.Services.AddScoped<IDepartmentServices, DepartmentServices>();
builder.Services.AddScoped<IEmployeeServices, EmployeeServices>();
builder.Services.AddScoped<IRolesServices, RolesServices>();
builder.Services.AddScoped<ICountryServices, CountryServices>();
builder.Services.AddScoped<IPerdiumServices, PerdiumServices>();
builder.Services.AddScoped<IInvoiceServices, InvoiceServices>();
builder.Services.AddScoped<IExpenseServices, ExpenseServices>();
builder.Services.AddScoped<IFileTypeServices, FileTypeServices>();
builder.Services.AddScoped<IReasonServices, ReasonServices>();
builder.Services.AddScoped<IStatusServices, StatusServices>();
builder.Services.AddScoped<IRequestServices, RequestServices>();
builder.Services.AddScoped<ITravelTypeService, TravelTypeService>();
builder.Services.AddScoped<ITravelModeService, TravelModeService>();
builder.Services.AddScoped<IAvailableOptionServices, AvailableOptionServices>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngularDev");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
