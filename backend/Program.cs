// using API.Controllers;
// using Application.Customer.Commands;
// using Application.Customer.Queries;
// using Domain.Customer;
// using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MediatR
// builder.Services.AddMediatR(cfg => {
//     cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly);
// });

// Add DbContext
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseInMemoryDatabase("CustomerDb"));

// Add Repositories
// builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map endpoints
// app.MapCustomerEndpoints();

app.Run();