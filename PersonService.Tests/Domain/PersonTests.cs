using System;
using PersonService.Domain.Entities;
using PersonService.Domain.ValueObjects;
using FluentAssertions;
using Xunit;
using PersonService.Domain.Factories;

namespace PersonService.Tests.Domain
{
    public class PersonTests
    {
        private static Name CreateName(string value) => new Name(value);

        [Fact]
        public void Constructor_ValidParameters_ShouldInitializeProperties()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var person = PersonFactory.Create(
                id, "John", "Doe", "1234567890", new DateTime(1990, 1, 1));
            // Assert
            person.Id.Should().Be(id);
            person.FirstName.Value.Should().Be("John");
            person.LastName.Value.Should().Be("Doe");
            person.NationalCode.Value.Should().Be("1234567890");
            person.BirthDate.Value.Should().Be(new DateTime(1990, 1, 1));
            person.IsDeleted.Should().BeFalse();
        }

        [Fact]
        public void Constructor_NullId_ShouldAssignGeneratedGuid()
        {
            // Arrange & Act
            var person = PersonFactory.Create(
                null, "Jane", "Smith", "1234567890", new DateTime(1990, 1, 1));
            // Assert
            person.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void UpdateFirstName_ValidValues_ShouldChangePropertiesAndUpdateTimestamp()
        {
            var person = PersonFactory.Create(null,
                "Old",
                "One",
                "1234567890",
                new DateTime(1990, 1, 1));

            // Act
            person.UpdateFirstName(CreateName("New"));

            // Assert
            person.FirstName.Value.Should().Be("New");
            person.LastName.Value.Should().Be("One");
            person.BirthDate.Value.Should().Be(new DateTime(1990, 1, 1));
            person.UpdatedAt.Should().NotBeNull();
        }
        [Fact]
        public void UpdateLastName_ValidValues_ShouldChangePropertiesAndUpdateTimestamp()
        {
            var person = PersonFactory.Create(null,
                "One",
                "Old",
                "1234567890",
                new DateTime(1990, 1, 1));

            // Act
            person.UpdateLastName(CreateName("New"));

            // Assert
            person.FirstName.Value.Should().Be("One");
            person.LastName.Value.Should().Be("New");
            person.BirthDate.Value.Should().Be(new DateTime(1990, 1, 1));
            person.UpdatedAt.Should().NotBeNull();
        }

        [Fact]
        public void UpdateBirthDate_ValidValues_ShouldChangePropertiesAndUpdateTimestamp()
        {
            var person = PersonFactory.Create(null,
                "Old",
                "One",
                "1234567890",
                new DateTime(1990, 1, 1));

            var newBirthDate = new DateTime(1993, 1, 1);

            // Act
            person.UpdateBirthDate(new BirthDate(newBirthDate));

            // Assert
            person.FirstName.Value.Should().Be("Old");
            person.LastName.Value.Should().Be("One");
            person.BirthDate.Value.Should().Be(newBirthDate);
            person.UpdatedAt.Should().NotBeNull();
        }

        [Fact]
        public void MarkDeleted_ShouldSetIsDeletedAndUpdateTimestamp()
        {
            var person = PersonFactory.Create(null,
                "Mark",
                "Delete",
                "1234567890",
                new DateTime(1990, 1, 1));

            // Act
            person.MarkDeleted();

            // Assert
            person.IsDeleted.Should().BeTrue();
            person.UpdatedAt.Should().NotBeNull();
        }
    }
}

