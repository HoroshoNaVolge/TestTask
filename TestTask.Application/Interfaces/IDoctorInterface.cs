using TestTask.Application.DTOs;

namespace TestTask.Application.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorListDto>> GetDoctorsAsync(int pageNumber, int pageSize, string sortBy);
        Task<DoctorEditDto> GetDoctorByIdAsync(int id);
        Task CreateDoctorAsync(DoctorEditDto doctorDto);
        Task UpdateDoctorAsync(int id, DoctorEditDto doctorDto);
        Task DeleteDoctorAsync(int id);
    }
}
