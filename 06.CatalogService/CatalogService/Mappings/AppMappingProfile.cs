using AutoMapper;
using CatalogService.Models.App;
using CatalogService.Models.EF;

namespace CatalogService.Api.Web.Utilities
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<CategoryDetail, Category>().ReverseMap();
            CreateMap<CategoryForCreate, Category>().ReverseMap();
            CreateMap<CategoryForUpdate, Category>().ReverseMap();
            CreateMap<ItemDetail, Item>().ReverseMap();
            CreateMap<ItemForCreate, Item>().ReverseMap();
            CreateMap<ItemForUpdate, Item>().ReverseMap();
        }
    }
}
