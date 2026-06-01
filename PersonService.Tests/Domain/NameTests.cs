using System;
using PersonService.Domain.ValueObjects;
using FluentAssertions;
using Xunit;
using PersonService.Domain.Exceptions;

namespace PersonService.Tests.Domain
{
    public class NameTests
    {
        [Fact]
        public void Ctor_ValidValue_ShouldSet()
        {
            var name = new Name("John Doe");
            name.Value.Should().Be("John Doe");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Ctor_Invalid_ShouldThrow(string invalid)
        {
            Action act = () => new Name(invalid);
            act.Should().Throw<DomainValidationException>()
                .Which
                .Errors.First().Code
                .Should().Be(DomainErrorCodes.EmptyName);
        }

        [Fact]
        public void Equals_SameValue_ShouldReturnTrue()
        {
            var a = new Name("Alice");
            var b = new Name("Alice");
            a.Equals(b).Should().BeTrue();
            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        [Fact]
        public void Equals_DifferentValue_ShouldReturnFalse()
        {
            var a = new Name("Alice");
            var b = new Name("Bob");
            a.Equals(b).Should().BeFalse();
        }

        [Theory]
        [InlineData("abcdefghijklmnopqrstuvwxyz")] // 26 chars >20
        public void Constructor_TooLong_Throws(string name)
        {
            Action act = () => new Name(name);
            act.Should().Throw<DomainValidationException>()
            .Which
            .Errors.First().Code
            .Should().Be(DomainErrorCodes.NameLenght);
        }
    }
}
