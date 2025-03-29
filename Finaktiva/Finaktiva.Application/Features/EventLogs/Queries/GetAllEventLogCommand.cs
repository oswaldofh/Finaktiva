using Finaktiva.Aplication.Features.Common;
using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Models.ViewModels;
using Finaktiva.Application.Models.ViewModels.EventLogs;
using MediatR;

namespace Application.Features.EventLogs.Queries
{
    public class GetAllEventLogCommand : PaginationParams, IRequest<Response<PaginationVm<EventLogVm>>>
    {
        public int? EventTypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
