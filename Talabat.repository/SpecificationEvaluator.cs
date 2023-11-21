using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities;
using Talabat.core.Specifications;

namespace Talabat.repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        //_dbContext.Set<T>().where(P=>P.id == id).include(P=>P.ProductBrand).Include(P=>P.ProductType)
        //Fun to build Query
        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery , ISpecifications<T> Spec)
        {
            var query = InputQuery; //_dbContext.Set<T>()

            if(Spec.Criteria is not null) //P=>P.id == id
            {
                query = query.Where(Spec.Criteria); //_dbContext.Set<T>().where(P=>P.id == id)
            }
           // P => P.ProductBrand , P => P.ProductType
           
            if(Spec.OrderBy is not null)
            {
                query = query.OrderBy(Spec.OrderBy);
            }



            if(Spec.OrderByDescending is not null)
            {
                query = query.OrderByDescending(Spec.OrderByDescending);
            }

            if (Spec.IsPaginationEnabled)
            {
                query = query.Skip(Spec.Skip).Take(Spec.Take);
            }

            query = Spec.Includes.Aggregate(query , (CurrentQuery , IncludeExpression) =>  CurrentQuery.Include(IncludeExpression) );


            return query;
        }
    }
}
