using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using PersonService.Application.Commands;
using PersonService.Application.Handlers.Commands;
using PersonService.Domain.Entities;
using PersonService.Domain.Factories;
using PersonService.Domain.Interfaces.Repositories;
using FluentAssertions;
using Xunit;

namespace PersonService.Tests.Application
{
    public class CreatePersonCommandHandlerTests
    {
        private readonly Mock<IWriteRepository<Person>> _repoMock;
        private readonly CreatePersonCommandHandler _handler;

        public CreatePersonCommandHandlerTests()
        {
            _repoMock = new Mock<IWriteRepository<Person>>();
            _handler = new CreatePersonCommandHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldAddAndReturnEntity()
        {
            // Arrange
            var command = new CreatePersonCommand("John", "Doe", "1234567890", new DateTime(1990,1,1));

            Person? capturedEntity = null;
            _repoMock.Setup(r => r.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Person e, CancellationToken _) => e)
                .Callback<Person, CancellationToken>((e, _) => capturedEntity = e);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.FirstName.Value.Should().Be("John");
            result.LastName.Value.Should().Be("Doe");
            result.NationalCode.Value.Should().Be("1234567890");
            result.BirthDate.Value.Should().Be(new DateTime(1990,1,1));

            capturedEntity.Should().NotBeNull();
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
