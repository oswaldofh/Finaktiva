using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Models.ViewModels;
using MediatR;

namespace Application.Features.ExceptionLogs.Queries
{
    public class GetAllExceptionLogCommand : IRequest<Response<IEnumerable<ExceptionLogVm>>>
    {
    }
}
