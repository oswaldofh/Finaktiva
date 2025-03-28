using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Application.Features.EventTypes.Commands
{
    public class DeleteEventTypeCommandHandler : IRequestHandler<DeleteEventTypeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteEventTypeCommandHandler> _logger;

        public DeleteEventTypeCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteEventTypeCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<bool> Handle(DeleteEventTypeCommand request, CancellationToken cancellationToken)
        {
            bool result = false;
            var name = request.GetType().Name; //NOMBRE DE LA CLASE A LA QUE PERTENECE

            try
            {
                _logger.LogInformation($"Ejecutando el command request {name}");

                var register = await _unitOfWork.Repository<EventType>().GetByIdAsync(request.Id);
                if (register != null)
                {
                    await _unitOfWork.Repository<EventType>().DeleteAsync(register);
                    result = true;
                }

                _logger.LogInformation($"El comando {name} se ejecuta exitosamente");

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex, $"El commando {name} tuvo errores");
                throw new  ApplicationException($"El commando {name} tuvo errores");
            }

        }
    }
}
