using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonadicSharp;

namespace Domain.Customer.Repository;

public interface ICustomerRepository
{
    Task<Result<IEnumerable<Customer>>> GetAllAsync();
    Task<Result<Customer>> GetByIdAsync(Guid id);
    Task<Result<Customer>> AddAsync(Customer customer);
    Task<Result<Customer>> UpdateAsync(Customer customer);
    Task<Result<Unit>> DeleteAsync(Guid id);
    Task<Result<Customer>> GetByVatNumberAsync(string vatNumber);
    Task<Result<bool>> ExistsAsync(Guid id);
}
