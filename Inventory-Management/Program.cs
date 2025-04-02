using Inventory_Management.Context;
using Inventory_Management.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<InventoryDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContextPool<InventoryDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// If connection to the database is successful, print a message to the console
using (var db = new InventoryDbContext(builder.Services.BuildServiceProvider().GetRequiredService<DbContextOptions<InventoryDbContext>>()))
{
    db.Database.EnsureCreated();
    Console.WriteLine("Connected to the database successfully!");
}

builder.Services.AddScoped<RetailDataParser>();

// Uncomment the following code to parse and save the data from the CSV file into the database

// RetailDataParser retailDataParser = builder.Services.BuildServiceProvider().GetRequiredService<RetailDataParser>();

// retailDataParser.ParseAndSaveData("C:\\Users\\spac-25\\source\\repos\\Inventory-Management\\Inventory-Management\\synthetic_online_retail_data.csv");

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
