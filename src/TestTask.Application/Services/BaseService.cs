using AutoMapper;
using TestTask.Application.Interfaces;
using TestTask.Domain.Interfaces.Persons;

namespace TestTask.Application.Services
{
    public abstract class BaseService<TEntityListDto, TEntityEditDto, TEntityCreateDto, TEntity>(IPersonRepository<TEntity> repository, IMapper mapper)
                                     : IBaseService<TEntityListDto, TEntityEditDto, TEntityCreateDto, TEntity> where TEntity : class
    {
        protected readonly IPersonRepository<TEntity> Repository = repository;
        protected readonly IMapper Mapper = mapper;

        public virtual async Task<IEnumerable<TEntityListDto>> GetAllAsync(int pageNumber, int pageSize, string sortBy, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var entities = await Repository.GetAllAsync(pageNumber, pageSize, sortBy, cancellationToken);
            return Mapper.Map<IEnumerable<TEntityListDto>>(entities);
        }

        public virtual async Task<TEntityEditDto> GetByIdAsync(int id)
        {
            var entity = await Repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"{typeof(TEntity).Name} not found");
            return Mapper.Map<TEntityEditDto>(entity);
        }

        public virtual async Task<int> CreateAsync(TEntityCreateDto dto)
        {
            var entity = Mapper.Map<TEntity>(dto);
            return await Repository.AddAsync(entity);
        }

        public virtual async Task UpdateAsync(int id, TEntityEditDto dto)
        {
            var entity = await Repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"{typeof(TEntity).Name} not found");
            Mapper.Map(dto, entity);
            await Repository.UpdateAsync(entity);
        }

        public virtual async Task DeleteAsync(int id)
        {
            await Repository.DeleteAsync(id);
        }
    }
}