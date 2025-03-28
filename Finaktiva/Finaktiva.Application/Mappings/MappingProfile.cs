using Application.Features.EventLogs.Commands;
using Application.Features.EventTypes.Commands;
using AutoMapper;
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
            CreateMap<EventLog, EventLogVm>().ReverseMap();

            //CreateMap<Product, ProductDto>()
            //.ForMember(pd => pd.Stock, opt => opt.MapFrom(p => p.Stock.Stock));

            // CreateMap<Regional, RegionalVm>().ReverseMap()
            //.ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name));
        }
    }
}
