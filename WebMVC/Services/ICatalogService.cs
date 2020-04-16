using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebMVC.Services
{
       public interface ICatalogService
        {
            Task<Catalog> GetCatalogItemsAsync(int page, int size, int? type, int? brand);
            Task<IEnumerable<SelectListItem>> GetBrandsAsync();
            Task<IEnumerable<SelectListItem>> GetTypesAsync();

        }   
}
