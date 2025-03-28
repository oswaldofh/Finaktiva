using AutoMapper;
using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Application.Models.ViewModels.EventTypes;
using Finaktiva.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace Application.Features.EventTypes.Commands
{
    public class UpdateEventTypeCommandHandler : IRequestHandler<UpdateEventTypeCommand, EventTypeVm>
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
        public async Task<EventTypeVm> Handle(UpdateEventTypeCommand request, CancellationToken cancellationToken)
        {
            var name = request.GetType().Name; //NOMBRE DE LA CLASE A LA QUE PERTENECE
            try
            {
                _logger.LogInformation($"Ejecutando el command request {name}");
                var register = await _unitOfWork.Repository<EventType>().GetFirstOrDefaultAsync(x => x.Id == request.Id);

                register.Name = request.Name;
                register.IsActive = request.IsActive;

                await _unitOfWork.Complete();


                _logger.LogInformation($"El comando {name} se ejecuta exitosamente");

                return _mapper.Map<EventTypeVm>(register);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex, $"El commando {name} tuvo errores");
                throw;
            }
           
        }
    }
}
