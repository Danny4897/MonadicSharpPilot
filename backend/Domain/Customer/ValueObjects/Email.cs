using MonadicSharp;
using System.Text.RegularExpressions;

namespace Domain.Customer.ValueObjects;

public sealed record Email
{
    private readonly string _value;
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    private Email(string value) => _value = value;

    public static Result<Email> Create(string value)
        => ValidateNotEmpty(value)
            .Bind(ValidateFormat)
            .Map(value => new Email(value));

    public static Result<string> ValidateNotEmpty(string value)
        => string.IsNullOrWhiteSpace(value)
            ? Result<string>.Failure("Email cannot be empty")
            : Result<string>.Success(value.Trim());

    public static Result<string> ValidateFormat(string value)
        => EmailRegex.IsMatch(value)
            ? Result<string>.Success(value)
            : Result<string>.Failure("Invalid email format");

    public override string ToString() => _value;
}