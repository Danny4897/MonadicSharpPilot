using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FunctionalSharp;
using static FunctionalSharp.Result;
using static FunctionalSharp.Option;

namespace Infrastructure.Repository;

public class GenericRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(DbContext context, DbSet<T> dbSet)
    {
        _context = context;
        _dbSet = dbSet;
    }

    protected async Task<Result<T>> FindEntityAsync(Guid id) =>
        (await TryAsync(async () => await _dbSet.FindAsync(id)))
            .Match(
                entity => Some(entity)
                    .Match(
                        e => Success(e),
                        () => Failure<T>($"Entity with ID {id} not found")),
                error => Failure<T>(error));

    protected async Task<Result<IEnumerable<T>>> GetAllEntitiesAsync() =>
        (await TryAsync(async () => await _dbSet.ToListAsync()))
            .Match(
                entities => Success(entities as IEnumerable<T>),
                error => Failure<IEnumerable<T>>(error));

    protected async Task<Result<T>> AddEntityAsync(T entity) =>
        (await Some(entity)
            .Match(
                async e =>
                {
                    await _dbSet.AddAsync(e);
                    await _context.SaveChangesAsync();
                    return Success(e);
                },
                () => Task.FromResult(Failure<T>("Entity cannot be null"))));

    protected async Task<Result<T>> UpdateEntityAsync(T entity) =>
        (await Some(entity)
            .Match(
                async e =>
                {
                    _dbSet.Update(e);
                    await _context.SaveChangesAsync();
                    return Success(e);
                },
                () => Task.FromResult(Failure<T>("Entity cannot be null"))));

    protected async Task<Result<Unit>> DeleteEntityAsync(Guid id) =>
        await (await FindEntityAsync(id))
            .Match(
                async entity =>
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                    return Success(Unit.Value);
                },
                error => Task.FromResult(Failure<Unit>(error)));
}