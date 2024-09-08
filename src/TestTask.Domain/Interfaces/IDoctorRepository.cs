using TestTask.Domain.Entities;

namespace TestTask.Domain.Interfaces
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllAsync(int pageNumber, int pageSize, string sortBy);
        Task<Doctor?> GetByIdAsync(int id);
        Task<int> AddAsync(Doctor doctor);
        Task UpdateAsync(Doctor doctor);
        Task DeleteAsync(int id);
    }
}