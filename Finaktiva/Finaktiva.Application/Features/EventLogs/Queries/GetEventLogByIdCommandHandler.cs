using AutoMapper;
using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Application.Models.ViewModels.EventLogs;
using Finaktiva.Application.Specifications;
using Finaktiva.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Features.EventLogs.Queries
{
    public class GetEventLogByIdCommandHandler : IRequestHandler<GetEventLogByIdCommand, Response<EventLogVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEventLogByIdCommandHandler> _logger;
        public GetEventLogByIdCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetEventLogByIdCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Response<EventLogVm>> Handle(GetEventLogByIdCommand request, CancellationToken cancellationToken)
        {
            var name = request.GetType().Name;
            var spec = new EventLogSpecification();
            try
            {
                var reventLog = await _unitOfWork.Repository<EventLog>().GetByIdWithSpec(spec);
                _logger.LogInformation($"El comando {name} se ejecuta exitosamente");

                if(reventLog is null)
                {
                    return Response<EventLogVm>.NoFoundResponse(
                       message: $"No se encontro un registro con el id {request.Id}",
                       statusCode: StatusCodes.Status404NotFound
                    );
                }
              
                var reventLogVm =  _mapper.Map<EventLogVm>(reventLog);

                return Response<EventLogVm>.SuccessResponse(reventLogVm);

            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex.Message, ex, $"El commando {name} tuvo errores");
                var exception = new ExcepcionLog
                {
                    Date = DateTime.UtcNow,
                    Name = name,
                    Description = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.StackTrace
                };
                await _unitOfWork.Repository<ExcepcionLog>().AddAsync(exception);
                return Response<EventLogVm>.ErrorResponse(
                    $"Acceso no autorizado: {ex.Message}",
                    StatusCodes.Status401Unauthorized
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex, $"El commando {name} tuvo errores");
                var exception = new ExcepcionLog
                {
                    Date = DateTime.UtcNow,
                    Name = name,
                    Description = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.StackTrace
                };
                await _unitOfWork.Repository<ExcepcionLog>().AddAsync(exception);
                return Response<EventLogVm>.ErrorResponse(
                    $"Error interno: {ex.InnerException.Message}",
                    StatusCodes.Status500InternalServerError
                );
            }

        }
    }
}
