using System;
using PersonService.Domain.ValueObjects;
using FluentAssertions;
using Xunit;
using PersonService.Domain.Exceptions;

namespace PersonService.Tests.Domain
{
    public class NationalCodeTests
    {
        [Fact]
        public void Ctor_ValidCode_ShouldSetValue()
        {
            // Arrange
            var code = "1234567890";

            // Act
            var nationalCode = new NationalCode(code);

            // Assert
            nationalCode.Value.Should().Be(code);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Ctor_EmptyOrWhiteSpace_ShouldThrow(string invalid)
        {
            Action act = () => new NationalCode(invalid);
            act.Should().Throw<DomainValidationException>()
                .Which
                .Errors.First().Code
                .Should()
                .Be(DomainErrorCodes.EmptyNationalCode);
        }

        [Theory]
        [InlineData("123456789")]
        [InlineData("abcdefghij")]
        [InlineData("12345abcde")]
        public void Ctor_InvalidFormat_ShouldThrow(string invalid)
        {
            Action act = () => new NationalCode(invalid);
            act.Should().Throw<DomainValidationException>()
                .Which
                .Errors.First().Code
                .Should()
                .Be(DomainErrorCodes.InvalidNationalCode);
        }

        [Fact]
        public void Equals_SameValue_ShouldReturnTrue()
        {
            var a = new NationalCode("1234567890");
            var b = new NationalCode("1234567890");

            a.Equals(b).Should().BeTrue();
            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        [Fact]
        public void Equals_DifferentValue_ShouldReturnFalse()
        {
            var a = new NationalCode("1234567890");
            var b = new NationalCode("0987654321");

            a.Equals(b).Should().BeFalse();
        }
    }
}
