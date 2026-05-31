using PersonService.Application;
using PersonService.Infrastructure;

namespace PersonService.Api.Common;

public static class Extensions
{
    public static void SetupDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        services.AddWritePersonRepository(RepositoryLifeCycle.Scoped);
        services.AddReadPersonRepository(RepositoryLifeCycle.Scoped);

        services.AddCreatePersonCommandHandler(ApplicationLifeCycle.Scoped);
        services.AddDeletePersonCommandHandler(ApplicationLifeCycle.Scoped);
        services.AddUpdatePersonCommandHandler(ApplicationLifeCycle.Scoped);

        services.AddFindPersonByNationalCodeQueryHandler(ApplicationLifeCycle.Scoped);
        services.AddGetAllPersonsQueryHandler(ApplicationLifeCycle.Scoped);
        services.AddGetPersonByIdQueryHandler(ApplicationLifeCycle.Scoped);
    }
}
