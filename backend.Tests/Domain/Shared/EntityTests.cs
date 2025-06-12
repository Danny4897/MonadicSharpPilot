using FluentAssertions;
using MonadicSharp;
using MyFullstackApp.Backend.Domain.Shared;

namespace backend.Tests.Domain.Shared;

public class EntityTests
{
    private class TestEntity : Entity
    {
        public string Name { get; set; } = string.Empty;

        public TestEntity() : base() { }

        public TestEntity(string name) : base()
        {
            Name = name;
        }

        public override Either<ValidationError, Entity> Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return Either.Left<ValidationError, Entity>(
                    ValidationError.Create("Name cannot be empty", nameof(Name), "EMPTY_NAME"));
            }

            return Either.Right<ValidationError, Entity>(this);
        }
    }

    [Fact]
    public void Entity_Should_Generate_Valid_Id_On_Creation()
    {
        // Arrange & Act
        var entity = new TestEntity("Test");

        // Assert
        entity.Id.Should().NotBe(Guid.Empty);
        entity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        entity.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Entity_Should_Update_Timestamp_When_Touched()
    {
        // Arrange
        var entity = new TestEntity("Test");
        var originalCreatedAt = entity.CreatedAt;

        // Act
        var result = entity.Touch();

        // Assert
        result.IsSome.Should().BeTrue();
        entity.UpdatedAt.Should().NotBeNull();
        entity.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        entity.CreatedAt.Should().Be(originalCreatedAt);
    }

    [Fact]
    public void Entity_Validate_Should_Return_Error_For_Invalid_Entity()
    {
        // Arrange
        var entity = new TestEntity(""); // Empty name

        // Act
        var result = entity.Validate();

        // Assert
        result.IsLeft.Should().BeTrue();
        result.Left.Message.Should().Be("Name cannot be empty");
        result.Left.PropertyName.Should().Be("Name");
        result.Left.Code.Should().Be("EMPTY_NAME");
    }

    [Fact]
    public void Entity_Validate_Should_Return_Success_For_Valid_Entity()
    {
        // Arrange
        var entity = new TestEntity("Valid Name");

        // Act
        var result = entity.Validate();

        // Assert
        result.IsRight.Should().BeTrue();
        result.Right.Should().Be(entity);
    }

    [Fact]
    public void Entity_Equality_Should_Work_Based_On_Id()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity("Test") { Id = id };
        var entity2 = new TestEntity("Different Name") { Id = id };
        var entity3 = new TestEntity("Test");

        // Act & Assert
        entity1.Should().Be(entity2); // Same ID
        entity1.Should().NotBe(entity3); // Different ID
        (entity1 == entity2).Should().BeTrue();
        (entity1 != entity3).Should().BeTrue();
    }
}
