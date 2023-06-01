namespace Core.Application.Interfaces.Persistence
{
    public interface IServiceBase<TDto> where TDto : class, IIdentifiable
    {
        Task<TDto> GetByIdAsync(Guid id);
        Task<IEnumerable<TDto>> GetAllAsync();
        Task CreateAsync(TDto dto);
        Task UpdateAsync(TDto dto);
        Task DeleteAsync(Guid id);
    }
}
