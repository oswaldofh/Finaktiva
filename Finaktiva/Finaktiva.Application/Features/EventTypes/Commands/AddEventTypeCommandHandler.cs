﻿using AutoMapper;
using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Application.Models.ViewModels.EventLogs;
using Finaktiva.Application.Models.ViewModels.EventTypes;
using Finaktiva.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json;

namespace Application.Features.EventTypes.Commands
{
    public class AddEventTypeCommandHandler : IRequestHandler<AddEventTypeCommand, Response<EventTypeVm>>
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
        public async Task<Response<EventTypeVm>> Handle(AddEventTypeCommand request, CancellationToken cancellationToken)
        {
            var eventType = _mapper.Map<EventType>(request);
            var name = request.GetType().Name;

            try
            {
                await _unitOfWork.Repository<EventType>().AddAsync(eventType);
                _logger.LogInformation($"El comando {name} se ejecuta exitosamente");

                var eventLogVm = _mapper.Map<EventTypeVm>(eventType);

                return Response<EventTypeVm>.SuccessResponse(
                    data: eventLogVm,
                    StatusCodes.Status201Created,
                    message: "Se guarda el registro exitosamente"
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

                return Response<EventTypeVm>.ErrorResponse(
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

                return Response<EventTypeVm>.ErrorResponse(
                    $"Error interno: {ex.InnerException.Message}",
                    StatusCodes.Status500InternalServerError
                );
            }
        }

       
    }
}
