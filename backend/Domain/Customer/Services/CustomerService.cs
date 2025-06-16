using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonadicSharp;
using Domain.Customer.Repository;

namespace Domain.Customer.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<IEnumerable<Customer>>> GetAllCustomersAsync()
    {
        return (await _customerRepository.GetAllAsync())
        .Match(
            success => success is null || !success.Any()
                ? Result<IEnumerable<Customer>>.Success(Enumerable.Empty<Customer>())
                : Result<IEnumerable<Customer>>.Success(success),
            failure => Result<IEnumerable<Customer>>.Failure($"Error retrieving customers: {failure}")
        );
    }

    public async Task<Result<Customer>> GetCustomerByIdAsync(Guid id)
    {
        return (await _customerRepository.GetByIdAsync(id))
        .Match(
            success => success is null
                ? Result<Customer>.Failure($"Customer with ID {id} not found")
                : Result<Customer>.Success(success),
            failure => Result<Customer>.Failure($"Error retrieving customer: {failure}")
        );
    }

    public async Task<Result<Customer>> CreateCustomerAsync(Customer customer)
    {
        return (await _customerRepository.AddAsync(customer))
        .Match(
            success => Result<Customer>.Success(success),
            failure => Result<Customer>.Failure($"Error creating customer: {failure}")
        );
    }

    public async Task<Result<Customer>> UpdateCustomerAsync(Customer customer)
    {
        var existingCustomerResult = await _customerRepository.GetByIdAsync(customer.Id);

        return await existingCustomerResult.Match(
            async existingCustomer => existingCustomer is null
                ? Result<Customer>.Failure($"Customer with ID {customer.Id} not found")
                : (await _customerRepository.UpdateAsync(customer))
                    .Match(
                        success => Result<Customer>.Success(success),
                        failure => Result<Customer>.Failure($"Error updating customer: {failure}")
                    ),
            failure => Task.FromResult(Result<Customer>.Failure($"Error retrieving customer: {failure}"))
        );
    }

    public async Task<Result<bool>> DeleteCustomerAsync(Guid id)
    {
        var existingCustomerResult = await _customerRepository.GetByIdAsync(id);

        return await existingCustomerResult.Match(
            async existingCustomer => existingCustomer is null
                ? Result<bool>.Failure($"Customer with ID {id} not found")
                : (await _customerRepository.DeleteAsync(id))
                    .Match(
                        _ => Result<bool>.Success(true),
                        failure => Result<bool>.Failure($"Error deleting customer: {failure}")
                    ),
            failure => Task.FromResult(Result<bool>.Failure($"Error retrieving customer: {failure}"))
        );
    }
}