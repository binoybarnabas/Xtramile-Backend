using Microsoft.EntityFrameworkCore;
using XtramileBackend.Data;
using XtramileBackend.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
using XtramileBackend.Repositories.RequestStatusRepository;
using XtramileBackend.Repositories.ProjectMappingRepository;
using XtramileBackend.Services.RequestStatusService;
using XtramileBackend.Services.ProjectMappingService;

using System.Text;
using XtramileBackend.Services.AuthService;

using XtramileBackend.Services.EmployeeViewPenReqService;
using XtramileBackend.Services.FinanceDepartment;
using XtramileBackend.Repositories.RequestMappingRepository;
using XtramileBackend.Services.RequestMappingService;


var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();


var secretkey = DotNetEnv.Env.GetString("SECRET_KEY");
var issuer = DotNetEnv.Env.GetString("ISSUER");

builder.Configuration["Jwt:SecretKey"] = secretkey;
builder.Configuration["Jwt:Issuer"] = issuer;

var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:SecretKey").Get<string>();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = "",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});


//adding policy for authorization
builder.Services.AddAuthorization(
    option => {
        option.AddPolicy("Employee", policy => policy.RequireRole("Employee"));
        option.AddPolicy("Manager", policy => policy.RequireRole("Manager"));
        option.AddPolicy("Head", policy => policy.RequireRole("Head"));
        option.AddPolicy("CXO", policy => policy.RequireRole("CXO"));

    });

// Add services to the container.
builder.Services.AddControllers();

var dbConnectionString = DotNetEnv.Env.GetString("DB_STRING");

builder.Configuration["ConnectionStrings:DB_KEY"] = dbConnectionString;


builder.Services.AddDbContext<AppDBContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DB_KEY")));


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
builder.Services.AddScoped<IRequestStatusRepository, RequestStatusRepository>();
builder.Services.AddScoped<IProjectMappingRepository, ProjectMappingRepository>();
builder.Services.AddScoped<IRequestMappingRepsitory, RequestMappingRepository>();


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
builder.Services.AddScoped<IRequestStatusServices, RequestStatusServices>();
builder.Services.AddScoped<IProjectMappingServices, ProjectMappingServices>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IEmployeeViewPenReqService, EmployeeViewPenReqService>();
builder.Services.AddScoped<IFinanceDepartmentService, FinanceDepartmentService>();
builder.Services.AddScoped<IRequestMappingService, RequestMappingService>();


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


app.UseHttpsRedirection();


app.UseCors("AllowAngularDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
