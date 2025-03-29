using AutoMapper;
using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Application.Models.ViewModels.EventTypes;
using Finaktiva.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Features.EventTypes.Commands
{
    public class UpdateEventTypeCommandHandler : IRequestHandler<UpdateEventTypeCommand, Response<EventTypeVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateEventTypeCommandHandler> _logger;

        public UpdateEventTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateEventTypeCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Response<EventTypeVm>> Handle(UpdateEventTypeCommand request, CancellationToken cancellationToken)
        {
            var name = request.GetType().Name;

            try
            {
                var eventType = await _unitOfWork.Repository<EventType>().GetFirstOrDefaultAsync(x => x.Id == request.Id);
                if (eventType is null)
                {
                    return Response<EventTypeVm>.NoFoundResponse(
                      message: $"No se encontro un registro con el id {request.Id}",
                      statusCode: StatusCodes.Status404NotFound
                   );
                }

                eventType.Name = request.Name;
                eventType.IsActive = request.IsActive;

                await _unitOfWork.Complete();

                _logger.LogInformation($"El comando {name} se ejecuta exitosamente");

                var reventLogVm = _mapper.Map<EventTypeVm>(eventType);

                return Response<EventTypeVm>.SuccessResponse(
                    data: reventLogVm,
                    message: "Registro actualizado exitosamente"
                );

            }
            catch (UnauthorizedAccessException ex)
            {
                var exception = new ExcepcionLog
                {
                    Date = DateTime.UtcNow,
                    Name = name,
                    Description = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.StackTrace
                };
                await _unitOfWork.Repository<ExcepcionLog>().AddAsync(exception);
                return Response<EventTypeVm>.ErrorResponse(
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
                return Response<EventTypeVm>.ErrorResponse(
                    $"Error interno: {ex.InnerException.Message}",
                    StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}
