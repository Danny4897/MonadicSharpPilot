using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Infrastructure.Persistence;
using Domain.Customer.Repository;
using System.Text.Json.Serialization;
using System.Linq;
using System.Threading.Tasks;
using System;
using MonadicPilot.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "MonadicPilot API",
        Version = "v1",
        Description = "API per testare MonadicSharp library"
    });
});

// Configure Database
var useInMemoryDb = builder.Configuration.GetValue<bool>("Environment:UseInMemoryDatabase");

if (useInMemoryDb)
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("MonadicPilotTestDb"));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// Register repositories and services
builder.Services.AddScoped<Domain.Customer.ICustomerRepository, Domain.Customer.Repository.CustomerRepository>();
builder.Services.AddScoped<Domain.Customer.Services.ICustomerService, Domain.Customer.Services.CustomerService>();

// Configure CORS for development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MonadicPilot API V1");
        c.RoutePrefix = string.Empty; // Swagger UI at root
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Map customer endpoints
MonadicPilot.Controllers.CustomerController.MapCustomerEndpoints(app);

// Initialize database and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!useInMemoryDb)
    {
        context.Database.EnsureCreated();
    }

    // Seed test data if configured
    if (builder.Configuration.GetValue<bool>("Environment:SeedTestData"))
    {
        await SeedTestData(context);
    }
}

Console.WriteLine("MonadicPilot API started successfully");

app.Run();

static async Task SeedTestData(ApplicationDbContext context)
{
    if (!context.Customers.Any())
    {
        Console.WriteLine("Seeding test data...");

        // Create sample addresses first
        var address1 = Domain.Customer.ValueObjects.Address.Create("123 Main St", "Milano", "20100", "Italy");
        var address2 = Domain.Customer.ValueObjects.Address.Create("456 Tech Ave", "Roma", "00100", "Italy");

        if (address1.IsSuccess && address2.IsSuccess)
        {
            // Create sample customers to showcase MonadicSharp
            var customer1 = Domain.Customer.Customer.Create(
                "Acme Corp",
                "1234567890",
                "contact@acme.com",
                address1.Value
            );

            var customer2 = Domain.Customer.Customer.Create(
                "Tech Solutions Ltd",
                "0987654321",
                "info@techsolutions.com",
                address2.Value
            );

            if (customer1.IsSuccess)
            {
                context.Customers.Add(customer1.Value);
            }

            if (customer2.IsSuccess)
            {
                context.Customers.Add(customer2.Value);
            }

            await context.SaveChangesAsync();
            Console.WriteLine("Test data seeded successfully");
        }
        else
        {
            Console.WriteLine("Failed to create test addresses");
        }
    }
}