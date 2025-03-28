using Finaktiva.Application.Models.ViewModels.EventTypes;
using MediatR;

namespace Application.Features.EventTypes.Queries
{
    public class GetAllEventTypeCommand : IRequest<IEnumerable<EventTypeVm>>
    {
    }
}
