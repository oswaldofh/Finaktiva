using AutoMapper;
using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Application.Models.ViewModels.EventTypes;
using Finaktiva.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.EventTypes.Queries
{
    public class GetAllEventTypeCommandHandler : IRequestHandler<GetAllEventTypeCommand, IEnumerable<EventTypeVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllEventTypeCommandHandler> _logger;

        public GetAllEventTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllEventTypeCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<EventTypeVm>> Handle(GetAllEventTypeCommand request, CancellationToken cancellationToken)
        {
            var name = request.GetType().Name; //NOMBRE DE LA CLASE A LA QUE PERTENECE
            try
            {
                _logger.LogInformation($"Ejecutando el command request {name}");

                var list = await _unitOfWork.Repository<EventType>().GetAllAsync();

                _logger.LogInformation($"El comando {name} se ejecuta exitosamente");

                return _mapper.Map<List<EventTypeVm>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex, $"El commando {name} tuvo errores");
                throw;
            }

        }
    }
}
