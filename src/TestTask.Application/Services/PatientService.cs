using TestTask.Application.DTOs;
using AutoMapper;
using TestTask.Domain.Interfaces.Common;
using TestTask.Domain.Interfaces.Persons;
using TestTask.Domain.Entities.Persons;
using TestTask.Domain.Entities.Other;
using static TestTask.Application.Validation.ValidationHelper;

namespace TestTask.Application.Services
{
    public class PatientService(IPersonRepository<Patient> patientRepository, ICommonRepository<Uchastok> uchastokRepository, IMapper mapper)
        : BaseService<PatientListDto, PatientEditDto, PatientBaseDto, Patient>(patientRepository, mapper)
    {

        private async Task TryValidateData(PatientBaseDto patientDto)
        {
            await EntityValidator.EnsureExistsAsync(uchastokRepository, patientDto.UchastokId, "Uchastok");
        }

        public override async Task<int> CreateAsync(PatientBaseDto dto, CancellationToken cancellationToken)
        {
            await TryValidateData(dto);
            return await base.CreateAsync(dto, cancellationToken);
        }

        public override async Task UpdateAsync(int id, PatientEditDto dto, CancellationToken cancellationToken)
        {
            await TryValidateData(dto);
            await base.UpdateAsync(id, dto, cancellationToken);
        }
    }
}

