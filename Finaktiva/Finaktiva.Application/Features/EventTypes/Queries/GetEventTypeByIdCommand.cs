using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Models.ViewModels.EventTypes;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.EventTypes.Queries
{
    public class GetEventTypeByIdCommand : IRequest<Response<EventTypeVm>>
    {
        [Required]
        public int Id { get; set; }
        public GetEventTypeByIdCommand(int id) => Id = id;
    }
}
