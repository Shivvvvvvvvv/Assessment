using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TA.TopUp.Core.DataAccessAbstractions
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeExpressions);
        Task<TEntity> GetById(object id);
        void Insert(TEntity entity);
        void Delete(TEntity entityToDelete);
        void Delete(object id);
        void Update(TEntity entityToUpdate);
    }
}
