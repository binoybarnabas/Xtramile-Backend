using Microsoft.EntityFrameworkCore;
using XtramileBackend.Data;
using XtramileBackend.Repositories;
using XtramileBackend.Repositories.PriorityRepository;
using XtramileBackend.Repositories.ProjectRepository;
using XtramileBackend.Services.EmployeeService;
using XtramileBackend.Services.PriorityService;
using XtramileBackend.Services.ProjectService;
using XtramileBackend.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDBContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DB_KEY")));
// Add services to the container.

builder.Services.AddControllers();

//Dependency injections
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IProjectRepository,ProjectRepository>();
builder.Services.AddScoped<IPriorityRepository, PriorityRepository>();


builder.Services.AddScoped<IPriorityServices, PriorityServices>();
builder.Services.AddScoped<IProjectServices, ProjectServices>();
builder.Services.AddScoped<IEmployeeServices,EmployeeServices>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
