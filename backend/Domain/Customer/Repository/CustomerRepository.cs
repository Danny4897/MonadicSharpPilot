using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Infrastructure.Repository;
using MonadicSharp;
using static MonadicSharp.Result;
using static MonadicSharp.Option;

namespace Domain.Customer.Repository;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository, Domain.Customer.ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context)
        : base(context, context.Customers)
    {
    }

    private async Task<Result<T>> SaveChangesAsync<T>(T entity) =>
        await TryAsync(async () =>
        {
            await _context.SaveChangesAsync();
            return entity;
        });

    private async Task<Result<T>> FindEntityAsync<T>(DbSet<T> dbSet, Guid id) where T : class =>
        await TryAsync(async () =>
        {
            var entity = await dbSet.FindAsync(id);
            return Some(entity)
                .Map(e => e)
                .Match(
                    e => e,
                    () => throw new Exception($"Entity with ID {id} not found")
                );
        });

    private async Task<Result<IEnumerable<T>>> GetAllEntitiesAsync<T>(DbSet<T> dbSet) where T : class =>
        await TryAsync(async () =>
        {
            var entities = await dbSet.ToListAsync();
            return entities as IEnumerable<T>;
        });

    private async Task<Result<T>> AddEntityAsync<T>(DbSet<T> dbSet, T entity) where T : class =>
        await TryAsync(async () =>
        {
            await dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        });

    private async Task<Result<T>> UpdateEntityAsync<T>(DbSet<T> dbSet, T entity) where T : class =>
        await TryAsync(async () =>
        {
            dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        });

    private async Task<Result<Unit>> DeleteEntityAsync<T>(DbSet<T> dbSet, Guid id) where T : class =>
        await TryAsync(async () =>
        {
            var entity = await dbSet.FindAsync(id);
            return Some(entity)
                .Map(e =>
                {
                    dbSet.Remove(e);
                    _context.SaveChangesAsync().Wait();
                    return Unit.Value;
                })
                .Match(
                    _ => Unit.Value,
                    () => throw new Exception($"Entity with ID {id} not found")
                );
        });

    public async Task<Result<IEnumerable<Customer>>> GetAllAsync() =>
        await GetAllEntitiesAsync();

    public async Task<Result<Customer>> GetByIdAsync(Guid id) =>
        await FindEntityAsync(id);

    public async Task<Result<Customer>> AddAsync(Customer customer) =>
        await Some(customer)
            .Match(
                c => Some(c.CompanyName)
                    .Match(
                        _ => Some(c.Email)
                            .Match(
                                _ => AddEntityAsync(c),
                                () => Task.FromResult(Failure<Customer>("Email is required"))),
                        () => Task.FromResult(Failure<Customer>("Company name is required"))),
                () => Task.FromResult(Failure<Customer>("Customer cannot be null")));

    public async Task<Result<Customer>> UpdateAsync(Customer customer) =>
        await Some(customer)
            .Match(
                c => Some(c.CompanyName)
                    .Match(
                        _ => Some(c.Email)
                            .Match(
                                _ => UpdateEntityAsync(c),
                                () => Task.FromResult(Failure<Customer>("Email is required"))),
                        () => Task.FromResult(Failure<Customer>("Company name is required"))),
                () => Task.FromResult(Failure<Customer>("Customer cannot be null")));

    public async Task<Result<Unit>> DeleteAsync(Guid id) =>
        await DeleteEntityAsync(id);

    public async Task<Result<Customer>> GetByVatNumberAsync(string vatNumber) =>
        await Some(vatNumber)
            .Match(
                async vn =>
                {
                    var customer = await _dbSet
                        .FirstOrDefaultAsync(c => c.VatNumber.ToString() == vn);
                    return Some(customer)
                        .Match(
                            c => Success(c),
                            () => Failure<Customer>($"Customer with VAT number {vn} not found"));
                },
                () => Task.FromResult(Failure<Customer>("VAT number is required")));

    public async Task<Result<bool>> ExistsAsync(Guid id) =>
        (await TryAsync(async () =>
            await _dbSet.AnyAsync(c => c.Id == id)))
            .Match(
                exists => Success(exists),
                error => Failure<bool>(error));
}