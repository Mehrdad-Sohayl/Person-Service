using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Moq;
using PersonService.Client.Api.Models;
using PersonService.Contracts;
using System.Net;
using System.Net.Http.Json;

namespace PersonService.IntegrationTests;

public class UpdatePersonTests : PersonsApiTestBase
{
    public UpdatePersonTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task UpdateFirstName_ShouldReturn200AndUpdatedPerson_WhenValid()
    {
        // Arrange
        var request = new UpdatePersonApiRequest
        (
            Id: Guid.NewGuid().ToString(),
            FirstName: "NewFirst",
            LastName: null,
            BirthDate: null
        );

        var grpcResponse = new PersonResponse
        {
            Id = request.Id,
            FirstName = request.FirstName == null ? "" : request.FirstName,
            LastName = request.LastName == null ? "" : request.LastName,
            BirthDate = request.BirthDate == null ? DateTime.UtcNow.ToTimestamp() : request.BirthDate.Value.ToTimestamp()
        };

        Factory.GrpcClientMock.Setup(m => m.UpdateFirstNameAsync(It.IsAny<UpdateFirstNameRequest>()))
            .ReturnsAsync(grpcResponse).Verifiable();

        // Act
        var content = JsonContent.Create(request);
        var response = await Client.PutAsync("/api/persons/FirstName", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var person = await response.Content.ReadFromJsonAsync<Person>();
        person.Should().NotBeNull();
        person!.Id.Should().Be(request.Id);

        Factory.GrpcClientMock.Verify(m => m.UpdateFirstNameAsync(It.IsAny<UpdateFirstNameRequest>()), Times.Once);
    }

    [Fact]
    public async Task UpdateLastName_ShouldReturn200AndUpdatedPerson_WhenValid()
    {
        // Arrange
        var request = new UpdatePersonApiRequest
        (
            Id: Guid.NewGuid().ToString(),
            FirstName: null,
            LastName: "NewLast",
            BirthDate: null
        );

        var grpcResponse = new PersonResponse
        {
            Id = request.Id,
            FirstName = request.FirstName == null ? "" : request.FirstName,
            LastName = request.LastName == null ? "" : request.LastName,
            BirthDate = request.BirthDate == null ? DateTime.UtcNow.ToTimestamp() : request.BirthDate.Value.ToTimestamp()
        };

        Factory.GrpcClientMock.Setup(m => m.UpdateLastNameAsync(It.IsAny<UpdateLastNameRequest>()))
            .ReturnsAsync(grpcResponse).Verifiable();

        // Act
        var content = JsonContent.Create(request);
        var response = await Client.PutAsync("/api/persons/LastName", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var person = await response.Content.ReadFromJsonAsync<Person>();
        person.Should().NotBeNull();
        person!.Id.Should().Be(request.Id);

        Factory.GrpcClientMock.Verify(m => m.UpdateLastNameAsync(It.IsAny<UpdateLastNameRequest>()), Times.Once);
    }
    [Fact]
    public async Task UpdateBirthDate_ShouldReturn200AndUpdatedPerson_WhenValid()
    {
        // Arrange
        var request = new UpdatePersonApiRequest
        (
            Id: Guid.NewGuid().ToString(),
            FirstName: null,
            LastName: null,
            BirthDate: DateTime.UtcNow.AddYears(-30)
        );

        var grpcResponse = new PersonResponse
        {
            Id = request.Id,
            FirstName = request.FirstName == null ? "" : request.FirstName,
            LastName = request.LastName == null ? "" : request.LastName,
            BirthDate = request.BirthDate == null ? DateTime.UtcNow.ToTimestamp() : request.BirthDate.Value.ToTimestamp()
        };

        Factory.GrpcClientMock.Setup(m => m.UpdateBirthDateAsync(It.IsAny<UpdateBirthDateRequest>()))
            .ReturnsAsync(grpcResponse).Verifiable();

        // Act
        var content = JsonContent.Create(request);
        var response = await Client.PutAsync("/api/persons/BirthDate", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var person = await response.Content.ReadFromJsonAsync<Person>();
        person.Should().NotBeNull();
        person!.Id.Should().Be(request.Id);

        Factory.GrpcClientMock.Verify(m => m.UpdateBirthDateAsync(It.IsAny<UpdateBirthDateRequest>()), Times.Once);
    }
}
