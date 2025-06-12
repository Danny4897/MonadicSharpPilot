using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FunctionalSharp;

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
        .Map(customers => customers is null
            ? Result<IEnumerable<Customer>>.Failure("No customers found")
            : Result<IEnumerable<Customer>>.Success(customers))
        .Match(
            success => success,
            failure => Result<IEnumerable<Customer>>.Failure($"Error retrieving customers: {failure}")
        );
    }

    public async Task<Result<Customer>> GetCustomerByIdAsync(Guid id)
    {
        return (await _customerRepository.GetByIdAsync(id))
        .Map(customer => customer is null
            ? Result<Customer>.Failure($"Customer with ID {id} not found")
            : Result<Customer>.Success(customer))
        .Match(
            success => success,
            failure => Result<Customer>.Failure($"Error retrieving customer: {failure}")
        );
    }

    public async Task<Result<Customer>> CreateCustomerAsync(Customer customer)
    {
        return (await _customerRepository.AddAsync(customer))
        .Map(Result<Customer>.Success)
        .Match(
            success => success,
            failure => Result<Customer>.Failure($"Error creating customer: {failure}")
        );
    }

    public async Task<Result<Customer>> UpdateCustomerAsync(Customer customer)
    {
        return (await _customerRepository.GetByIdAsync(customer.Id))
        .Map(existingCustomer => existingCustomer is null
            ? Result<Customer>.Failure($"Customer with ID {customer.Id} not found")
            : Result<Customer>.Success(customer))
        .Match(
            success => success,
            failure => Result<Customer>.Failure($"Error updating customer: {failure}")
        );
    }

    public async Task<Result<bool>> DeleteCustomerAsync(Guid id)
    {
        return (await _customerRepository.GetByIdAsync(id))
            .Map(customer => customer is null
                ? Result<Customer>.Failure($"Customer with ID {id} not found")
                : Result<Customer>.Success(customer))
            .Bind(customer =>
            {
                _customerRepository.DeleteAsync(id);
                return Result<bool>.Success(true);
            })
            .Match(
                success => success,
                failure => Result<bool>.Failure($"Error deleting customer: {failure}")
            );
    }
}