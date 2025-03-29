using AutoMapper;
using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Application.Models.ViewModels;
using Finaktiva.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Features.ExceptionLogs.Queries
{
    public class GetAllExceptionLogCommandHandler : IRequestHandler<GetAllExceptionLogCommand, Response<IEnumerable<ExceptionLogVm>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllExceptionLogCommandHandler> _logger;

        public GetAllExceptionLogCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllExceptionLogCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Response<IEnumerable<ExceptionLogVm>>> Handle(GetAllExceptionLogCommand request, CancellationToken cancellationToken)
        {
            var name = request.GetType().Name;
            try
            {
                var list = await _unitOfWork.Repository<ExcepcionLog>().GetAllAsync();
                _logger.LogInformation($"El comando {name} se ejecuta exitosamente");

                var result = _mapper.Map<List<ExceptionLogVm>>(list);

                return Response<IEnumerable<ExceptionLogVm>>.SuccessResponse(result);
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
                return Response<IEnumerable<ExceptionLogVm>>.ErrorResponse(
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
                return Response<IEnumerable<ExceptionLogVm>>.ErrorResponse(
                    $"Error interno: {ex.InnerException.Message}",
                    StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}
