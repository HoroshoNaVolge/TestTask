using AutoMapper;
using TestTask.Application.Interfaces;
using TestTask.Domain.Interfaces.Persons;

namespace TestTask.Application.Services
{
    public abstract class BaseService<TEntityListDto, TEntityEditDto, TEntityBaseDto, TEntity>(IPersonRepository<TEntity> repository, IMapper mapper)
    : IBaseService<TEntityListDto, TEntityEditDto, TEntityBaseDto, TEntity> where TEntity : class
    {
        protected readonly IPersonRepository<TEntity> Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        protected readonly IMapper Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public virtual async Task<IEnumerable<TEntityListDto>> GetAllAsync(int pageNumber, int pageSize, string sortBy, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var entities = await Repository.GetAllAsync(pageNumber, pageSize, sortBy, cancellationToken);
            return Mapper.Map<IEnumerable<TEntityListDto>>(entities);
        }

        public virtual async Task<TEntityEditDto> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var entity = await GetByIdOrThrowAsync(id, cancellationToken);
            return Mapper.Map<TEntityEditDto>(entity);
        }

        public virtual async Task<int> CreateAsync(TEntityBaseDto dto, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var entity = Mapper.Map<TEntity>(dto);
            return await Repository.AddAsync(entity, cancellationToken);
        }

        public virtual async Task UpdateAsync(int id, TEntityEditDto dto, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var entity = await GetByIdOrThrowAsync(id, cancellationToken);
            Mapper.Map(dto, entity);
            await Repository.UpdateAsync(entity, cancellationToken);
        }

        public virtual async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Repository.DeleteAsync(id, cancellationToken);
        }

        public async Task<TEntity> GetByIdOrThrowAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Repository.GetByIdAsync(id, cancellationToken)
                ?? throw new KeyNotFoundException($"{typeof(TEntity).Name} not found");
        }
    }
}
