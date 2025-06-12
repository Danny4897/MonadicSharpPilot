using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FunctionalSharp;

namespace Domain.Customer.Services;

public interface ICustomerService
{
    Task<Result<IEnumerable<Customer>>> GetAllCustomersAsync();
    Task<Result<Customer>> GetCustomerByIdAsync(Guid id);
    Task<Result<Customer>> CreateCustomerAsync(Customer customer);
    Task<Result<Customer>> UpdateCustomerAsync(Customer customer);
    Task<Result<bool>> DeleteCustomerAsync(Guid id);
}