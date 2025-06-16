using MonadicSharp;

public sealed record BusinessAddress
{
    private readonly string _value;

    private BusinessAddress(string value) => _value = value;

    public static Option<BusinessAddress> Create(string value)
    => ValidateNotEmpty(value)
    .Bind(ValidateLenght)
    .Map(value => new BusinessAddress(value));

    public static Option<string> ValidateNotEmpty(string value)
    => string.IsNullOrWhiteSpace(value)
    ? Option<string>.None
    : Option<string>.Some(value.Trim());

    public static Option<string> ValidateLenght(string value)
    => value.Length > 2 && value.Length < 100
    ? Option<string>.Some(value)
    : Option<string>.None;

    public static implicit operator string(BusinessAddress businessAddress) => businessAddress._value;
    public override string ToString() => _value;
}