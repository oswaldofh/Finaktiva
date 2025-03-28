using Finaktiva.Application.Models.ViewModels.EventTypes;
using MediatR;

namespace Application.Features.EventTypes.Commands
{
    public class UpdateEventTypeCommand : IRequest<EventTypeVm>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
