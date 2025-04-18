﻿using AutoMapper;
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
    public class UpdateEventLogCommandHandler : IRequestHandler<UpdateEventLogCommand, Response<EventLogVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateEventLogCommandHandler> _logger;

        public UpdateEventLogCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateEventLogCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Response<EventLogVm>> Handle(UpdateEventLogCommand request, CancellationToken cancellationToken)
        {
            var name = request.GetType().Name;

            try
            {
                var eventLog = await _unitOfWork.Repository<EventLog>().GetFirstOrDefaultAsync(x => x.Id == request.Id);
                if (eventLog is null)
                {
                    return Response<EventLogVm>.NoFoundResponse(
                      message: $"No se encontro un registro con el id {request.Id}",
                      statusCode: StatusCodes.Status404NotFound
                   );
                }

                eventLog.Description = request.Description;
                eventLog.Date = request.Date;
                eventLog.EventTypeId = request.EventTypeId;

                await _unitOfWork.Complete();

                _logger.LogInformation($"El comando {name} se ejecuta exitosamente");

                var reventLogVm = _mapper.Map<EventLogVm>(eventLog);

                return Response<EventLogVm>.SuccessResponse(
                    data: reventLogVm,
                    message: "Registro actualizado exitosamente"
                );

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
                return Response<EventLogVm>.ErrorResponse(
                    $"Acceso no autorizado: {ex.Message}",
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
                return Response<EventLogVm>.ErrorResponse(
                    $"Error interno: {ex.InnerException.Message}",
                    StatusCodes.Status500InternalServerError
                );
            }

        }
    }
}
