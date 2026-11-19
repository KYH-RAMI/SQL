using FailSafeRentals.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrera controllers
builder.Services.AddControllers();
var connectionName = builder.Configuration["DatabaseConnection"] ?? "DefaultConnection";

var connectionString = builder.Configuration.GetConnectionString(connectionName)
    ?? throw new InvalidOperationException($"Anslutningssträngen {connectionName} saknas.");

builder.Services.AddDbContext<FailSafeRentalsDbContext>(options =>
    options.UseSqlServer(connectionString));


// Registrera Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Aktivera Swagger i utvecklingsmiljön
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
