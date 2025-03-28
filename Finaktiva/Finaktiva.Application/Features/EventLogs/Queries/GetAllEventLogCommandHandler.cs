using AutoMapper;
using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Application.Models.ViewModels.EventLogs;
using Finaktiva.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Features.EventLogs.Queries
{
    public class GetAllEventLogCommandHandler : IRequestHandler<GetAllEventLogCommand, Response<IEnumerable<EventLogVm>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllEventLogCommandHandler> _logger;

        public GetAllEventLogCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllEventLogCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Response<IEnumerable<EventLogVm>>> Handle(GetAllEventLogCommand request, CancellationToken cancellationToken)
        {
            var name = request.GetType().Name;
            try
            {
                var list = await _unitOfWork.Repository<EventLog>().GetAllAsync();
                _logger.LogInformation($"El comando {name} se ejecuta exitosamente");

                var result = _mapper.Map<List<EventLogVm>>(list);

                return Response<IEnumerable<EventLogVm>>.SuccessResponse(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<IEnumerable<EventLogVm>>.ErrorResponse(
                    $"Acceso no autorizado {ex.Message}",
                    StatusCodes.Status401Unauthorized
                );
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<EventLogVm>>.ErrorResponse(
                    $"Error interno: {ex.InnerException.Message}",
                    StatusCodes.Status500InternalServerError
                );
            }

        }
    }
}
