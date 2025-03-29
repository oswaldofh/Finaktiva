using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.ExceptionLogs.Commands
{
    public class AddExceptionLogCommand : IRequest<bool>
    {
        public DateTime Date { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string? Description { get; set; }
        public string? StackTrace { get; set; }
    }
}
