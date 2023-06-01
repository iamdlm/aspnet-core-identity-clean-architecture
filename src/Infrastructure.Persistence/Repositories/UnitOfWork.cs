using Core.Application.Interfaces.Persistence;
using Infrastructure.Persistence.Data;

namespace Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> repositories;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            repositories = new Dictionary<Type, object>();
        }

        public IRepositoryBase<T> GetRepository<T>() where T : class
        {
            // Check if the repository already exists in the dictionary.
            if (repositories.ContainsKey(typeof(T)))
            {
                return repositories[typeof(T)] as IRepositoryBase<T>;
            }

            // If the repository doesn't exist, create it and add to the dictionary.
            IRepositoryBase<T> repo = new RepositoryBase<T>(_context);
            repositories.Add(typeof(T), repo);
            return repo;
        }

        public async Task<bool> CompleteAsync() => await _context.SaveChangesAsync() > 0;

        public void Dispose() => _context.Dispose();
    }
}
