﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace WebMVC.Infrastructure
{
    public class ApiPaths
    {
        public static class Catalog
        {
            public static string GetAllCatalogItems(string baseUri,
                int page, int take, int? brand, int? type)

            {
                //module18
                var filterQs = string.Empty;
                if (brand.HasValue||type.HasValue)
                {
                    var brandQs = (brand.HasValue) ? brand.Value.ToString() : "null";
                    var typeQs = (type.HasValue) ? type.Value.ToString() : "null";
                    filterQs=$"/type/{typeQs}/brand/{brandQs}";
                }
                return $"{baseUri}items{filterQs}?pageIndex={page}&pageSize={take}";
                
            }
        

            public static string GetAllTypes(string baseUri)
            {
                return $"{baseUri}catalogtypes";
            }

            public static string GetAllBrands(string baseUri)
            {
                return $"{baseUri}catalogbrands";
            }
        }


    }

}

