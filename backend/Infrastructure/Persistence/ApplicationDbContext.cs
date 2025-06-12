using System;
using Domain.Customer;
using Domain.Customer.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.CompanyName)
                .HasConversion(
                    v => v.ToString(),
                    v => CompanyName.Create(v).Value);

            entity.Property(e => e.Email)
                .HasConversion(
                    v => v.ToString(),
                    v => Email.Create(v).Value);

            entity.Property(e => e.VatNumber)
                .HasConversion(
                    v => v.ToString(),
                    v => VatNumber.Create(v).Value);

            entity.OwnsOne(e => e.Address, address =>
            {
                address.Property(a => a.Street);
                address.Property(a => a.City);
                address.Property(a => a.PostalCode);
                address.Property(a => a.Country);
            });
        });
    }
}