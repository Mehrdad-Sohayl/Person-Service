using Xunit;
using PersonService.Domain.Entities;
using PersonService.Domain.ValueObjects;
using PersonService.Domain.Exceptions;
using PersonService.Domain.Factories;

namespace PersonService.Tests
{
    public class DomainTests
    {
        [Fact]
        public void Name_Should_Validate_On_Construction()
        {
            // Arrange & Act
            var validName = new Name("John");
            var invalidName = Assert.Throws<ArgumentException>(() => new Name(""));

            // Assert
            Assert.Equal("John", validName.Value);
            Assert.Contains("Name cannot be empty", invalidName.Message);
        }

        [Fact]
        public void BirthDate_Should_Validate_On_Construction()
        {
            var today = DateTime.Today;
            var futureDate = today.AddYears(1);

            // Test future date validation
            var invalidFutureDate = Assert.Throws<ArgumentException>(() => new BirthDate(futureDate));

            // Test valid date
            var birth = new BirthDate(new DateTime(1990, 5, 20));

            // Assert
            Assert.Contains("Birth date cannot be in the future", invalidFutureDate.Message);
            Assert.Equal(new DateTime(1990, 5, 20), birth.Value);
        }

        [Fact]
        public void NationalCode_Should_Validate_On_Construction()
        {
            // Test valid national code
            var valid = new NationalCode("1234567890");
            Assert.Equal("1234567890", valid.Value);

            // Test invalid formats
            var invalidEmpty = Assert.Throws<ArgumentException>(() => new NationalCode(""));
            var invalidShort = Assert.Throws<ArgumentException>(() => new NationalCode("12345"));
            var invalidAlpha = Assert.Throws<ArgumentException>(() => new NationalCode("abcdefghij"));

            // Assert
            Assert.Contains("National code cannot be empty", invalidEmpty.Message);
            Assert.Contains("National code must be exactly 10 digits", invalidShort.Message);
            Assert.Contains("National code must be exactly 10 digits", invalidAlpha.Message);
        }

        [Fact]
        public void PersonFactory_Should_Create_Valid_Person()
        {
            // Arrange
            var name = new Name("Jane");
            var birth = new BirthDate(new DateTime(1985, 1, 15));
            var nationalCode = new NationalCode("9876543210");

            // Act
            var person = PersonFactory.Create(name.Value, name.Value, nationalCode.Value, birth.Value);

            // Assert
            Assert.NotNull(person.Id); // GUID assigned by BaseEntity
            Assert.Equal(name.Value, person.FirstName.Value);
            Assert.Equal(name.Value, person.LastName.Value);
            Assert.Equal(nationalCode.Value, person.NationalCode.Value);
            Assert.Equal(birth.Value, person.BirthDate.Value);
        }

        [Fact]
        public void Person_Should_Update_Correctly()
        {
            // Arrange
            var originalName = new Name("John");
            var newName = new Name("Jane");
            var birth = new BirthDate(new DateTime(1985, 1, 15));
            var nationalCode = new NationalCode("1234567890");

            var person = PersonFactory.Create(originalName.Value, originalName.Value, nationalCode.Value, birth.Value);

            // Act
            person.Update(newName, newName, birth);

            // Assert
            Assert.Equal(newName.Value, person.FirstName.Value);
            Assert.Equal(newName.Value, person.LastName.Value);
        }

        [Fact]
        public void Person_Should_Mark_As_Deleted()
        {
            // Arrange
            var name = new Name("John");
            var birth = new BirthDate(new DateTime(1985, 1, 15));
            var nationalCode = new NationalCode("1234567890");

            var person = PersonFactory.Create(name.Value, name.Value, nationalCode.Value, birth.Value);

            // Act
            person.MarkDeleted();

            // Assert
            Assert.True(person.IsDeleted);
        }
    }
}