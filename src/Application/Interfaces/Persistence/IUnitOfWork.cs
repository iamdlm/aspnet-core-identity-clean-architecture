namespace Application.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> CompleteAsync();
    }
}
