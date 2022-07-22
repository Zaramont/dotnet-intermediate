using AutoMapper;
using CatalogService.Api.Web.Models;
using CatalogService.Models;

namespace CatalogService.Api.Web.Utilities
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<CategoryUpdate, Category>().ReverseMap();
            CreateMap<ItemUpdate, Item>().ReverseMap();
            CreateMap<Category, Category>()
                .ForMember(dst => dst.Items, opt => opt.MapFrom(src => src.Items))
                .ReverseMap();
        }
    }
}
