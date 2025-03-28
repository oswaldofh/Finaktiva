using Finaktiva.Application.Models.ViewModels.EventTypes;
using MediatR;

namespace Application.Features.EventTypes.Queries
{
    public class GetEventTypeByIdCommand : IRequest<EventTypeVm>
    {
        public int Id { get; set; }
        public GetEventTypeByIdCommand(int id) => Id = id;
    }
}
