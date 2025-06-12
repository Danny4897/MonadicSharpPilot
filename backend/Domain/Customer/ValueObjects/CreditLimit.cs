using FunctionalSharp;

public sealed record CreditLimit
{
    private readonly decimal _value;

    private CreditLimit(decimal value) => _value = value;

    public static Option<CreditLimit> Create(decimal value)
    => ValidateNotNegative(value)
    .Map(value => new CreditLimit(value));

    public static Option<decimal> ValidateNotNegative(decimal value)
    => value >= 0
    ? Option<decimal>.Some(value)
    : Option<decimal>.None;

    public static implicit operator decimal(CreditLimit creditLimit) => creditLimit._value;
    public override string ToString() => _value.ToString("C");
}