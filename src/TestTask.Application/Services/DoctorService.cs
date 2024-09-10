using TestTask.Application.DTOs;
using AutoMapper;
using TestTask.Domain.Interfaces.Common;
using TestTask.Domain.Interfaces.Persons;
using TestTask.Domain.Entities.Persons;
using TestTask.Domain.Entities.Other;

namespace TestTask.Application.Services
{
    public class DoctorService(
    IPersonRepository<Doctor> doctorRepository,
    ICommonRepository<Cabinet> cabinetRepository,
    ICommonRepository<Specialization> specializationRepository,
    ICommonRepository<Uchastok> uchastokRepository,
    IMapper mapper) : BaseService<DoctorListDto, DoctorEditDto, DoctorCreateDto, Doctor>(doctorRepository, mapper)
    {
        private async Task TryValidateData(DoctorBaseDto doctorDto)
        {
            if (doctorDto.CabinetId != null && !await cabinetRepository.ExistsAsync(doctorDto.CabinetId.Value))
                throw new ArgumentException("Cabinet with specified ID does not exist.");

            if (doctorDto.SpecializationId != null && !await specializationRepository.ExistsAsync(doctorDto.SpecializationId.Value))
                throw new ArgumentException("Specialization with specified ID does not exist.");

            if (doctorDto.UchastokId != null && !await uchastokRepository.ExistsAsync(doctorDto.UchastokId.Value))
                throw new ArgumentException("Uchastok with specified ID does not exist.");
        }

        public override async Task<int> CreateAsync(DoctorCreateDto dto)
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
