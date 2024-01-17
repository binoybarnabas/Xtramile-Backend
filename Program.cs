using Microsoft.EntityFrameworkCore;
using XtramileBackend.Data;
using XtramileBackend.Repositories;
using XtramileBackend.Repositories.ExpenseRepository;
using XtramileBackend.Repositories.InvoiceRepository;
using XtramileBackend.Services.ExpenseService;
using XtramileBackend.Services.InvoiceService;
using XtramileBackend.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDBContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DB_KEY")));
// Add services to the container.

builder.Services.AddControllers();

//Dependency injections
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IInvoiceRepository,InvoiceRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IExpenseServices, ExpenseServices>();
builder.Services.AddScoped<IInvoiceServices, InvoiceServices>();

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
