using System;
using Domain.Customer;
using Domain.Customer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MonadicPilot.Controllers;

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
            var customerResult = customer.ToCustomer();
            if (customerResult.IsFailure)
                return Results.BadRequest(customerResult.Error);

            var result = await customerService.CreateCustomerAsync(customerResult.Value);
            return result.Match(
                success => Results.Created($"/api/customers/{success.Id}", success),
                failure => Results.BadRequest(failure)
            );
        });

        group.MapPut("/{id}", async (Guid id, CustomerDto customer, ICustomerService customerService) =>
        {
            var customerResult = customer.ToCustomer();
            if (customerResult.IsFailure)
                return Results.BadRequest(customerResult.Error);

            var result = await customerService.UpdateCustomerAsync(customerResult.Value);
            return result.Match(
                success => Results.Ok(success),
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