using Xunit;
using MonadicPilot.Backend.Domain.Customer.ValueObjects;
using MonadicPilot.Backend.Domain.Shared;

namespace MonadicPilot.Backend.Tests.Domain.Customer.ValueObjects;

public class CompanyNameTests
{
    #region Valid Cases Tests

    [Fact]
    public void Create_WithValidName_ReturnsCompanyName()
    {
        // Arrange
        var validName = "Acme Corporation";

        // Act
        var result = CompanyName.Create(validName);

        // Assert
        Assert.True(result.HasValue);
        Assert.Equal(validName, result.Value.ToString());
    }

    [Fact]
    public void Create_WithValidNameWithSpaces_ReturnsCompanyName()
    {
        // Arrange
        var validName = "Microsoft Corp Inc";

        // Act
        var result = CompanyName.Create(validName);

        // Assert
        Assert.True(result.HasValue);
        Assert.Equal(validName, result.Value.ToString());
    }

    [Fact]
    public void Create_WithValidSpecialCharacters_ReturnsCompanyName()
    {
        // Arrange
        var validName = "Tech-Solutions Ltd.";

        // Act
        var result = CompanyName.Create(validName);

        // Assert
        Assert.True(result.HasValue);
        Assert.Equal(validName, result.Value.ToString());
    }

    [Fact]
    public void Create_WithMinLength_ReturnsCompanyName()
    {
        // Arrange
        var minLengthName = "AB";  // 2 caratteri

        // Act
        var result = CompanyName.Create(minLengthName);

        // Assert
        Assert.True(result.HasValue);
        Assert.Equal(minLengthName, result.Value.ToString());
    }

    [Fact]
    public void Create_WithMaxLength_ReturnsCompanyName()
    {
        // Arrange
        var maxLengthName = new string('A', 100);  // 100 caratteri

        // Act
        var result = CompanyName.Create(maxLengthName);

        // Assert
        Assert.True(result.HasValue);
        Assert.Equal(maxLengthName, result.Value.ToString());
    }

    [Fact]
    public void Create_WithNumbersInName_ReturnsCompanyName()
    {
        // Arrange
        var nameWithNumbers = "Company123 Solutions";

        // Act
        var result = CompanyName.Create(nameWithNumbers);

        // Assert
        Assert.True(result.HasValue);
        Assert.Equal(nameWithNumbers, result.Value.ToString());
    }

    #endregion

    #region Invalid Cases Tests

    [Fact]
    public void Create_WithNull_ReturnsNone()
    {
        // Act
        var result = CompanyName.Create(null);

        // Assert
        Assert.False(result.HasValue);
    }

    [Fact]
    public void Create_WithEmptyString_ReturnsNone()
    {
        // Arrange
        var emptyName = "";

        // Act
        var result = CompanyName.Create(emptyName);

        // Assert
        Assert.False(result.HasValue);
    }

    [Fact]
    public void Create_WithWhitespaceOnly_ReturnsNone()
    {
        // Arrange
        var whitespaceName = "   ";

        // Act
        var result = CompanyName.Create(whitespaceName);

        // Assert
        Assert.False(result.HasValue);
    }

    [Fact]
    public void Create_WithTooShortName_ReturnsNone()
    {
        // Arrange
        var shortName = "A";  // 1 carattere

        // Act
        var result = CompanyName.Create(shortName);

        // Assert
        Assert.False(result.HasValue);
    }

    [Fact]
    public void Create_WithTooLongName_ReturnsNone()
    {
        // Arrange
        var longName = new string('A', 101);  // 101 caratteri

        // Act
        var result = CompanyName.Create(longName);

        // Assert
        Assert.False(result.HasValue);
    }

    [Fact]
    public void Create_WithInvalidSpecialCharacters_ReturnsNone()
    {
        // Arrange
        var invalidName = "Company@Name#Test";

        // Act
        var result = CompanyName.Create(invalidName);

        // Assert
        Assert.False(result.HasValue);
    }

    [Fact]
    public void Create_WithSymbols_ReturnsNone()
    {
        // Arrange
        var invalidName = "Test & Company $$$";

        // Act
        var result = CompanyName.Create(invalidName);

        // Assert
        Assert.False(result.HasValue);
    }

    [Fact]
    public void Create_WithUnicodeCharacters_ReturnsNone()
    {
        // Arrange
        var invalidName = "Company™ Ltd®";

        // Act
        var result = CompanyName.Create(invalidName);

        // Assert
        Assert.False(result.HasValue);
    }

    #endregion

    #region Edge Cases Tests

    [Theory]
    [InlineData("  Valid Company  ")]  // Leading/trailing spaces
    [InlineData("Valid Company")]      // No extra spaces
    public void Create_WithLeadingTrailingSpaces_TrimsAndReturnsCompanyName(string input)
    {
        // Act
        var result = CompanyName.Create(input);

        // Assert
        Assert.True(result.HasValue);
        Assert.Equal("Valid Company", result.Value.ToString());
    }

    [Theory]
    [InlineData("Tech-Corp")]          // Hyphen
    [InlineData("Corp.")]              // Dot
    [InlineData("Inc,")]               // Comma (se supportata)
    [InlineData("A B")]                // Single space
    [InlineData("Company 123")]        // Numbers with space
    public void Create_WithAllowedSpecialCharacters_ReturnsCompanyName(string validName)
    {
        // Act
        var result = CompanyName.Create(validName);

        // Assert
        Assert.True(result.HasValue);
        Assert.Equal(validName, result.Value.ToString());
    }

    [Theory]
    [InlineData("@")]
    [InlineData("#")]
    [InlineData("$")]
    [InlineData("%")]
    [InlineData("&")]
    [InlineData("*")]
    [InlineData("(")]
    [InlineData(")")]
    [InlineData("+")]
    [InlineData("=")]
    [InlineData("?")]
    public void Create_WithForbiddenCharacters_ReturnsNone(string forbiddenChar)
    {
        // Arrange
        var invalidName = $"Company{forbiddenChar}Name";

        // Act
        var result = CompanyName.Create(invalidName);

        // Assert
        Assert.False(result.HasValue);
    }

    #endregion

    #region Conversion Tests

    [Fact]
    public void ImplicitConversion_ToString_Works()
    {
        // Arrange
        var originalName = "Test Company";
        var companyName = CompanyName.Create(originalName).Value;

        // Act
        string convertedName = companyName;  // Conversione implicita

        // Assert
        Assert.Equal(originalName, convertedName);
    }

    [Fact]
    public void ToString_ReturnsOriginalValue()
    {
        // Arrange
        var originalName = "Another Company";
        var companyName = CompanyName.Create(originalName).Value;

        // Act
        var result = companyName.ToString();

        // Assert
        Assert.Equal(originalName, result);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void TwoCompanyNames_WithSameValue_AreEqual()
    {
        // Arrange
        var name = "Equal Company";
        var companyName1 = CompanyName.Create(name).Value;
        var companyName2 = CompanyName.Create(name).Value;

        // Act & Assert
        Assert.Equal(companyName1, companyName2);
        Assert.True(companyName1 == companyName2);
        Assert.False(companyName1 != companyName2);
    }

    [Fact]
    public void TwoCompanyNames_WithDifferentValues_AreNotEqual()
    {
        // Arrange
        var companyName1 = CompanyName.Create("Company One").Value;
        var companyName2 = CompanyName.Create("Company Two").Value;

        // Act & Assert
        Assert.NotEqual(companyName1, companyName2);
        Assert.False(companyName1 == companyName2);
        Assert.True(companyName1 != companyName2);
    }

    #endregion
}