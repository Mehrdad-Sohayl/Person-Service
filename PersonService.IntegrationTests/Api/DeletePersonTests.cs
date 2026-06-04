using FluentAssertions;
using Moq;
using PersonService.Contracts;
using System.Net;

namespace PersonService.IntegrationTests;
public class DeletePersonTests : PersonsApiTestBase
{
    public DeletePersonTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Delete_ShouldReturn200_WhenSuccessful()
    {
        // Arrange
        var personId = Guid.NewGuid().ToString();
        var deletePersonRequest = new DeletePersonRequest
        {
            Id = personId
        };

        Factory.GrpcClientMock.Setup(m => m.DeletePersonAsync(deletePersonRequest))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var response = await Client.DeleteAsync($"/api/persons/{personId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        Factory.GrpcClientMock.Verify(
            m => m.DeletePersonAsync(deletePersonRequest),
            Times.Once);
    }
}