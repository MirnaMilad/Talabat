﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities;

namespace Talabat.core.Specifications
{
    public class ProductWithBrandAndTypeSpecifications :BaseSpecifications<Products>
    {
        //CTOR is used for get all Products 
        public ProductWithBrandAndTypeSpecifications(ProductSpecParams Params) :base(
            P=>
            ( string.IsNullOrEmpty(Params.Search) ||P.Name.ToLower().Contains(Params.Search) )
            &&
            (
            !Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId)
            &&
            (Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId)
            )
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);

            if (string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }

            //Products = 100
            // PageSize = 10
            //PageIndex = 5

            //Skip => 40 = 10*4
            //Take => 10
            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);
        }
        //CTOR is used for get Product by id
        public ProductWithBrandAndTypeSpecifications(int id):base(P=> P.Id == id) 
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);

        }
    }
}
