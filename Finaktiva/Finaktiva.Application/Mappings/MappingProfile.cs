using Application.Features.EventLogs.Commands;
using Application.Features.EventTypes.Commands;
using AutoMapper;
using Finaktiva.Application.Models.ViewModels;
using Finaktiva.Application.Models.ViewModels.EventLogs;
using Finaktiva.Application.Models.ViewModels.EventTypes;
using Finaktiva.Domain.Entities;

namespace Finaktiva.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EventType, AddEventTypeCommand>().ReverseMap();
            CreateMap<EventType, EventTypeVm>().ReverseMap();

            CreateMap<EventLog, AddEventLogCommand>().ReverseMap();

            CreateMap<EventLog, EventLogVm>()
            .ForMember(e => e. EventType, opt => opt.MapFrom(vm => vm.EventType.Name));

            CreateMap<ExcepcionLog, ExceptionLogVm>().ReverseMap();

            
        }
    }
}
