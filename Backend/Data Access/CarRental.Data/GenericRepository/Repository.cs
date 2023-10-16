using CarRental.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.GenericRepository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> dbSet = null;

        public Repository(CarRentalDbContext dbContext)
        {
            dbSet = dbContext.Set<TEntity>();
        }

        public virtual int Count
        {
            get { return dbSet.Count(); }
        }

        public async virtual Task Insert(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public virtual async Task Update(TEntity entity)
        {
            dbSet.Update(entity);
        }

        public virtual async Task Delete(object id)
        {
            TEntity entity = await dbSet.FindAsync(id);
            dbSet.Remove(entity);
        }

        public virtual async Task Delete(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public virtual async Task<TEntity> GetById(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.Where(predicate).FirstOrDefaultAsync();
        }

        public virtual async Task<bool> Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.AnyAsync(predicate);
        }

        public virtual async Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet.Where(predicate);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }
    }
}
