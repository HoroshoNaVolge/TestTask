namespace TestTask.Domain.Interfaces.Common
{
    public interface ICommonRepository<TEntity> where TEntity : class
    {
        Task<bool> ExistsAsync(int id);
    }
}
