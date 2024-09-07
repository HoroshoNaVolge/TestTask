namespace TestTask.Domain.Interfaces
{
    public interface IUchastokRepository
    {
        Task<bool> ExistsAsync(int id);
    }
}
