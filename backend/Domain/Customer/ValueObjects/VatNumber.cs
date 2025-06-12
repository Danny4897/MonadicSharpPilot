using FunctionalSharp;

namespace Domain.Customer.ValueObjects;

public sealed record VatNumber
{
    private readonly string _value;

    private VatNumber(string value) => _value = value;

    public static Result<VatNumber> Create(string value)
        => ValidateNotEmpty(value)
            .Bind(ValidateLength)
            .Map(value => new VatNumber(value));

    public static Result<string> ValidateNotEmpty(string value)
        => string.IsNullOrWhiteSpace(value)
            ? Result<string>.Failure("VAT number cannot be empty")
            : Result<string>.Success(value.Trim());

    public static Result<string> ValidateLength(string value)
        => value.Length == 10
            ? Result<string>.Success(value)
            : Result<string>.Failure("VAT number must be exactly 10 characters");

    public override string ToString() => _value;
}