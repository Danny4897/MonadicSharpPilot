using System.Linq;
using FunctionalSharp;
using MyFullstackApp.Backend.Domain.Shared;

namespace Domain.Customer.ValueObjects;

public sealed record CompanyName
{
    private readonly string _value;

    private CompanyName(string value) => _value = value;

    public static Result<CompanyName> Create(string value)
        => ValidateNotEmpty(value)
            .Bind(ValidateLength)
            .Bind(ValidateFormat)
            .Map(value => new CompanyName(value));

    public static Result<string> ValidateNotEmpty(string value)
        => string.IsNullOrWhiteSpace(value)
            ? Result<string>.Failure("Company name cannot be empty")
            : Result<string>.Success(value.Trim());

    public static Result<string> ValidateLength(string value)
        => value.Length >= 2 && value.Length <= 100
            ? Result<string>.Success(value)
            : Result<string>.Failure("Company name must be between 2 and 100 characters");

    public static Result<string> ValidateFormat(string value)
        => value.All(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-' || c == '_')
            ? Result<string>.Success(value)
            : Result<string>.Failure("Company name contains invalid characters");

    public override string ToString() => _value;
}