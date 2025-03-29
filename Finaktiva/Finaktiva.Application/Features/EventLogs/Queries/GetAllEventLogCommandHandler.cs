using AutoMapper;
using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Application.Models.ViewModels;
using Finaktiva.Application.Models.ViewModels.EventLogs;
using Finaktiva.Application.Specifications;
using Finaktiva.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Features.EventLogs.Queries
{
    public class GetAllEventLogCommandHandler : IRequestHandler<GetAllEventLogCommand, Response<PaginationVm<EventLogVm>>>
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
        public async Task<Response<PaginationVm<EventLogVm>>> Handle(GetAllEventLogCommand request, CancellationToken cancellationToken)
        {
            var name = request.GetType().Name;
            var spec = new EventLogPaginationSpecification(request);
            try
            {
                var list = await _unitOfWork.Repository<EventLog>().GetAllWithSpec(spec);
                var total = await _unitOfWork.Repository<EventLog>().CountAsync(spec.Criteria);

                _logger.LogInformation($"El comando {name} se ejecuta exitosamente");

                var result = _mapper.Map<List<EventLogVm>>(list);
                var pagination = new PaginationVm<EventLogVm>
                {
                    Data = result,
                    Count = total,
                    PageCount = total % request.PageSize == 0 ? total / request.PageSize : (total / request.PageSize) + 1,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                };

                return Response<PaginationVm<EventLogVm>>.SuccessResponse(pagination);
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
                return Response<PaginationVm<EventLogVm>>.ErrorResponse(
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
                return Response<PaginationVm<EventLogVm>>.ErrorResponse(
                    $"Error interno: {ex.InnerException.Message}",
                    StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}
