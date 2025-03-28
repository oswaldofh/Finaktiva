using Finaktiva.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finaktiva.Domain.Entities
{
    public class EventLog : Entity
    {
        [ForeignKey("VehicleId")]
        public int EventTypeId { get; set; }
        public EventType EventType { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
