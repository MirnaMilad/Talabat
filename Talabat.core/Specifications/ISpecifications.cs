using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities;

namespace Talabat.core.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        //_dbContext.Products.Where(P=>P.id == id).Include(P=> P.ProductBrand).Include(P=> P.ProductType);

        //sign for property for where condition [Where(P=>P.id == id)]
        public Expression<Func<T , bool>> Criteria { get; set; }
        //Sign for property for list of include [Include(P=> P.ProductBrand).Include(P=> P.ProductType)]
        public List<Expression<Func<T , Object>>> Includes { get; set; }

        //Property for OrderBy [orderBy(P=>P.Name)]

        public Expression<Func<T , object>> OrderBy { get; set; }

        //Property for OrderBy [orderByDesc(P=>P.Name)]
        public Expression <Func<T , object>> OrderByDescending { get;set; }
        //Take(2)
        public int Take { get; set; }

        //Skip(2)
        public int Skip { get; set; }

        public bool IsPaginationEnabled { get; set; }
    }
}
