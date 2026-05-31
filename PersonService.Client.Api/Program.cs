using PersonService.Client.Api.Services;
using PersonService.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the PersonService client with proper gRPC configuration

var url = builder.Configuration["GrpcSettings:PersonServiceUrl"];
builder.Services.AddGrpcClient<PersonCrudService.PersonCrudServiceClient>(options =>
{
    options.Address = new Uri(url!);
});

builder.Services.AddSingleton<IPersonGrpcClientService, PersonGrpcClientService>();
builder.Services.AddScoped<CreatePersonService>();
builder.Services.AddScoped<UpdatePersonService>();
builder.Services.AddScoped<DeletePersonService>();
builder.Services.AddScoped<GetPersonService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
