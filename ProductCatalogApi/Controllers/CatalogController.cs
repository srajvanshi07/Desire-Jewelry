using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductCatalogApi.Data;
using ProductCatalogApi.Domain;
using ProductCatalogApi.ViewModels;

namespace ProductCatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CatalogController : ControllerBase
    {
        //global variable, incong coming from staup.cs
        private readonly CatalogContext _context;
        //create construtor //add this conf 
        private readonly IConfiguration _config;
        public CatalogController(CatalogContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Items(
            [FromQuery]int pageIndex = 0,
            [FromQuery]int pageSize = 6)
        {
            var itemsCount = await _context.CatalogItems.LongCountAsync();

            var items = await _context.CatalogItems
                                .OrderBy(c => c.Name)
                                .Skip(pageIndex * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            items = ChangePictureUrl(items);

            var model = new PaginatedItemsViewModel<CatalogItem>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Count = itemsCount,
                Data = items
            };
            return Ok(model);
        }

        [HttpGet]
        //---->[Route("[action]")]
        [Route("[action]/type/{catalogTypeId}/brand/{catalogBrandId}")]
        //action--> item{pageIndex}/{pageSize}")]
        //async required, web UI doing right mutithread way , means no blocking to access API backend , []-->attribute
        public async Task<IActionResult> Items(
            //Module18
             int? catalogTypeId,
            int? catalogBrandId,
            [FromQuery]int PageIndex = 0,
            [FromQuery] int PageSize = 6)
        //index is 1st page 6 3 size of page as doesnit have a lot data
        {

            //Module 18 select * from catalointem and filter apply
            var root = (IQueryable<CatalogItem>)_context.CatalogItems;
            if (catalogTypeId.HasValue)
            {
                root = root.Where(c => c.CatalogTypeId == catalogTypeId);
            }
            if (catalogBrandId.HasValue)
            {
                root = root.Where(c => c.CatalogBrandId == catalogBrandId);
            }
            //for COUNT select count(*) from catalogitems; now pass property to column
            //-- var itemsCount = await _context.CatalogItems.LongCountAsync();

            var itemsCount = await root.LongCountAsync();
            //await because this call will be in secondary thread 
            //var items = await _context.CatalogItems
            var items = await root
                             .OrderBy(c=>c.Name)
                            .Skip(PageIndex * PageSize)
                            .Take(PageSize)
                            .ToListAsync();

            //generate method
            items = ChangePictureUrl(items);
            //api send status back 200- ok 400-bad 500-internal error(pass data with ok

            var model = new PaginatedItemsViewModel<CatalogItem>
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                Count = itemsCount,
                Data = items
            };
            // return Ok(items);
            return Ok(model);

        }
        private List<CatalogItem> ChangePictureUrl(List<CatalogItem> items)
        {
            // throw new NotImplementedException(); use link query to get data from _config
            items.ForEach(
                c => c.PictureUrl =
                c.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced",
                _config["ExternalCatalogBaseUrl"]));
            return items;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogTypes()
        {
            var items = await _context.CatalogTypes.ToListAsync();
            return Ok(items);
        }
        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogBrands()
        {
            var items = await _context.CatalogBrands.ToListAsync();
            return Ok(items);
        }
    }
}