namespace TestTask.Application.Interfaces
{
    public interface IBaseService<TEntityListDto, TEntityEditDto, TEntityCreateDto, TEntity>
    {
        Task<IEnumerable<TEntityListDto>> GetAllAsync(int pageNumber, int pageSize, string sortBy, CancellationToken cancellationToken);
        Task<TEntityEditDto> GetByIdAsync(int id);
        Task<int> CreateAsync(TEntityCreateDto dto);
        Task UpdateAsync(int id, TEntityEditDto dto);
        Task DeleteAsync(int id);
    }
}
