using FunctionalSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Customer;

public interface ICustomerRepository
{
    Task<Result<Customer>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<Customer>>> GetAllAsync();
    Task<Result<Customer>> AddAsync(Customer customer);
    Task<Result<Customer>> UpdateAsync(Customer customer);
    Task<Result<Unit>> DeleteAsync(Guid id);
    Task<Result<Customer>> GetByVatNumberAsync(string vatNumber);
    Task<Result<bool>> ExistsAsync(Guid id);
}