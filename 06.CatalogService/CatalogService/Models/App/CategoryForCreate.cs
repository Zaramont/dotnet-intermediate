﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Models.App
{
    public class CategoryForCreate
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
