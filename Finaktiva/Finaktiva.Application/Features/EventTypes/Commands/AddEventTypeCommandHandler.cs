using AutoMapper;
using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Application.Models.ViewModels.EventTypes;
using Finaktiva.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Application.Features.EventTypes.Commands
{
    public class AddEventTypeCommandHandler : IRequestHandler<AddEventTypeCommand, EventTypeVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AddEventTypeCommandHandler> _logger;


        public AddEventTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddEventTypeCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public ILogger<AddEventTypeCommandHandler> Logger { get; }

        public async Task<EventTypeVm> Handle(AddEventTypeCommand request, CancellationToken cancellationToken)
        {
            var register = _mapper.Map<EventType>(request);
            var name = request.GetType().Name; //NOMBRE DE LA CLASE A LA QUE PERTENECE
            try
            {
                _logger.LogInformation($"Ejecutando el command request {name}");
                await _unitOfWork.Repository<EventType>().AddAsync(register);
                _logger.LogInformation($"El comando {name} se ejecuta exitosamente");
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Duplicate") || ex.InnerException.Message.Contains("Duplicada"))
                {
                    _logger.LogError(ex.Message, ex, $"El commando {name} tuvo errores");
                    throw new DuplicateNameException($"EventType name {request.Name} is duplicate");
                }

            }
            return _mapper.Map<EventTypeVm>(register);
        }
    }
}
