using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Api.Web.Models
{
    public class CategoryUpdate
    {
        public long CategoryId { get; set; }
        public string Name { get; set; }
    }
}
