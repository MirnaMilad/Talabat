using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities;
using Talabat.core.Repositories;
using Talabat.core.Specifications;
using Talabat.repository.Data;

namespace Talabat.repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext) {
            _dbContext = dbContext;
        }
        #region With Specifications
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }


        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
            //return await _dbContext.Products.Where(p=>p.id == id).ToListAsync();
        }

        #endregion

        #region Without Specifications
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecifications(Spec).ToListAsync();
        }
        public async Task<T> GetEntityWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecifications(Spec).FirstOrDefaultAsync();
        }
        
        private IQueryable<T> ApplySpecifications(ISpecifications<T> Spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), Spec);
        }

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecifications(Spec).CountAsync();
        }

        public async Task Add(T item)
        {
            await _dbContext.Set<T>().AddAsync(item);
        }

        public void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
        }

        public void Update(T item)
        {
            _dbContext.Set<T>().Update(item);
        }


        #endregion
    }
}
