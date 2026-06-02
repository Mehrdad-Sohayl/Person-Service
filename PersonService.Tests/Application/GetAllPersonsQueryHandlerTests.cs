using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using PersonService.Application.Handlers.Queries;
using PersonService.Application.Queries;
using PersonService.Domain.Entities;
using PersonService.Domain.Interfaces.Repositories;
using FluentAssertions;
using Xunit;
using PersonService.Domain.Factories;

namespace PersonService.Tests.Application
{
    public class GetAllPersonsQueryHandlerTests
    {
        private readonly Mock<IReadRepository<Person>> _repoMock = new();
        private readonly GetAllPersonsQueryHandler _handler;

        public GetAllPersonsQueryHandlerTests()
        {
            _handler = new GetAllPersonsQueryHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedListAndApplySkip()
        {
            // Arrange
            var pageNumber = 3;
            var pageSize = 10;
            var skipExpected = (pageNumber - 1) * pageSize;
            var persons = new[] { PersonFactory.Create(Guid.NewGuid(), "A", "B", "1234567890", DateTime.Today) };

            _repoMock.Setup(r => r.GetPagedAsync(skipExpected, pageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(persons);

            // Act
            var result = await _handler.Handle(new GetAllPersonsQuery(pageNumber, pageSize), CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(persons);
            _repoMock.Verify(r => r.GetPagedAsync(skipExpected, pageSize, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            var persons = Array.Empty<Person>();
            _repoMock.Setup(r => r.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(persons);

            // Act
            var result = await _handler.Handle(new GetAllPersonsQuery(1, 5), CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }
    }
}
