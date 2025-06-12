using System;
using Domain.Common;
using FunctionalSharp;
using Domain.Customer.ValueObjects;

namespace Domain.Customer;

public record CreateCustomerCommand
{
    public string CompanyName { get; init; }
    public string Email { get; init; }
    public string VatNumber { get; init; }
    public string Street { get; init; }
    public string City { get; init; }
    public string PostalCode { get; init; }
    public string Country { get; init; }

    private CreateCustomerCommand(
        string companyName,
        string email,
        string vatNumber,
        string street,
        string city,
        string postalCode,
        string country)
    {
        CompanyName = companyName;
        Email = email;
        VatNumber = vatNumber;
        Street = street;
        City = city;
        PostalCode = postalCode;
        Country = country;
    }

    public static Result<CreateCustomerCommand> Create(
        string companyName,
        string email,
        string vatNumber,
        string street,
        string city,
        string postalCode,
        string country)
    {
        return ValueObjects.CompanyName.Create(companyName)
            .Bind(name => ValueObjects.Email.Create(email)
                .Bind(email => ValueObjects.VatNumber.Create(vatNumber)
                    .Bind(vat => ValueObjects.Address.Create(street, city, postalCode, country)
                        .Map(address => new CreateCustomerCommand(
                            name.ToString(),
                            email.ToString(),
                            vat.ToString(),
                            address.Street,
                            address.City,
                            address.PostalCode,
                            address.Country)))));
    }
}
