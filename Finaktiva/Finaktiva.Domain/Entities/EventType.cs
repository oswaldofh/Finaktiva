using Finaktiva.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Finaktiva.Domain.Entities
{
    public class EventType : Entity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
