using Application.Features.EventLogs.Queries;
using Finaktiva.Domain.Entities;

namespace Finaktiva.Application.Specifications
{
    public class EventLogPaginationSpecification : Specification<EventLog>
    {
        public EventLogPaginationSpecification(GetAllEventLogCommand query)
            : base()
        {
            AddInclude(x => x.EventType);
            AddOrderByDescending(x => x.Id);

            if (query.EventTypeId.HasValue)
            {
                AddCriteria(x => x.EventTypeId == query.EventTypeId.Value);
            }
            if (query.StartDate.HasValue || query.EndDate.HasValue)
            {
                if (query.StartDate.HasValue && query.EndDate.HasValue)
                {
                    AddCriteria(x => x.Date >= query.StartDate.Value && x.Date <= query.EndDate.Value);
                }
                else if (query.StartDate.HasValue)
                {
                    AddCriteria(x => x.Date == query.StartDate.Value);
                }
                else if (query.EndDate.HasValue)
                {
                    AddCriteria(x => x.Date == query.EndDate.Value);
                }
            }
            ApplyPaging(query.PageSize * (query.PageIndex - 1), query.PageSize);
        }
    }
}
