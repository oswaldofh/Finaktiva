using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Models.ViewModels.EventTypes;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.EventTypes.Commands
{
    public class AddEventTypeCommand : IRequest<Response<EventTypeVm>>
    {
        [Display(Name = "Nombre")]
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
