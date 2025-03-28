using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Models.ViewModels.EventLogs;
using MediatR;

namespace Application.Features.EventLogs.Queries
{
    public class GetAllEventLogCommand : IRequest<Response<IEnumerable<EventLogVm>>>
    {
    }
}
