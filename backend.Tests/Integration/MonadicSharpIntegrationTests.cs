using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Customer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using MonadicSharp;

namespace backend.Tests.Integration;

public class MonadicSharpIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public MonadicSharpIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateCustomer_WithValidData_ShouldReturnSuccess()
    {
        // Arrange - Test del pattern Result di MonadicSharp
        var customerDto = new CustomerDto(
            Guid.NewGuid(),
            "MonadicSharp Test Corp",
            "test@monadicsharp.com",
            "IT12345678901",
            "Via Monadic 123",
            "Milano",
            "20100",
            "Italy"
        );

        var json = JsonSerializer.Serialize(customerDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act - Test della funzionalità monadica
        var response = await _client.PostAsync("/api/customers", content);

        // Assert - Verifica che MonadicSharp gestisca correttamente il flusso
        Assert.True(response.IsSuccessStatusCode,
            "La creazione del customer dovrebbe avere successo con MonadicSharp");

        var locationHeader = response.Headers.Location?.ToString();
        Assert.NotNull(locationHeader);
        Assert.Contains("/api/customers/", locationHeader);
    }

    [Fact]
    public async Task CreateCustomer_WithInvalidEmail_ShouldReturnValidationError()
    {
        // Arrange - Test del pattern Result.Failure di MonadicSharp
        var customerDto = new CustomerDto(
            Guid.NewGuid(),
            "Test Corp",
            "invalid-email", // Email non valida per testare la validazione monadica
            "IT12345678901",
            "Via Test 123",
            "Milano",
            "20100",
            "Italy"
        );

        var json = JsonSerializer.Serialize(customerDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/customers", content);

        // Assert - MonadicSharp dovrebbe restituire un errore di validazione
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetCustomers_ShouldReturnSeededData()
    {
        // Act - Test del pattern Result.Success di MonadicSharp
        var response = await _client.GetAsync("/api/customers");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        // Verifica che ci siano i dati di test seedati
        Assert.NotEmpty(content);
        Assert.Contains("Acme Corp", content);
    }

    [Fact]
    public async Task GetCustomer_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange - Test del pattern Option.None -> Result.Failure
        var invalidId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/customers/{invalidId}");

        // Assert - MonadicSharp dovrebbe gestire correttamente il caso "not found"
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("", "test@example.com", "IT12345678901")] // CompanyName vuoto
    [InlineData("Test Corp", "", "IT12345678901")] // Email vuota
    [InlineData("Test Corp", "test@example.com", "")] // VatNumber vuoto
    public async Task CreateCustomer_WithMissingRequiredFields_ShouldReturnBadRequest(
        string companyName, string email, string vatNumber)
    {
        // Arrange - Test delle validazioni monadic chain
        var customerDto = new CustomerDto(
            Guid.NewGuid(),
            companyName,
            email,
            vatNumber,
            "Via Test 123",
            "Milano",
            "20100",
            "Italy"
        );

        var json = JsonSerializer.Serialize(customerDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/customers", content);

        // Assert - MonadicSharp dovrebbe intercettare le validazioni nella chain
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}
