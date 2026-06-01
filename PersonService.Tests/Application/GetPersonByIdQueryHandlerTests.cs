using Moq;
using PersonService.Application.Handlers.Queries;
using PersonService.Application.Queries;
using PersonService.Domain.Entities;
using PersonService.Domain.Interfaces.Repositories;
using FluentAssertions;
using PersonService.Domain.Factories;

namespace PersonService.Tests.Application
{
    public class GetPersonByIdQueryHandlerTests
    {
        private readonly Mock<IReadRepository<Person>> _repoMock;
        private readonly GetPersonByIdQueryHandler _handler;

        public GetPersonByIdQueryHandlerTests()
        {
            _repoMock = new Mock<IReadRepository<Person>>();
            _handler = new GetPersonByIdQueryHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingId_ShouldReturnEntity()
        {
            var id = Guid.NewGuid();

            var person = PersonFactory.Create(
            id,
            "A",
            "B",
             "1234567890",
             new DateTime(1995, 1, 1));

            var readRepoMock = _repoMock.Setup(r => r.GetByIdAsync(id, CancellationToken.None)).ReturnsAsync((Person)person);
            var result = await _handler.Handle(new GetPersonByIdQuery(id), CancellationToken.None);

            result.Should().BeSameAs(person);
        }

        [Fact]
        public async Task Handle_NonExistingId_ShouldReturnNull()
        {
            var id = Guid.NewGuid();
            _repoMock.Setup(r => r.GetByIdAsync(id, CancellationToken.None)).ReturnsAsync((Person?)null);

            var result = await _handler.Handle(new GetPersonByIdQuery(id), CancellationToken.None);

            result.Should().BeNull();
        }
    }
}

