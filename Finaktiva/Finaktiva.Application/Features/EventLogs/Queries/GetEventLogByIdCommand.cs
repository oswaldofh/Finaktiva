using Finaktiva.Application.Abstractions;
using Finaktiva.Application.Models.ViewModels.EventLogs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.EventLogs.Queries
{
    public class GetEventLogByIdCommand : IRequest<Response<EventLogVm>>
    {
        [Required]
        public int Id { get; set; }
        public GetEventLogByIdCommand(int id) => Id = id;
    }
}
