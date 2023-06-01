using AutoMapper;
using Core.Application.Interfaces.Persistence;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistence.Services
{
    public abstract class ServiceBase<TEntity, TDto> : IServiceBase<TDto>
        where TEntity : class
        where TDto : class, IIdentifiable
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public ServiceBase(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.GetRepository<TEntity>().GetByIdAsync(id);

            if (entity == null)
            {
                throw new ValidationException("Not found.");
            }

            return _mapper.Map<TDto>(entity);
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            IEnumerable<TEntity> entities = await _unitOfWork.GetRepository<TEntity>().GetAllAsync();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public async Task CreateAsync(TDto dto)
        {
            ValidateDto(dto);

            var entity = _mapper.Map<TEntity>(dto);
            _unitOfWork.GetRepository<TEntity>().Add(entity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(TDto dto)
        {
            ValidateDto(dto);

            TEntity entity = await _unitOfWork.GetRepository<TEntity>().GetByIdAsync(dto.Id);
            _mapper.Map(dto, entity);
            _unitOfWork.GetRepository<TEntity>().Update(entity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            TEntity entity = await _unitOfWork.GetRepository<TEntity>().GetByIdAsync(id);

            if (entity == null)
            {
                throw new ValidationException("Not found.");
            }

            _unitOfWork.GetRepository<TEntity>().Delete(entity);
            await _unitOfWork.CompleteAsync();
        }

        protected abstract void ValidateDto(TDto dto);
    }
}
