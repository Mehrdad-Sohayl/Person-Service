using System;
using PersonService.Domain.ValueObjects;
using FluentAssertions;
using Xunit;
using PersonService.Domain.Exceptions;

namespace PersonService.Tests.Domain
{
    public class BirthDateTests
    {
        [Fact]
        public void Ctor_ValidPastDate_ShouldSetValue()
        {
            var past = new DateTime(1990, 1, 1);
            var birth = new BirthDate(past);
            birth.Value.Should().Be(past);
        }
        [Fact]
        public void Ctor_FutureDate_ShouldThrowDomainValidationException()
        {
            // Arrange
            var future = DateTime.UtcNow.AddDays(1);

            // Act
            Action act = () => new BirthDate(future);

            // Assert
            act.Should()
               .Throw<DomainValidationException>()
               .Which
               .Errors.First().Code
               .Should().Be(DomainErrorCodes.InvalidBirthDate);
        }

        [Fact]
        public void Equals_SameValue_ShouldReturnTrue()
        {
            var d1 = new BirthDate(new DateTime(1985, 5, 20));
            var d2 = new BirthDate(new DateTime(1985, 5, 20));
            d1.Equals(d2).Should().BeTrue();
            d1.GetHashCode().Should().Be(d2.GetHashCode());
        }

        [Fact]
        public void Equals_DifferentValue_ShouldReturnFalse()
        {
            var d1 = new BirthDate(new DateTime(1985, 5, 20));
            var d2 = new BirthDate(new DateTime(1990, 7, 15));
            d1.Equals(d2).Should().BeFalse();
        }
    }
}
