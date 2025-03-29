using AutoMapper;
using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.ExceptionLogs.Commands
{
    public class AddExceptionLogCommandHandler : IRequestHandler<AddExceptionLogCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AddExceptionLogCommandHandler> _logger;


        public AddExceptionLogCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddExceptionLogCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<bool> Handle(AddExceptionLogCommand request, CancellationToken cancellationToken)
        {
            var exceptionLog = _mapper.Map<ExcepcionLog>(request);
            var name = request.GetType().Name;

            try
            {
                await _unitOfWork.Repository<ExcepcionLog>().AddAsync(exceptionLog);
                _logger.LogInformation($"El comando {name} se ejecuta exitosamente");
                return true;
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

            }
            return false;
        }


    }
}
