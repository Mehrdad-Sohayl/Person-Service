// PersonService.IntegrationTests/PersonsControllerTests.cs
using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;

namespace PersonService.IntegrationTests;

/// <summary>
/// Base test class that provides a HttpClient and the gRPC mock.
/// </summary>
public abstract class PersonsApiTestBase : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly CustomWebApplicationFactory Factory;
    protected readonly HttpClient Client;

    protected PersonsApiTestBase(CustomWebApplicationFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient(); // default base address is /api/persons
    }
}