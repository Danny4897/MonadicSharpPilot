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

            // Configure properties with value object conversions
            entity.Property(e => e.CompanyName)
                .HasConversion(
                    v => v.ToString(),
                    v => CompanyName.Create(v).Value)
                .HasMaxLength(255);

            entity.Property(e => e.Email)
                .HasConversion(
                    v => v.ToString(),
                    v => Email.Create(v).Value)
                .HasMaxLength(255);

            entity.Property(e => e.VatNumber)
                .HasConversion(
                    v => v.ToString(),
                    v => VatNumber.Create(v).Value)
                .HasMaxLength(50);

            entity.Property(e => e.IsActive);

            // Configure Address as owned entity
            entity.OwnsOne(e => e.Address, address =>
            {
                address.Property(a => a.Street)
                    .HasColumnName("Address_Street")
                    .HasMaxLength(255);
                address.Property(a => a.City)
                    .HasColumnName("Address_City")
                    .HasMaxLength(100);
                address.Property(a => a.PostalCode)
                    .HasColumnName("Address_PostalCode")
                    .HasMaxLength(20);
                address.Property(a => a.Country)
                    .HasColumnName("Address_Country")
                    .HasMaxLength(100);
            });

            // Configure table name
            entity.ToTable("Customers");
        });
    }
}