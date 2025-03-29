using Finaktiva.Domain.Entities;

namespace Finaktiva.Application.Specifications
{
    public class EventLogSpecification : Specification<EventLog>
    {
        public EventLogSpecification()
        : base()
        {
            AddInclude(x => x.EventType);
            AddOrderByDescending(x => x.Id);
        }
    }
}
