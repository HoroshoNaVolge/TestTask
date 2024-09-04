namespace TestTask.Domain.Interfaces
{
    public interface ISpecializationRepository
    {
        Task<bool> ExistsAsync(int id);
    }
}
