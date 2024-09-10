namespace TestTask.Application.Interfaces
{
    public interface IBaseService<TEntityListDto, TEntityEditDto, TEntityBaseDto, TEntity>
    {
        Task<IEnumerable<TEntityListDto>> GetAllAsync(int pageNumber, int pageSize, string sortBy, CancellationToken cancellationToken);
        Task<TEntityEditDto> GetByIdAsync(int id);
        Task<int> CreateAsync(TEntityBaseDto dto);
        Task UpdateAsync(int id, TEntityEditDto dto);
        Task DeleteAsync(int id);
    }
}
