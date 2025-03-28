using AutoMapper;

namespace Finaktiva.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Regional, AddRegionalCommand>().ReverseMap();
            //CreateMap<Regional, RegionalVm>().ReverseMap();
            //CreateMap<Product, ProductDto>()
            //.ForMember(pd => pd.Stock, opt => opt.MapFrom(p => p.Stock.Stock));

            // CreateMap<Regional, RegionalVm>().ReverseMap()
            //.ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name));
        }
    }
}
