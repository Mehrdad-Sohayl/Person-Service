using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Moq;
using PersonService.Client.Api.Models;
using PersonService.Contracts;

namespace PersonService.IntegrationTests;

public class CreatePersonTests : PersonsApiTestBase
{
    public CreatePersonTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Create_ShouldReturn201AndCreatedPerson_WhenValid()
    {
        // Arrange
        var request = new CreatePersonApiRequest(
            FirstName: "John",
            LastName: "Doe",
            NationalCode: "1234567890",
            BirthDate: DateTime.UtcNow.AddYears(-30)
        );

        var grpcResponse = new PersonResponse
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate.ToTimestamp()
        };

        Factory.GrpcClientMock.Setup(m => m.CreatePersonAsync(
            It.IsAny<CreatePersonRequest>()))
            .ReturnsAsync(grpcResponse)
            .Verifiable();

        // Act
        var content = JsonContent.Create(request);
        var response = await Client.PostAsync("/api/persons", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await response.Content.ReadFromJsonAsync<PersonResponse>();
        responseBody.Should().NotBeNull();
        responseBody!.Id.Should().Be(grpcResponse.Id);
        responseBody.FirstName.Should().Be(request.FirstName);
        responseBody.LastName.Should().Be(request.LastName);
        responseBody.BirthDate.ToDateTime().Should()
            .BeCloseTo(request.BirthDate, TimeSpan.FromSeconds(1));

        Factory.GrpcClientMock.Verify(
            m => m.CreatePersonAsync(It.IsAny<CreatePersonRequest>()),
            Times.Once);
    }
}