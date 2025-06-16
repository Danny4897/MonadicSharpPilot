using System;
using MonadicSharp;

namespace MonadicPilot.Backend.Domain.Shared;

/// <summary>
/// Base class for all domain entities with monadic operations support
/// </summary>
public abstract class Entity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    protected Entity(Guid id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
    }    /// <summary>
         /// Validates the entity using monadic approach
         /// TODO: Will be available in MonadicSharp 1.1
         /// </summary>
         /// <returns>Either success or validation errors</returns>
    // public abstract Either<ValidationError, Entity> Validate();

    /// <summary>
    /// Validates the entity (simplified version until MonadicSharp 1.1)
    /// </summary>
    /// <returns>Validation result</returns>
    public abstract ValidationResult ValidateEntity();    /// <summary>
                                                          /// Updates the timestamp safely
                                                          /// </summary>
                                                          /// <returns>Option containing the updated entity</returns>
    public Option<Entity> Touch()
    {
        try
        {
            UpdatedAt = DateTime.UtcNow;
            return Option<Entity>.Some(this);
        }
        catch
        {
            return Option<Entity>.None;
        }
    }
    public override bool Equals(object obj)
    {
        if (obj is not Entity other) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity left, Entity right)
    {
        return left?.Equals(right) ?? right == null;
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }
}
