using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using PersonService.Api.Common;
using PersonService.Application.Commands;
using PersonService.Application.Queries;
using PersonService.Domain.Exceptions;
using ProtoPerson = PersonService.Api.Person;

namespace PersonService.Api.Services;

public class GrpcPersonService : PersonService.PersonServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<GrpcPersonService> _logger;

    public GrpcPersonService(IMediator mediator, ILogger<GrpcPersonService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task<Person> Create(
        CreatePersonRequest request,
        ServerCallContext context)
    {
        try
        {

            var command = new CreatePersonCommand(
                firstName: request.Person.FirstName,
                lastName: request.Person.LastName,
                nationalCode: request.Person.NationalCode,
                birthDate: request.Person.BirthDate.ToDateTime());

            var result = await _mediator.Send(command, context.CancellationToken);

            return ToProto(result);
        }
        catch (DomainValidationException ex)
        {
            _logger.LogWarning(ex, "CreatePerson validation failed");
            throw new RpcException(new Status(StatusCode.InvalidArgument,
                string.Join("; ", ex.Errors)));
        }
        catch (Exception ex) when (!(ex is RpcException))
        {
            _logger.LogError(ex, "Unexpected error in CreatePerson");
            throw new RpcException(
                new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<Person> GetById(
        GetPersonByIdRequest request,
        ServerCallContext context)
    {
        try
        {
            var query = new GetPersonByIdQuery(id: GrpcExtensions.ToGuidOrThrow(request.Id));
            var result = await _mediator.Send(query, context.CancellationToken);

            if (result == null)
                throw new RpcException(new Status(StatusCode.NotFound,
                    $"Person with Id={request.Id} not found"));

            return ToProto(result);

        }
        catch (RpcException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetById");
            throw new RpcException(
                new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<Person> Update(
        UpdatePersonRequest request,
        ServerCallContext context)
    {
        try
        {
            var command = new UpdatePersonCommand(
                GrpcExtensions.ToGuidOrThrow(request.Person.Id),
                request.Person.FirstName,
                request.Person.LastName,
                request.Person.NationalCode,
                request.Person.BirthDate.ToDateTime()
            );

            var result = await _mediator.Send(command, context.CancellationToken);

            if (result == null)
                throw new RpcException(new Status(StatusCode.NotFound,
                    $"Person with Id={request.Person.Id} not found"));

            return ToProto(result);
        }
        catch (DomainValidationException ex)
        {
            _logger.LogWarning(ex, "UpdatePerson validation failed");
            throw new RpcException(new Status(StatusCode.InvalidArgument,
                string.Join("; ", ex.Errors)));
        }
        catch (RpcException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in UpdatePerson");
            throw new RpcException(new Status(StatusCode.Internal,
                "Internal server error"));
        }
    }

    public override async Task<Empty> Delete(
        DeletePersonRequest request,
        ServerCallContext context)
    {
        try
        {
            var command = new DeletePersonCommand
            {
                Id = GrpcExtensions.ToGuidOrThrow(request.Id)
            };

            await _mediator.Send(command, context.CancellationToken);
            return new Empty();
        }
        catch (RpcException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in DeletePerson");
            throw new RpcException(new Status(StatusCode.Internal,
                "Internal server error"));
        }
    }

    private static ProtoPerson ToProto(Domain.Entities.Person entity) => new ProtoPerson
    {
        Id = entity.Id.ToString(),
        FirstName = entity.FirstName.Value,
        LastName = entity.LastName.Value,
        NationalCode = entity.NationalCode.Value,
        BirthDate = entity.BirthDate.Value.ToTimestamp()
    };
}

