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
            CreateMap<PagedList<Category>, PagedList<CategoryDetail>>().ConvertUsing(new PagedListCategoryConverter());
            CreateMap<PagedList<Item>, PagedList<ItemDetail>>().ConvertUsing(new PagedListItemConverter());
        }

        public class PagedListCategoryConverter : ITypeConverter<PagedList<Category>, PagedList<CategoryDetail>>
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

        public class PagedListItemConverter : ITypeConverter<PagedList<Item>, PagedList<ItemDetail>>
        {
            public PagedList<ItemDetail> Convert(PagedList<Item> source, PagedList<ItemDetail> destination, ResolutionContext context)
            {
                var vm = source
                    .Select(m => new ItemDetail()
                    { ItemId = m.ItemId, Name = m.Name, Description = m.Description, Price = m.Price, CategoryId = m.CategoryId });
                var result = PagedList<ItemDetail>.ToPagedList(vm.AsQueryable(), source.CurrentPage, source.PageSize);


                return result;
            }
        }
    }

    
}
