using MonadicSharp;

namespace Domain.Customer.ValueObjects;

public sealed record Address
{
    public string Street { get; }
    public string City { get; }
    public string PostalCode { get; }
    public string Country { get; }

    private Address(string street, string city, string postalCode, string country)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
        Country = country;
    }

    public static Result<Address> Create(string street, string city, string postalCode, string country)
    {
        return ValidateStreet(street)
            .Bind(s => ValidateCity(city)
                .Bind(c => ValidatePostalCode(postalCode)
                    .Bind(p => ValidateCountry(country)
                        .Map(co => new Address(s, c, p, co)))));
    }

    private static Result<string> ValidateStreet(string street)
        => string.IsNullOrWhiteSpace(street)
            ? Result<string>.Failure("Street cannot be empty")
            : Result<string>.Success(street.Trim());

    private static Result<string> ValidateCity(string city)
        => string.IsNullOrWhiteSpace(city)
            ? Result<string>.Failure("City cannot be empty")
            : Result<string>.Success(city.Trim());

    private static Result<string> ValidatePostalCode(string postalCode)
        => string.IsNullOrWhiteSpace(postalCode)
            ? Result<string>.Failure("Postal code cannot be empty")
            : Result<string>.Success(postalCode.Trim());

    private static Result<string> ValidateCountry(string country)
        => string.IsNullOrWhiteSpace(country)
            ? Result<string>.Failure("Country cannot be empty")
            : Result<string>.Success(country.Trim());
}