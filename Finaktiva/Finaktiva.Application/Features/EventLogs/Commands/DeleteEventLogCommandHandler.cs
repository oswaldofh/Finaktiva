using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Features.EventLogs.Commands
{
    public class DeleteEventLogCommandHandler : IRequestHandler<DeleteEventLogCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteEventLogCommandHandler> _logger;

        public DeleteEventLogCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteEventLogCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Response<bool>> Handle(DeleteEventLogCommand request, CancellationToken cancellationToken)
        {
            var name = request.GetType().Name;

            try
            {
                var reventLog = await _unitOfWork.Repository<EventLog>().GetByIdAsync(request.Id);

                if (reventLog is null)
                {
                    return Response<bool>.NoFoundResponse(
                       message: $"No se encontro un registro con el id {request.Id}",
                       statusCode: StatusCodes.Status404NotFound
                    );
                }

                await _unitOfWork.Repository<EventLog>().DeleteAsync(reventLog);
                _logger.LogInformation($"El comando {name} se ejecuta exitosamente");

                return Response<bool>.SuccessResponse(
                    data: true,
                    StatusCodes.Status200OK,
                    message: $"Se elimino el registro con {request.Id} exitosamente"
                );

            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<bool>.ErrorResponse(
                    $"Acceso no autorizado: {ex.Message}",
                    StatusCodes.Status401Unauthorized
                );
            }
            catch (Exception ex)
            {
                return Response<bool>.ErrorResponse(
                    $"Error interno: {ex.InnerException.Message}",
                    StatusCodes.Status500InternalServerError
                );
            }

        }
    }
}
