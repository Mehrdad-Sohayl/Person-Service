using FluentAssertions;
using Moq;
using PersonService.Client.Api.Models;
using PersonService.Contracts;
using System.Net;
using System.Net.Http.Json;

namespace PersonService.IntegrationTests;

public class GetAllPersonsTests : PersonsApiTestBase
{
    public GetAllPersonsTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task GetAll_ShouldReturn200AndPagedResult_WhenCalled()
    {
        // Arrange
        var request = new GetAllPersonsApiRequest(PageNumber: 1, PageSize: 2);
        var grpcResponse = new Contracts.GetAllPersonsResponse
        {
            TotalCount = 5,
            Persons =
            {
                new PersonResponse { Id="1", FirstName="A", LastName="B" },
                new PersonResponse { Id="2", FirstName="C", LastName="D" }
            }
        };

        Factory.GrpcClientMock.Setup(m => m.GetAllPersonsAsync(It.IsAny<GetAllPersonsRequest>()))
            .ReturnsAsync(grpcResponse)
            .Verifiable();

        // Act
        var response = await Client.GetAsync($"/api/persons?pageNumber={request.PageNumber}&pageSize={request.PageSize}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var paged = await response.Content.ReadFromJsonAsync<PagedResult<List<PersonResponse>>>();
        paged.Should().NotBeNull();
        paged!.TotalCount.Should().Be(grpcResponse.TotalCount);
        paged.Value.Count.Should().Be(2);

        Factory.GrpcClientMock.Verify(
            m => m.GetAllPersonsAsync(It.IsAny<GetAllPersonsRequest>()),
            Times.Once);
    }
}