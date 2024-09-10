using TestTask.Application.DTOs;
using AutoMapper;
using TestTask.Domain.Interfaces.Common;
using TestTask.Domain.Interfaces.Persons;
using TestTask.Domain.Entities.Persons;
using TestTask.Domain.Entities.Other;

namespace TestTask.Application.Services
{
    public class PatientService(IPersonRepository<Patient> patientRepository, ICommonRepository<Uchastok> uchastokRepository, IMapper mapper)
        : BaseService<PatientListDto, PatientEditDto, PatientCreateDto, Patient>(patientRepository, mapper)
    {

        private async Task TryValidateData(PatientBaseDto patientDto)
        {
            if (patientDto.UchastokId != null && !await uchastokRepository.ExistsAsync(patientDto.UchastokId.Value))
                throw new ArgumentException("Uchastok with specified ID does not exist.");
        }

        public override async Task<int> CreateAsync(PatientCreateDto dto)
        {
            await TryValidateData(dto);
            return await base.CreateAsync(dto);
        }

        public override async Task UpdateAsync(int id, PatientEditDto dto)
        {
            await TryValidateData(dto);
            await base.UpdateAsync(id, dto);
        }
    }
}

