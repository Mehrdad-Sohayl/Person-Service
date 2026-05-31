using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonService.Domain.Entities;
using PersonService.Domain.Interfaces.Repositories;
using PersonService.Infrastructure.Data;
using PersonService.Infrastructure.Repositories;

namespace PersonService.Infrastructure;

public static class DependencyRegistry
{
    private static void AddService<TService, TImplementation>(this IServiceCollection services, RepositoryLifeCycle lifeCycle)
         where TService : class
        where TImplementation : class, TService
    {
        switch (lifeCycle)
        {
            case RepositoryLifeCycle.Singleton:
                services.AddSingleton<TService, TImplementation>();
                break;
            case RepositoryLifeCycle.Scoped:
                services.AddScoped<TService, TImplementation>();
                break;
            case RepositoryLifeCycle.Transient:
                services.AddTransient<TService, TImplementation>();
                break;
        }
    }

    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PersonDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }

    public static void AddWritePersonRepository(this IServiceCollection services, RepositoryLifeCycle lifeCycle) =>
        services.AddService<IWriteRepository<Person>, WritePersonRepository>(lifeCycle);

    public static void AddReadPersonRepository(this IServiceCollection services, RepositoryLifeCycle lifeCycle) =>
        services.AddService<IReadRepository<Person>, ReadPersonRepository>(lifeCycle);
}

public enum RepositoryLifeCycle
{
    Singleton = 1,
    Scoped = 2,
    Transient = 3,
}
