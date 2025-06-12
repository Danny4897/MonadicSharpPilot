using System;
using System.Threading.Tasks;
using Domain.Customer;
using Domain.Customer.Services;
using FunctionalSharp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace API.Controllers;

public static class CustomerController
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/customers")
            .WithTags("Customers");

        group.MapGet("/", async (ICustomerService customerService) =>
        {
            var result = await customerService.GetAllCustomersAsync();
            return result.Match(
                success => Results.Ok(success),
                failure => Results.BadRequest(failure)
            );
        });

        group.MapGet("/{id}", async (Guid id, ICustomerService customerService) =>
        {
            var result = await customerService.GetCustomerByIdAsync(id);
            return result.Match(
                success => Results.Ok(success),
                failure => Results.NotFound(failure)
            );
        });

        group.MapPost("/", async (CustomerDto customer, ICustomerService customerService) =>
        {
            return customer.ToCustomer()
                .Map(c => customerService.CreateCustomerAsync(c))
                .Match(
                    success => Results.Created($"/api/customers/{success.Id}", success.Id),
                    failure => Results.BadRequest(failure)
                );
        });

        group.MapPut("/{id}", async (Guid id, CustomerDto customer, ICustomerService customerService) =>
        {
            return customer.ToCustomer()
                .Map(c => customerService.UpdateCustomerAsync(c))
                .Match(
                    success => Results.Ok(success.Id),
                    failure => Results.BadRequest(failure)
                );
        });

        group.MapDelete("/{id}", async (Guid id, ICustomerService customerService) =>
        {
            var result = await customerService.DeleteCustomerAsync(id);
            return result.Match(
                success => Results.NoContent(),
                failure => Results.BadRequest(failure)
            );
        });
    }
}