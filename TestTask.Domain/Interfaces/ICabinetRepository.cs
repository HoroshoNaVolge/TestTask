namespace TestTask.Domain.Interfaces
{
    public interface ICabinetRepository
    {
        Task<bool> ExistsAsync(int id);
    }
}
