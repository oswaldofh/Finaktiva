using AutoMapper;
using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Application.Models.ViewModels.EventLogs;
using Finaktiva.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Features.EventLogs.Commands
{
    public class AddEventLogCommandHandler : IRequestHandler<AddEventLogCommand, Response<EventLogVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AddEventLogCommandHandler> _logger;


        public AddEventLogCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddEventLogCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public ILogger<AddEventLogCommandHandler> Logger { get; }

        public async Task<Response<EventLogVm>> Handle(AddEventLogCommand request, CancellationToken cancellationToken)
        {
            var reventLog = _mapper.Map<EventLog>(request);
            var name = request.GetType().Name;
           
            try
            {
                await _unitOfWork.Repository<EventLog>().AddAsync(reventLog);
                _logger.LogInformation($"El comando {name} se ejecuta exitosamente");

                var reventLogVm = _mapper.Map<EventLogVm>(reventLog);

                return Response<EventLogVm>.SuccessResponse(
                    data: reventLogVm,
                    StatusCodes.Status201Created,
                    message: "Se guarda el registro exitosamente"
                );
               
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex.InnerException?.Message ?? ex.Message, ex, $"El commando {name} tuvo errores");

                var exception = new ExcepcionLog
                {
                    Date = DateTime.UtcNow,
                    Name = name,
                    Description = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.StackTrace
                };
                await _unitOfWork.Repository<ExcepcionLog>().AddAsync(exception);
                return Response<EventLogVm>.ErrorResponse(
                    $"Acceso no autorizado {ex.InnerException?.Message ?? ex.Message}",
                    StatusCodes.Status401Unauthorized
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message ?? ex.Message, ex, $"El commando {name} tuvo errores");
                var exception = new ExcepcionLog
                {
                    Date = DateTime.UtcNow,
                    Name = name,
                    Description = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.StackTrace
                };
                await _unitOfWork.Repository<ExcepcionLog>().AddAsync(exception);

                return Response<EventLogVm>.ErrorResponse(
                    $"Error interno: {ex.InnerException?.Message ?? ex.Message}",
                    StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}
