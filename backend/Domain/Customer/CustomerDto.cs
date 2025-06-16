using System;
using MonadicSharp;
using Domain.Customer.ValueObjects;

namespace Domain.Customer;

public record CustomerDto(
    Guid Id,
    string CompanyName,
    string Email,
    string VatNumber,
    string Street,
    string City,
    string PostalCode,
    string Country
);

public static class CustomerDtoExtensions
{
    public static Result<Customer> ToCustomer(this CustomerDto dto) =>
        CompanyName.Create(dto.CompanyName)
            .Bind(companyName => Email.Create(dto.Email)
            .Bind(email => VatNumber.Create(dto.VatNumber)
            .Bind(vatNumber => Address.Create(dto.Street, dto.City, dto.PostalCode, dto.Country)
            .Bind(address => Customer.Create(
                dto.CompanyName,
                dto.VatNumber,
                dto.Email,
                address
            )))));
}
