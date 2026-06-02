using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using PersonService.Application.Commands;
using PersonService.Application.Handlers.Commands;
using PersonService.Domain.Entities;
using PersonService.Domain.Interfaces.Repositories;
using FluentAssertions;
using Xunit;
using PersonService.Domain.Factories;
using PersonService.Application.Exceptions;

namespace PersonService.Tests.Application
{
    public class UpdateFirstNameCommandHandlerTests
    {
        private readonly Mock<IReadRepository<Person>> _readRepoMock = new();
        private readonly Mock<IWriteRepository<Person>> _writeRepoMock = new();
        private readonly UpdateFirstNameCommandHandler _handler;

        public UpdateFirstNameCommandHandlerTests()
        {
            _handler = new UpdateFirstNameCommandHandler(_readRepoMock.Object, _writeRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingPerson_ShouldUpdateAndReturnEntity()
        {
            // Arrange
            var id = Guid.NewGuid();
            var existingPerson = PersonFactory.Create(id, "OldFirst", "Last", "1234567890",
                                            new DateTime(1995, 5, 15));
            _readRepoMock.Setup(r => r.GetByIdAsync(id, CancellationToken.None)).ReturnsAsync(existingPerson);

            Person? capturedEntity = null;
            _writeRepoMock.Setup(w => w.UpdateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Person e, CancellationToken _) => e)
                .Callback<Person, CancellationToken>((e, _) => capturedEntity = e);

            var command = new UpdateFirstNameCommand(id, "NewFirst");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.FirstName.Value.Should().Be("NewFirst");
            result.LastName.Value.Should().Be(existingPerson.LastName.Value);
            result.NationalCode.Value.Should().Be(existingPerson.NationalCode.Value);
            result.BirthDate.Value.Should().Be(existingPerson.BirthDate.Value);

            capturedEntity.Should().NotBeNull();
            _writeRepoMock.Verify(w => w.UpdateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingPerson_ShouldThrowApplicationValidationException()
        {
            // Arrange
            var id = Guid.NewGuid();
            _readRepoMock.Setup(r => r.GetByIdAsync(id, CancellationToken.None)).ReturnsAsync((Person?)null);
            var command = new UpdateFirstNameCommand(id, "AnyName");

            // Act & Assert
            Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

            var exception = await act.Should().ThrowAsync<ApplicationValidationException>();

            exception.Which.Errors.First().Code
                .Should().Be(ApplicationErrorCodes.NotFound);
        }
    }
}
