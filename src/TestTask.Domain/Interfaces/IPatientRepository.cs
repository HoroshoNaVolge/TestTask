using TestTask.Domain.Entities;

namespace TestTask.Domain.Interfaces
{
    public interface IPatientRepository
    {
        Task<IReadOnlyCollection<Patient>> GetAllAsync(int pageNumber, int pageSize, string sortBy, CancellationToken cancellationToken);
        Task<Patient?> GetByIdAsync(int id);
        Task<int> AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(int id);
    }
}
