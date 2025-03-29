using AutoMapper;
using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Application.Models.ViewModels.EventTypes;
using Finaktiva.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Features.EventTypes.Queries
{
    public class GetAllEventTypeCommandHandler : IRequestHandler<GetAllEventTypeCommand, Response<IEnumerable<EventTypeVm>>>
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
        public async Task<Response<IEnumerable<EventTypeVm>>> Handle(GetAllEventTypeCommand request, CancellationToken cancellationToken)
        {
            var name = request.GetType().Name;
            try
            {
                var list = await _unitOfWork.Repository<EventType>().GetAllAsync();
                _logger.LogInformation($"El comando {name} se ejecuta exitosamente");

                var result = _mapper.Map<List<EventTypeVm>>(list);

                return Response<IEnumerable<EventTypeVm>>.SuccessResponse(result);
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
                return Response<IEnumerable<EventTypeVm>>.ErrorResponse(
                    $"Acceso no autorizado {ex.Message}",
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
                return Response<IEnumerable<EventTypeVm>>.ErrorResponse(
                    $"Error interno: {ex.InnerException.Message}",
                    StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}
