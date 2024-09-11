using System.Threading;

namespace TestTask.Application.Interfaces
{
    public interface IBaseService<TEntityListDto, TEntityEditDto, TEntityBaseDto, TEntity>
    {
        Task<IEnumerable<TEntityListDto>> GetAllAsync(int pageNumber, int pageSize, string sortBy, CancellationToken cancellationToken);
        Task<TEntityEditDto> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> CreateAsync(TEntityBaseDto dto, CancellationToken cancellationToken);
        Task UpdateAsync(int id, TEntityEditDto dto, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
