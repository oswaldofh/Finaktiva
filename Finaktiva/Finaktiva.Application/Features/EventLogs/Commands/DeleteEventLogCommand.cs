using Finaktiva.Application.Abstractions;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.EventLogs.Commands
{
    public class DeleteEventLogCommand : IRequest<Response<bool>>
    {
        [Required]
        public int Id { get; set; }
        public DeleteEventLogCommand(int id) => Id = id;
    }
}
