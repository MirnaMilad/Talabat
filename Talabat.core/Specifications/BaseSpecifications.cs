﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities;

namespace Talabat.core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get ; set; }
        public List<Expression<Func<T, object>>> Includes { get ; set; } = new List<Expression<Func<T, object>>> { };
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }

        //Get All
        public BaseSpecifications()
        {
        }

        //Get by id
        public BaseSpecifications(Expression<Func<T, bool>> CreiteriaExpression)
        {
            Criteria = CreiteriaExpression;
        }

        public void AddOrderBy(Expression<Func<T , object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        public void AddOrderByDescending(Expression <Func<T , object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        public void ApplyPagination (int skip , int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}
