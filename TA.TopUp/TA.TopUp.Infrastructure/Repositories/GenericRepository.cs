using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TA.TopUp.Core.DataAccessAbstractions;
using TA.TopUp.Infrastructure.DataAccessAbstractions;

namespace TA.TopUp.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal TopUpSystemDbContext _context;
        internal DbSet<TEntity> _dbSet;

        public GenericRepository(TopUpSystemDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter,params Expression<Func<TEntity, object>>[] includeExpressions)
        {

            IQueryable<TEntity> query = _dbSet;
            if (includeExpressions != null)
            {
                foreach (Expression<Func<TEntity, object>> includeExpression in includeExpressions)
                    query = query.Include(includeExpression);
            }
           
            if(filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if(_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Delete(object id)
        {
            TEntity? entityToDelete = _dbSet.Find(id);
            if (entityToDelete != null)
                Delete (entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
