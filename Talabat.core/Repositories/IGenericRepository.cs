using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities;
using Talabat.core.Specifications;

namespace Talabat.core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        #region Without specifications
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        #endregion

        #region With Specifications
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec);
        Task<T> GetEntityWithSpecAsync(ISpecifications<T> Spec);

        Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec);

        Task Add(T item);

        void Delete(T item);
        void Update(T item);
        #endregion
    }
}
