using PersonService.Api.Common;
using PersonService.Api.Services;

namespace PersonService.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
            .ConfigureAppConfiguration((hostingContext, configuration) =>
            {
                var env = hostingContext.HostingEnvironment;

                configuration.AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .AddEnvironmentVariables();
            });

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(5001, listenOptions =>
            {
                listenOptions.UseHttps();
            });
        });

        builder.Services
            .AddGrpc(options => { options.Interceptors.Add<GrpcErrorHandlingInterceptor>(); });
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly))
        .SetupDI(builder.Configuration);

        var app = builder.Build();

        app.MapGrpcService<GrpcPersonService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}
