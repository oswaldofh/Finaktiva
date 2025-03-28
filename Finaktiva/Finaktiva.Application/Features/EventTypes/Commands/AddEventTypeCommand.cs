using Finaktiva.Application.Models.ViewModels.EventTypes;
using MediatR;

namespace Application.Features.EventTypes.Commands
{
    public class AddEventTypeCommand : IRequest<EventTypeVm>
    {
        public string Name { get; set; }
    }
}
