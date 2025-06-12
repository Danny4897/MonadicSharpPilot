using System.Collections.Generic;
using System.Threading.Tasks;
using FunctionalSharp;
using MyFullstackApp.Backend.Domain.Shared;

namespace MyFullstackApp.Backend.Domain.Shared;

/// <summary>
/// Base repository interface with monadic operations
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
/// <typeparam name="TId">Id type</typeparam>
public interface IRepository<TEntity, TId> where TEntity : Entity
{
    /// <summary>
    /// Finds an entity by id using Option pattern
    /// </summary>
    /// <param name="id">Entity id</param>
    /// <returns>Option containing the entity or None</returns>
    Task<Option<TEntity>> FindByIdAsync(TId id);

    /// <summary>
    /// Gets all entities
    /// </summary>
    /// <returns>Collection of entities</returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Adds an entity (simplified version until MonadicSharp 1.1)
    /// TODO: Will use Try pattern when available in MonadicSharp 1.1
    /// </summary>
    /// <param name="entity">Entity to add</param>
    /// <returns>Result containing the added entity</returns>
    Task<OperationResult<TEntity>> AddAsync(TEntity entity);

    /// <summary>
    /// Updates an entity (simplified version until MonadicSharp 1.1)
    /// TODO: Will use Either pattern when available in MonadicSharp 1.1
    /// </summary>
    /// <param name="entity">Entity to update</param>
    /// <returns>Result containing validation errors or updated entity</returns>
    Task<OperationResult<TEntity>> UpdateAsync(TEntity entity);    /// <summary>
                                                                   /// Deletes an entity by id (simplified version until MonadicSharp 1.1)
                                                                   /// TODO: Will use Try pattern when available in MonadicSharp 1.1
                                                                   /// </summary>
                                                                   /// <param name="id">Entity id</param>
                                                                   /// <returns>Result indicating success or failure</returns>
    Task<OperationResult<Unit>> DeleteAsync(TId id);

    /// <summary>
    /// Checks if entity exists
    /// </summary>
    /// <param name="id">Entity id</param>
    /// <returns>Boolean indicating existence</returns>
    Task<bool> ExistsAsync(TId id);
}

/// <summary>
/// Unit type for operations that don't return a value
/// </summary>
public struct Unit
{
    public static Unit Value => new();
}
