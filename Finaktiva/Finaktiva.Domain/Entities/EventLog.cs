using Finaktiva.Domain.Common;

namespace Finaktiva.Domain.Entities
{
    public class EventLog : Entity
    {
        public int EventTypeId { get; set; }
        public EventType EventType { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
