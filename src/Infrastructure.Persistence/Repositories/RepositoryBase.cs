using Core.Application.Interfaces.Persistence;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly AppDbContext _appContext;

        public RepositoryBase(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _appContext.Set<T>();

            foreach (Expression<Func<T, object>> include in includes)
            {
                query = query.Include(include);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _appContext.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _appContext.Set<T>();

            foreach (Expression<Func<T, object>> include in includes)
            {
                query = query.Include(include);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> filter) => await _appContext.Set<T>().Where(filter).ToListAsync();

        public async Task<T> GetByIdAsync(Guid id) => await _appContext.Set<T>().FindAsync(id);

        public void Add(T entity) => _appContext.Set<T>().Add(entity);

        public void Update(T entity) => _appContext.Set<T>().Update(entity);

        public void Delete(T entity) => _appContext.Set<T>().Remove(entity);
    }
}
