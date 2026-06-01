using PersonService.Client.Api;
using PersonService.Client.Api.Services;
using PersonService.Contracts;
using Polly;
using Polly.Extensions.Http;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();

