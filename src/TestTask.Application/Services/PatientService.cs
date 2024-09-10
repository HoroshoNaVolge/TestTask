using TestTask.Application.Interfaces;
using TestTask.Application.DTOs;
using AutoMapper;
using TestTask.Domain.Interfaces.Common;
using TestTask.Domain.Interfaces.Persons;
using TestTask.Domain.Entities.Persons;
using TestTask.Domain.Entities.Other;

namespace TestTask.Application.Services
{
    public class PatientService(IPersonRepository<Patient> patientRepository, ICommonRepository<Uchastok> uchastokRepository, IMapper mapper) : IPatientService
    {
        public async Task<IEnumerable<PatientListDto>> GetPatientsAsync(int pageNumber, int pageSize, string sortBy, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var patients = await patientRepository.GetAllAsync(pageNumber, pageSize, sortBy, cancellationToken);

            return mapper.Map<IEnumerable<PatientListDto>>(patients);
        }

        public async Task<PatientEditDto> GetPatientByIdAsync(int id)
        {
            var patient = await patientRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("patient not found");

            return mapper.Map<PatientEditDto>(patient);
        }

        public async Task<int> CreatePatientAsync(PatientCreateDto patientDto)
        {
            await TryValidateData(patientDto);

            var patient = mapper.Map<Patient>(patientDto);
            return await patientRepository.AddAsync(patient);
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

        private async Task TryValidateData(PatientBaseDto patientDto)
        {
            if (patientDto.UchastokId != null && !await uchastokRepository.ExistsAsync(patientDto.UchastokId.Value))
                throw new ArgumentException("Uchastok with specified ID does not exist.");
        }
    }
}
