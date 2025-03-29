using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;

namespace Application.Features.EventTypes.Commands
{
    public class DeleteEventTypeCommandHandler : IRequestHandler<DeleteEventTypeCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteEventTypeCommandHandler> _logger;

        public DeleteEventTypeCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteEventTypeCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Response<bool>> Handle(DeleteEventTypeCommand request, CancellationToken cancellationToken)
        {
            var name = request.GetType().Name;
            try
            {
                var eventType = await _unitOfWork.Repository<EventType>().GetByIdAsync(request.Id);

                if (eventType is null)
                {
                    return Response<bool>.NoFoundResponse(
                       message: $"No se encontro un registro con el id {request.Id}",
                       statusCode: StatusCodes.Status404NotFound
                    );
                }

                await _unitOfWork.Repository<EventType>().DeleteAsync(eventType);
                _logger.LogInformation($"El comando {name} se ejecuta exitosamente");

                return Response<bool>.SuccessResponse(
                    data: true,
                    StatusCodes.Status200OK,
                    message: $"Se elimino el registro con {request.Id} exitosamente"
                );

            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex.InnerException.Message, ex, $"El commando {name} tuvo errores");
                var exception = new ExcepcionLog
                {
                    Date = DateTime.UtcNow,
                    Name = name,
                    Description = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.StackTrace
                };
                await _unitOfWork.Repository<ExcepcionLog>().AddAsync(exception);
                return Response<bool>.ErrorResponse(
                    $"Acceso no autorizado: {ex.Message}",
                    StatusCodes.Status401Unauthorized
                );
            }
            catch (Exception ex)
            {
                /*_logger.LogError(ex.Message, ex, $"El commando {name} tuvo errores");
                var exception = new ExcepcionLog
                {
                    Date = DateTime.UtcNow,
                    Name = name,
                    Description = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.StackTrace
                };
                await _unitOfWork.Repository<ExcepcionLog>().AddAsync(exception);*/
                return Response<bool>.ErrorResponse(
                    $"Error interno: {ex.InnerException.Message}",
                    StatusCodes.Status500InternalServerError
                );
            }

        }
    }
}
