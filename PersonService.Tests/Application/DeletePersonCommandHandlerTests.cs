using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using PersonService.Application.Commands;
using PersonService.Application.Handlers.Commands;
using PersonService.Domain.Interfaces.Repositories;
using FluentAssertions;
using Xunit;
using PersonService.Domain.Entities;

namespace PersonService.Tests.Application
{
    public class DeletePersonCommandHandlerTests
    {
        private readonly Mock<IWriteRepository<Person>> _repoMock;
        private readonly DeletePersonCommandHandler _handler;

        public DeletePersonCommandHandlerTests()
        {
            _repoMock = new Mock<IWriteRepository<Person>>();
            _handler = new DeletePersonCommandHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldInvokeDeleteAsyncWithCorrectId()
        {
            var id = Guid.NewGuid();
            _repoMock.Setup(r => r.DeleteAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await _handler.Handle(new DeletePersonCommand(id), CancellationToken.None);

            _repoMock.Verify(r => r.DeleteAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
