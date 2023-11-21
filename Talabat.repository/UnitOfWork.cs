using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core;
using Talabat.core.Entities;
using Talabat.core.Repositories;
using Talabat.repository.Data;

namespace Talabat.repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repositories;
        private readonly StoreContext _dbContext;

        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
            _repositories= new Hashtable();
        }
        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()=>
        await _dbContext.DisposeAsync();
        

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if(_repositories.ContainsKey(type))
            {
                //Create
                var Repository = new GenericRepository<TEntity>(_dbContext);    
                _repositories.Add(type, Repository);

            }
            return  _repositories[type] as IGenericRepository<TEntity>;
            
        }
    }
}
