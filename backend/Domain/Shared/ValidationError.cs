using System;
using System.Collections.Generic;
using System.Linq;

namespace MonadicPilot.Backend.Domain.Shared;

/// <summary>
/// Represents validation errors in the domain
/// </summary>
public class ValidationError
{
    public string Message { get; }
    public string PropertyName { get; }
    public string Code { get; }

    public ValidationError(string message, string propertyName = "", string code = "")
    {
        Message = message;
        PropertyName = propertyName;
        Code = code;
    }

    public static ValidationError Create(string message, string propertyName = "", string code = "")
        => new(message, propertyName, code);

    public override string ToString() => $"{Code}: {Message} ({PropertyName})";
}

/// <summary>
/// Collection of validation errors
/// </summary>
public class ValidationErrors
{
    private readonly List<ValidationError> _errors = new();

    public IReadOnlyList<ValidationError> Errors => _errors.AsReadOnly();
    public bool HasErrors => _errors.Count > 0;

    public ValidationErrors Add(ValidationError error)
    {
        _errors.Add(error);
        return this;
    }

    public ValidationErrors Add(string message, string propertyName = "", string code = "")
    {
        return Add(new ValidationError(message, propertyName, code));
    }

    public static ValidationErrors Empty() => new();

    public static ValidationErrors Create(ValidationError error) => new ValidationErrors().Add(error);

    public override string ToString() => string.Join("; ", _errors.Select(e => e.ToString()));
}

/// <summary>
/// Simplified validation result until MonadicSharp 1.1 Either is available
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; }
    public ValidationErrors Errors { get; }

    private ValidationResult(bool isValid, ValidationErrors errors)
    {
        IsValid = isValid;
        Errors = errors;
    }

    public static ValidationResult Success() => new(true, ValidationErrors.Empty());

    public static ValidationResult Failure(ValidationErrors errors) => new(false, errors);

    public static ValidationResult Failure(ValidationError error) => new(false, ValidationErrors.Create(error));

    public static ValidationResult Failure(string message, string propertyName = "", string code = "")
        => new(false, ValidationErrors.Create(ValidationError.Create(message, propertyName, code)));
}

/// <summary>
/// Simplified operation result until MonadicSharp 1.1 Try/Either are available
/// Represents the result of an operation that can succeed or fail
/// </summary>
/// <typeparam name="T">Type of the success value</typeparam>
public class OperationResult<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T Value { get; }
    public string Error { get; }
    public ValidationErrors ValidationErrors { get; }

    private OperationResult(bool isSuccess, T value, string error, ValidationErrors validationErrors = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        ValidationErrors = validationErrors;
    }

    public static OperationResult<T> Success(T value)
        => new(true, value, string.Empty);

    public static OperationResult<T> Failure(string error)
        => new(false, default, error);

    public static OperationResult<T> Failure(ValidationErrors validationErrors)
        => new(false, default, validationErrors.ToString(), validationErrors);

    public static OperationResult<T> Failure(ValidationError validationError)
        => new(false, default, validationError.ToString(), ValidationErrors.Create(validationError));

    /// <summary>
    /// Maps the success value to a new type
    /// </summary>
    public OperationResult<TResult> Map<TResult>(Func<T, TResult> mapper)
    {
        if (IsFailure)
            return OperationResult<TResult>.Failure(Error); try
        {
            var result = mapper(Value);
            return OperationResult<TResult>.Success(result);
        }
        catch (Exception ex)
        {
            return OperationResult<TResult>.Failure(ex.Message);
        }
    }

    /// <summary>
    /// Chains operations together (monadic bind)
    /// </summary>
    public OperationResult<TResult> Bind<TResult>(Func<T, OperationResult<TResult>> binder)
    {
        if (IsFailure)
            return OperationResult<TResult>.Failure(Error); try
        {
            return binder(Value);
        }
        catch (Exception ex)
        {
            return OperationResult<TResult>.Failure(ex.Message);
        }
    }
}
