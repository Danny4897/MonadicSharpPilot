using System.Linq;
using System.Text.RegularExpressions;
using MonadicSharp;
using MonadicPilot.Backend.Domain.Shared;

public sealed record BusinessEmail
{
    private readonly string _value;

    private BusinessEmail(string value) => _value = value;

    public static Option<BusinessEmail> Create(string value)
        => ValidateNotEmpty(value)
            .Bind(ValidateContainsAtSymbol)
            .Bind(ValidateLength)
            .Bind(ValidateBasicEmailFormat)
            .Map(value => new BusinessEmail(value));

    public static Option<string> ValidateNotEmpty(string value)
        => string.IsNullOrWhiteSpace(value)
            ? Option<string>.None
            : Option<string>.Some(value.Trim());

    public static Option<string> ValidateContainsAtSymbol(string value)
        => value.Contains('@')
            ? Option<string>.Some(value)
            : Option<string>.None;

    public static Option<string> ValidateLength(string value)
        => value.Length >= 5 && value.Length <= 254  // RFC 5321 standard
            ? Option<string>.Some(value)
            : Option<string>.None;

    public static Option<string> ValidateBasicEmailFormat(string value)
        => SplitEmailParts(value)
        .Bind(parts => ValidateLocalPart(parts.localPart)
        .Bind(_ => ValidateDomainPart(parts.domainPart))
        .Map(_ => value));

    private static Option<(string localPart, string domainPart)> SplitEmailParts(string value)
    {
        var parts = value.Split('@');
        return parts.Length == 2
            ? Option<(string, string)>.Some((parts[0], parts[1]))
            : Option<(string, string)>.None;
    }

    private static Option<string> ValidateLocalPart(string localPart)
        => (!string.IsNullOrWhiteSpace(localPart) && localPart.Length <= 64)
            ? Option<string>.Some(localPart)
            : Option<string>.None;

    private static Option<string> ValidateDomainPart(string domainPart)
        => (!string.IsNullOrWhiteSpace(domainPart) &&
            domainPart.Contains('.') &&
            !domainPart.StartsWith('.') &&
            !domainPart.EndsWith('.'))
            ? Option<string>.Some(domainPart)
            : Option<string>.None;

    public static implicit operator string(BusinessEmail businessEmail) => businessEmail._value;
    public override string ToString() => _value;
}