using AutoMapper;
using CatalogService.Models.App;
using CatalogService.Models.EF;
using CatalogService.Services;

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
            CreateMap<PagedList<Category>, PagedList<CategoryDetail>>().ConvertUsing(new PagedListConverter());
        }
        public class PagedListConverter : ITypeConverter<PagedList<Category>, PagedList<CategoryDetail>>
        {
            public PagedList<CategoryDetail> Convert(PagedList<Category> source, PagedList<CategoryDetail>  destination, ResolutionContext context)
            {
                var vm = source
                    .Select(m => new CategoryDetail()
                    { CategoryId = m.CategoryId, Name = m.Name, Description = m.Description });
                var result = PagedList<CategoryDetail>.ToPagedList(vm.AsQueryable(), source.CurrentPage, source.PageSize);
                    

                return result;
            }
        }
    }

    
}
