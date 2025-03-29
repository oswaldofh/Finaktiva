using Finaktiva.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Finaktiva.Domain.Entities
{
    public class ExcepcionLog : Entity
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string? Description { get; set; }
        public string? StackTrace { get; set; }
    }
}
