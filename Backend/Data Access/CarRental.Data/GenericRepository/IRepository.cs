using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.GenericRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        int Count { get; }
        Task Insert(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(object id);
        Task Delete(TEntity entity);
        Task<TEntity> GetById(object id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        Task<bool> Contains(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> GetAll(params Expression<Func<TEntity, object>>[] includes);
    }
}
