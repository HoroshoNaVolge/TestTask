using TestTask.Application.Interfaces;
using TestTask.Application.DTOs;
using TestTask.Domain.Entities;
using AutoMapper;
using TestTask.Domain.Interfaces;

namespace TestTask.Application.Services
{
    public class PatientService(IPatientRepository patientRepository, IUchastokRepository uchastokRepository, IMapper mapper) : IPatientService
    {
        public async Task<IEnumerable<PatientListDto>> GetPatientsAsync(int pageNumber, int pageSize, string sortBy)
        {
            var patients = await patientRepository.GetAllAsync(pageNumber, pageSize, sortBy);
            return mapper.Map<IEnumerable<PatientListDto>>(patients);
        }

        public async Task<PatientEditDto> GetPatientByIdAsync(int id)
        {
            var patient = await patientRepository.GetByIdAsync(id);
            if (patient == null) return null!;

            return mapper.Map<PatientEditDto>(patient);
        }

        public async Task CreatePatientAsync(PatientEditDto patientDto)
        {
            var patient = mapper.Map<Patient>(patientDto);
            await patientRepository.AddAsync(patient);
        }

        public async Task UpdatePatientAsync(int id, PatientEditDto patientDto)
        {
            await TryValidateData(patientDto);

            var patient = await patientRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("patient not found");
            patient.UchastokId = patientDto.UchastokId;

            await patientRepository.UpdateAsync(patient);
        }

        public async Task DeletePatientAsync(int id)
        {
            await patientRepository.DeleteAsync(id);
        }

        private async Task TryValidateData(PatientEditDto patientDto)
        {
            if (patientDto.UchastokId != null && !await uchastokRepository.ExistsAsync(patientDto.UchastokId.Value))
                throw new ArgumentException("Uchastok with specified ID does not exist.");
        }
    }
}
