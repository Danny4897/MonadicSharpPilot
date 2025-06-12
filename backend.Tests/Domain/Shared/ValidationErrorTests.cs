using FluentAssertions;
using MyFullstackApp.Backend.Domain.Shared;

namespace backend.Tests.Domain.Shared;

public class ValidationErrorTests
{
    [Fact]
    public void ValidationError_Should_Create_With_All_Properties()
    {
        // Arrange
        var message = "Test error message";
        var propertyName = "TestProperty";
        var code = "TEST_ERROR";

        // Act
        var error = new ValidationError(message, propertyName, code);

        // Assert
        error.Message.Should().Be(message);
        error.PropertyName.Should().Be(propertyName);
        error.Code.Should().Be(code);
    }

    [Fact]
    public void ValidationError_Create_Static_Method_Should_Work()
    {
        // Arrange
        var message = "Test error";
        var propertyName = "Property";
        var code = "CODE";

        // Act
        var error = ValidationError.Create(message, propertyName, code);

        // Assert
        error.Message.Should().Be(message);
        error.PropertyName.Should().Be(propertyName);
        error.Code.Should().Be(code);
    }

    [Fact]
    public void ValidationErrors_Should_Collect_Multiple_Errors()
    {
        // Arrange
        var errors = ValidationErrors.Empty();
        var error1 = ValidationError.Create("Error 1", "Prop1", "ERR1");
        var error2 = ValidationError.Create("Error 2", "Prop2", "ERR2");

        // Act
        errors.Add(error1).Add(error2);

        // Assert
        errors.HasErrors.Should().BeTrue();
        errors.Errors.Should().HaveCount(2);
        errors.Errors.Should().Contain(error1);
        errors.Errors.Should().Contain(error2);
    }

    [Fact]
    public void ValidationErrors_Empty_Should_Have_No_Errors()
    {
        // Act
        var errors = ValidationErrors.Empty();

        // Assert
        errors.HasErrors.Should().BeFalse();
        errors.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ValidationErrors_ToString_Should_Format_Properly()
    {
        // Arrange
        var errors = ValidationErrors.Empty()
            .Add("First error", "Prop1", "ERR1")
            .Add("Second error", "Prop2", "ERR2");

        // Act
        var result = errors.ToString();

        // Assert
        result.Should().Contain("ERR1: First error (Prop1)");
        result.Should().Contain("ERR2: Second error (Prop2)");
        result.Should().Contain(";");
    }
}
