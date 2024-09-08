using TestTask.Application.DTOs;

namespace TestTask.Application.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientListDto>> GetPatientsAsync(int pageNumber, int pageSize, string sortBy);
        Task<PatientEditDto> GetPatientByIdAsync(int id);
        Task<int> CreatePatientAsync(PatientCreateDto patientDto);
        Task UpdatePatientAsync(int id, PatientEditDto patientDto);
        Task DeletePatientAsync(int id);
    }
}
