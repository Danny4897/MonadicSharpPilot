using MonadicSharp;
using System;
using Domain.Common;
using Domain.Customer.ValueObjects;

namespace Domain.Customer;

public sealed record Customer
{
    public Guid Id { get; init; }
    public CompanyName CompanyName { get; init; }
    public VatNumber VatNumber { get; init; }
    public Email Email { get; init; }
    public Address Address { get; init; }
    public bool IsActive { get; init; }

    // Constructor per EF Core (parameterless)
    public Customer() : this(Guid.Empty, null!, null!, null!, null!, false) { }

    private Customer(
        Guid id,
        CompanyName companyName,
        VatNumber vatNumber,
        Email email,
        Address address,
        bool isActive)
    {
        Id = id;
        CompanyName = companyName;
        VatNumber = vatNumber;
        Email = email;
        Address = address;
        IsActive = isActive;
    }

    public static Result<Customer> Create(
        string companyName,
        string vatNumber,
        string email,
        Address address)
    {
        return CompanyName.Create(companyName)
            .Bind(name => VatNumber.Create(vatNumber)
            .Bind(vat => Email.Create(email)
            .Map(mail => new Customer(
                        Guid.NewGuid(),
                        name,
                        vat,
                        mail,
                        address,
                        true))));
    }

    public Result<Customer> UpdateEmail(string newEmail)
    {
        return Email.Create(newEmail)
            .Map(email => this with { Email = email });
    }

    public Result<Customer> UpdateCompanyName(string newName)
    {
        return CompanyName.Create(newName)
            .Map(name => this with { CompanyName = name });
    }

    public Result<Customer> UpdateVatNumber(string newVat)
    {
        return VatNumber.Create(newVat)
            .Map(vat => this with { VatNumber = vat });
    }

    public Customer UpdateAddress(Address newAddress) =>
        this with { Address = newAddress };

    public Customer Deactivate() =>
        this with { IsActive = false };

    public Customer Activate() =>
        this with { IsActive = true };

    public bool IsValid() =>
        IsActive &&
        !string.IsNullOrWhiteSpace(CompanyName.ToString()) &&
        !string.IsNullOrWhiteSpace(VatNumber.ToString()) &&
        !string.IsNullOrWhiteSpace(Email.ToString());
};

