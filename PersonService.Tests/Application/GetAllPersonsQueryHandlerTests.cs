using Moq;
using PersonService.Application.Handlers.Queries;
using PersonService.Application.Queries;
using PersonService.Domain.Entities;
using PersonService.Domain.Interfaces.Repositories;
using FluentAssertions;
using PersonService.Domain.Factories;
using PersonService.Application.Common;

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
            var persons = new List<Person>()
            {
                PersonFactory.Create(Guid.NewGuid(), "A", "B", "1234567890", DateTime.UtcNow),
                PersonFactory.Create(Guid.NewGuid(), "C", "D", "0123456789", DateTime.UtcNow),
                PersonFactory.Create(Guid.NewGuid(), "E", "F", "0912345678", DateTime.UtcNow)
            };

            var pagedResult = new PagedResult<Person>(
                Items: persons,
                TotalCount: persons.Count()
            );

            _repoMock.Setup(r => r.GetPagedAsync(skipExpected, pageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(persons);

            // Act
            var result = await _handler.Handle(new GetAllPersonsQuery(pageNumber, pageSize), CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(pagedResult);
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
            result.Items.Should().BeEmpty();
        }
    }
}
