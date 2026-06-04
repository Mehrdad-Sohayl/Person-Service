using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PersonService.Client.Api.Services;

namespace PersonService.IntegrationTests;

/// <summary>
/// Factory that creates an in‑memory test server for the API.
/// It replaces IPersonGrpcClientService with a Moq mock so no real gRPC call is made.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    /// <summary>Publicly exposed mock that tests can configure.</summary>
    public Mock<IPersonGrpcClientService> GrpcClientMock { get; } = new(MockBehavior.Strict);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the real implementation if it has already been added
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IPersonGrpcClientService));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Register the mock as a singleton so that all services receive the same instance
            services.AddSingleton(GrpcClientMock.Object);
        });
    }
}