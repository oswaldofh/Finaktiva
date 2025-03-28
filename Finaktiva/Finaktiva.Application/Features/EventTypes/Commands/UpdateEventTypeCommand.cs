using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Models.ViewModels.EventTypes;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.EventTypes.Commands
{
    public class UpdateEventTypeCommand : IRequest<Response<EventTypeVm>>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
