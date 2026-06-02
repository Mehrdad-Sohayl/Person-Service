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
using PersonService.Application.Exceptions;
using PersonService.Domain.Factories;

namespace PersonService.Tests.Application
{
    public class UpdateBirthDateCommandHandlerTests
    {
        private readonly Mock<IReadRepository<Person>> _readRepoMock = new();
        private readonly Mock<IWriteRepository<Person>> _writeRepoMock = new();
        private readonly UpdateBirthDateCommandHandler _handler;

        public UpdateBirthDateCommandHandlerTests()
        {
            _handler = new UpdateBirthDateCommandHandler(_readRepoMock.Object, _writeRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingPerson_ShouldUpdateAndReturnEntity()
        {
            // Arrange
            var id = Guid.NewGuid();
            var existing = PersonFactory.Create(id, "First", "Last", "1234567890",
                new DateTime(1995, 5, 15));
            _readRepoMock.Setup(r => r.GetByIdAsync(id, CancellationToken.None)).ReturnsAsync(existing);

            Person? capturedEntity = null;
            _writeRepoMock.Setup(w => w.UpdateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Person e, CancellationToken _) => e)
                .Callback<Person, CancellationToken>((e, _) => capturedEntity = e);

            var newBirthDate = new DateTime(2000, 1, 1);
            var command = new UpdateBirthDateCommand(id, newBirthDate);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.FirstName.Value.Should().Be(existing.FirstName.Value);
            result.LastName.Value.Should().Be(existing.LastName.Value);
            result.NationalCode.Value.Should().Be(existing.NationalCode.Value);
            result.BirthDate.Value.Should().Be(newBirthDate);

            capturedEntity.Should().NotBeNull();
            _writeRepoMock.Verify(w => w.UpdateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingPerson_ShouldThrowApplicationValidationException()
        {
            // Arrange
            var id = Guid.NewGuid();
            _readRepoMock.Setup(r => r.GetByIdAsync(id, CancellationToken.None)).ReturnsAsync((Person?)null);
            var command = new UpdateBirthDateCommand(id, DateTime.Today);

            // Act & Assert
            Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

            var exception = await act.Should().ThrowAsync<ApplicationValidationException>();

            exception.Which.Errors.First().Code
                .Should().Be(ApplicationErrorCodes.NotFound);
        }
    }
}
