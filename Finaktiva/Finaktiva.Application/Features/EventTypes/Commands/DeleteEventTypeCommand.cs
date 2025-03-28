using MediatR;

namespace Application.Features.EventTypes.Commands
{
    public class DeleteEventTypeCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public DeleteEventTypeCommand(int id) => Id = id;
    }
}
