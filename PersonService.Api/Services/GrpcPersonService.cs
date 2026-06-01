using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using PersonService.Api.Common;
using PersonService.Application.Commands;
using PersonService.Application.Queries;
using PersonService.Contracts;
using PersonService.Domain.Exceptions;

namespace PersonService.Api.Services;

public class GrpcPersonService : PersonCrudService.PersonCrudServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<GrpcPersonService> _logger;

    public GrpcPersonService(
        IMediator mediator,
        ILogger<GrpcPersonService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task<PersonResponse> Create(
        CreatePersonRequest request,
        ServerCallContext context)
    {
        try
        {

            var command = new CreatePersonCommand(
                firstName: request.FirstName,
                lastName: request.LastName,
                nationalCode: request.NationalCode,
                birthDate: request.BirthDate.ToDateTime().ToUniversalTime());

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

    public async override Task<PersonResponse> GetById(GetPersonByIdRequest request, ServerCallContext context)
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

    public override async Task<PersonResponse> UpdateFirstName(
        UpdateFirstNameRequest request,
        ServerCallContext context)
    {
        try
        {
            var command = new UpdateFirstNameCommand(
                GrpcExtensions.ToGuidOrThrow(request.Id),
                request.FirstName
            );

            var result = await _mediator.Send(command, context.CancellationToken);

            if (result == null)
                throw new RpcException(new Status(StatusCode.NotFound,
                    $"Person with Id={request.Id} not found"));

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

        public override async Task<PersonResponse> UpdateLastName(
        UpdateLastNameRequest request,
        ServerCallContext context)
    {
        try
        {
            var command = new UpdateLastNameCommand(
                GrpcExtensions.ToGuidOrThrow(request.Id),
                request.LastName
            );

            var result = await _mediator.Send(command, context.CancellationToken);

            if (result == null)
                throw new RpcException(new Status(StatusCode.NotFound,
                    $"Person with Id={request.Id} not found"));

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

        public override async Task<PersonResponse> UpdateBirthDate(
        UpdateBirthDateRequest request,
        ServerCallContext context)
    {
        try
        {
            var command = new UpdateBirthDateCommand(
                GrpcExtensions.ToGuidOrThrow(request.Id),
                request.BirthDate.ToDateTime().ToUniversalTime()
            );

            var result = await _mediator.Send(command, context.CancellationToken);

            if (result == null)
                throw new RpcException(new Status(StatusCode.NotFound,
                    $"Person with Id={request.Id} not found"));

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

    public override async Task<Contracts.Empty> Delete(
        DeletePersonRequest request,
        ServerCallContext context)
    {
        try
        {
            var command = new DeletePersonCommand(GrpcExtensions.ToGuidOrThrow(request.Id));

            await _mediator.Send(command, context.CancellationToken);
            return new Contracts.Empty();
        }
        catch (RpcException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in DeletePerson");
            throw new RpcException(new Status(StatusCode.Internal,
                "Internal server error"));
        }
    }

    private static PersonResponse ToProto(Domain.Entities.Person entity) => new PersonResponse
    {
        Id = entity.Id.ToString(),
        FirstName = entity.FirstName.Value,
        LastName = entity.LastName.Value,
        NationalCode = entity.NationalCode.Value,
        BirthDate = new Timestamp { Seconds = entity.BirthDate.Value.ToUniversalTime().ToTimestamp().Seconds }
    };
}
