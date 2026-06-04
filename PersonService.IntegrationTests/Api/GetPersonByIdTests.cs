using FluentAssertions;
using Moq;
using PersonService.Contracts;
using System.Net;
using System.Net.Http.Json;

namespace PersonService.IntegrationTests;

public class GetPersonByIdTests : PersonsApiTestBase
{
    public GetPersonByIdTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Get_ShouldReturn200AndPerson_WhenExists()
    {
        // Arrange
        var personId = Guid.NewGuid().ToString();
        var grpcResponse = new PersonResponse
        {
            Id = personId,
            FirstName = "Alice",
            LastName = "Smith",
            BirthDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
        };

        var request = new GetPersonByIdRequest { Id = personId };

        Factory.GrpcClientMock.Setup(m => m.GetPersonByIdAsync(request))
            .ReturnsAsync(grpcResponse)
            .Verifiable();

        // Act
        var response = await Client.GetAsync($"/api/persons/{personId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var person = await response.Content.ReadFromJsonAsync<Person>();
        person.Should().NotBeNull();
        person!.Id.Should().Be(personId);
        person.FirstName.Should().Be(grpcResponse.FirstName);
        person.LastName.Should().Be(grpcResponse.LastName);

        Factory.GrpcClientMock.Verify(
            m => m.GetPersonByIdAsync(request),
            Times.Once);
    }
}