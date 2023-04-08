using Application.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppIdentityDbContext _context;
        
        public UnitOfWork(AppIdentityDbContext context)
        {
            _context = context;            
        }

        public async Task<bool> CompleteAsync() => await _context.SaveChangesAsync() > 0;

        public void Dispose() => _context.Dispose();
    }
}
