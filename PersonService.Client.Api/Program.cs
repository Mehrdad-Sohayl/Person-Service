using PersonService.Client.Api;
using PersonService.Client.Api.Services;
using PersonService.Contracts;
using Polly;
using Polly.Extensions.Http;

namespace PersonService.Client.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
        builder.Services.AddControllers();

        var url = builder.Configuration["GrpcSettings:PersonServiceUrl"] ?? throw new InvalidOperationException("GrpcSettings:PersonServiceUrl is required");
        var timeoutSecondsStr = builder.Configuration["GrpcSettings:TimeoutSeconds"] ?? "10";
        if (!int.TryParse(timeoutSecondsStr, out var timeoutSeconds))
            timeoutSeconds = 10;
        builder.Services.AddGrpcClient<PersonCrudService.PersonCrudServiceClient>(options =>
        {
            options.Address = new Uri(url);
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new SocketsHttpHandler
            {
                ConnectTimeout = TimeSpan.FromSeconds(timeoutSeconds)
            };
        })
        .AddPolicyHandler(GrpcPolicies.GetRetryPolicy())
        .AddPolicyHandler(GrpcPolicies.GetCircuitBreakerPolicy());

        builder.Services.AddSingleton<IPersonGrpcClientService, PersonGrpcClientService>();
        builder.Services.AddScoped<CreatePersonService>();
        builder.Services.AddScoped<UpdatePersonService>();
        builder.Services.AddScoped<DeletePersonService>();
        builder.Services.AddScoped<GetPersonService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        app.Run();
    }
}
