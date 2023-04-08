namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> CompleteAsync();
    }
}
