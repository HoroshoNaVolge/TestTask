using TestTask.Application.DTOs;
using AutoMapper;
using TestTask.Domain.Interfaces.Common;
using TestTask.Domain.Interfaces.Persons;
using TestTask.Domain.Entities.Persons;
using TestTask.Domain.Entities.Other;
using static TestTask.Application.Validation.ValidationHelper;

namespace TestTask.Application.Services
{
    public class DoctorService(
    IPersonRepository<Doctor> doctorRepository,
    ICommonRepository<Cabinet> cabinetRepository,
    ICommonRepository<Specialization> specializationRepository,
    ICommonRepository<Uchastok> uchastokRepository,
    IMapper mapper) : BaseService<DoctorListDto, DoctorEditDto, DoctorBaseDto, Doctor>(doctorRepository, mapper)
    {
        private async Task TryValidateData(DoctorBaseDto doctorDto)
        {
            await EntityValidator.EnsureExistsAsync(cabinetRepository, doctorDto.CabinetId, "Cabinet");
            await EntityValidator.EnsureExistsAsync(specializationRepository, doctorDto.SpecializationId, "Specialization");
            await EntityValidator.EnsureExistsAsync(uchastokRepository, doctorDto.UchastokId, "Uchastok");
        }

        public override async Task<int> CreateAsync(DoctorBaseDto dto)
        {
            await TryValidateData(dto);
            return await base.CreateAsync(dto);
        }

        public override async Task UpdateAsync(int id, DoctorEditDto dto)
        {
            await TryValidateData(dto);
            await base.UpdateAsync(id, dto);
        }
    }
}
