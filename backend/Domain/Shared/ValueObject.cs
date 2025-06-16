using System;
using System.Collections.Generic;
using System.Linq;

namespace MonadicPilot.Backend.Domain.Shared;

/// <summary>
/// Base class for all value objects with monadic validation
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Validates the value object (simplified version until MonadicSharp 1.1)
    /// TODO: Will use Either pattern when available in MonadicSharp 1.1
    /// </summary>
    /// <returns>Validation result</returns>
    public abstract ValidationResult ValidateValueObject();

    /// <summary>
    /// Creates a value object safely (simplified version until MonadicSharp 1.1)
    /// TODO: Will use Try pattern when available in MonadicSharp 1.1
    /// </summary>
    /// <typeparam name="T">Type of value object</typeparam>
    /// <param name="factory">Factory function</param>
    /// <returns>OperationResult containing the created value object</returns>
    public static OperationResult<T> Create<T>(Func<T> factory) where T : ValueObject
    {
        try
        {
            var result = factory();
            var validation = result.ValidateValueObject();

            if (validation.IsValid)
                return OperationResult<T>.Success(result);
            else
                return OperationResult<T>.Failure(validation.Errors);
        }
        catch (Exception ex)
        {
            return OperationResult<T>.Failure(ex.Message);
        }
    }

    protected abstract IEnumerable<object> GetEqualityComponents(); public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    public static bool operator ==(ValueObject left, ValueObject right)
    {
        return left?.Equals(right) ?? right == null;
    }

    public static bool operator !=(ValueObject left, ValueObject right)
    {
        return !(left == right);
    }
}
