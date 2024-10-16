using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using System.Data;
using ShopDap.Repositories;
using ShopDap.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

///////////////////////////////// 
// Add services to the container. 
/////////////////////////////////

builder.Services.AddControllers();

// CONFIG FILES
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);

// SWAGGER
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ShopDap API",
        Description = "An ASP.NET Core Web API for managing Product and Order items",
    });
});

// DATABASE (MySQL)
builder.Services.AddScoped<MySqlConnection>(s =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var connection = new MySqlConnection(connectionString);
    connection.Open(); // Ensure connection is opened for transactions
    return connection;
});

builder.Services.AddScoped<IDbTransaction>(s =>
{
    var connection = s.GetRequiredService<MySqlConnection>();
    return connection.BeginTransaction(); // Begin transaction for unit of work
});

// DEPENDENCY INJECTION (Repositories and UnitOfWork)
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

/////////////////////////////////////// 
// Configure the HTTP request pipeline. 
///////////////////////////////////////

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
