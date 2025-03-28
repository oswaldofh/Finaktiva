namespace Finaktiva.Application.Models.ViewModels.EventLogs
{
    public class EventLogVm
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string EventType { get; set; }
        public int EventTypeId { get; set; }
        public DateTime Date { get; set; }
    }
}
