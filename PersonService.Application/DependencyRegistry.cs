using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PersonService.Application.Commands;
using PersonService.Application.Handlers.Commands;
using PersonService.Application.Handlers.Queries;
using PersonService.Application.Queries;
using PersonService.Domain.Entities;

namespace PersonService.Application;

public static class DependencyRegistry
{
    private static void AddService<TService, TImplementation>(this IServiceCollection services, ApplicationLifeCycle lifeCycle)
        where TService : class
        where TImplementation : class, TService
    {
        switch (lifeCycle)
        {
            case ApplicationLifeCycle.Singleton:
                services.AddSingleton<TService, TImplementation>();
                break;
            case ApplicationLifeCycle.Scoped:
                services.AddScoped<TService, TImplementation>();
                break;
            case ApplicationLifeCycle.Transient:
                services.AddTransient<TService, TImplementation>();
                break;
        }
    }

    public static void AddCreatePersonCommandHandler(this IServiceCollection services, ApplicationLifeCycle lifeCycle) =>
        services.AddService<IRequestHandler<CreatePersonCommand, Person>, CreatePersonCommandHandler>(lifeCycle);

    public static void AddDeletePersonCommandHandler(this IServiceCollection services, ApplicationLifeCycle lifeCycle) =>
        services.AddService<IRequestHandler<DeletePersonCommand>, DeletePersonCommandHandler>(lifeCycle);

    public static void AddUpdatePersonCommandHandler(this IServiceCollection services, ApplicationLifeCycle lifeCycle) =>
        services.AddService<IRequestHandler<UpdateFirstNameCommand, Person?>, UpdateFirstNameCommandHandler>(lifeCycle);

    public static void AddFindPersonByNationalCodeQueryHandler(this IServiceCollection services, ApplicationLifeCycle lifeCycle) =>
        services.AddService<IRequestHandler<FindPersonByNationalCodeQuery, Person?>, FindPersonByNationalCodeQueryHandler>(lifeCycle);

    public static void AddGetAllPersonsQueryHandler(this IServiceCollection services, ApplicationLifeCycle lifeCycle) =>
        services.AddService<IRequestHandler<GetAllPersonsQuery, IReadOnlyList<Person>>, GetAllPersonsQueryHandler>(lifeCycle);

    public static void AddGetPersonByIdQueryHandler(this IServiceCollection services, ApplicationLifeCycle lifeCycle) =>
        services.AddService<IRequestHandler<GetPersonByIdQuery, Person?>, GetPersonByIdQueryHandler>(lifeCycle);
}

public enum ApplicationLifeCycle
{
    Singleton = 1,
    Scoped = 2,
    Transient = 3
}
