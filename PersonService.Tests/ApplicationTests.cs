using Xunit;
using Moq;
using MediatR;
using PersonService.Application.Commands;
using PersonService.Application.Handlers.Commands;
using PersonService.Domain.Entities;
using PersonService.Domain.Factories;
using PersonService.Domain.ValueObjects;
using PersonService.Domain.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace PersonService.Tests
{
    public class ApplicationTests
    {
        [Fact]
        public async Task CreatePersonCommandHandler_Should_Create_And_Return_Person()
        {
            // Arrange
            var mockRepo = new Mock<IWriteRepository<Person>>();
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Person p, CancellationToken _) => p);

            var handler = new CreatePersonCommandHandler(mockRepo.Object);
            var command = new CreatePersonCommand("John", "Doe", "1234567890", new System.DateTime(1990, 1, 1));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName.Value);
            Assert.Equal("Doe", result.LastName.Value);
        }

        [Fact]
        public async Task UpdatePersonCommandHandler_Should_Update_Person()
        {
            // Arrange
            var mockRepo = new Mock<IReadRepository<Person>>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(PersonFactory.Create(
                        "John",
                        "Doe",
                        "1234567890",
                        new DateTime(1990, 1, 1)
                    ));

            var mockWriteRepo = new Mock<IWriteRepository<Person>>();
            mockWriteRepo.Setup(r => r.UpdateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Person p, CancellationToken _) => p);

            var handler = new UpdatePersonCommandHandler(mockWriteRepo.Object);
            var command = new UpdatePersonCommand(
                Guid.NewGuid(),
                "Jane",
                "Smith",
                "0987654321",
                new System.DateTime(1991, 2, 2));

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            mockWriteRepo.Verify(r => r.UpdateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}