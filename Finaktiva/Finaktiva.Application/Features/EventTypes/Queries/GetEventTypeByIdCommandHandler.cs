using AutoMapper;
using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Application.Models.ViewModels.EventTypes;
using Finaktiva.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.EventTypes.Queries
{
    public class GetEventTypeByIdCommandHandler : IRequestHandler<GetEventTypeByIdCommand, EventTypeVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEventTypeByIdCommandHandler> _logger;
        public GetEventTypeByIdCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetEventTypeByIdCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<EventTypeVm> Handle(GetEventTypeByIdCommand request, CancellationToken cancellationToken)
        {
            var name = request.GetType().Name;
            try
            {
                _logger.LogInformation($"Ejecutando el command request {name}");

                var register = await _unitOfWork.Repository<EventType>().GetByIdAsync(request.Id);

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
