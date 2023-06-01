namespace Core.Application.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryBase<T> GetRepository<T>() where T : class;
        Task<bool> CompleteAsync();
    }
}
