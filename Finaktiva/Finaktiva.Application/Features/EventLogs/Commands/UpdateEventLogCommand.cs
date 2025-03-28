using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Models.ViewModels.EventLogs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.EventLogs.Commands
{
    public class UpdateEventLogCommand : IRequest<Response<EventLogVm>>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int EventTypeId { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
