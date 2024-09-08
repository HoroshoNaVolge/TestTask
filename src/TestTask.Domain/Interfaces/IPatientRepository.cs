using TestTask.Domain.Entities;

namespace TestTask.Domain.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync(int pageNumber, int pageSize, string sortBy);
        Task<Patient?> GetByIdAsync(int id);
        Task<int> AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(int id);
    }
}
