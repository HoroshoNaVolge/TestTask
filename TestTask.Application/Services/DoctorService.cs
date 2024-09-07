﻿using TestTask.Application.Interfaces;
using TestTask.Application.DTOs;
using TestTask.Domain.Entities;
using AutoMapper;
using TestTask.Domain.Interfaces;

namespace TestTask.Application.Services
{
    public class DoctorService(
        IDoctorRepository doctorRepository,
        ICabinetRepository cabinetRepository,
        ISpecializationRepository specializationRepository,
        IUchastokRepository uchastokRepository,
        IMapper mapper)
        : IDoctorService
    {
        public async Task<IEnumerable<DoctorListDto>> GetDoctorsAsync(int pageNumber, int pageSize, string sortBy)
        {
            var doctors = await doctorRepository.GetAllAsync(pageNumber, pageSize);

            var doctorDtos = mapper.Map<IEnumerable<DoctorListDto>>(doctors);

            return SortDoctors(doctorDtos, sortBy);
        }

        public async Task<DoctorEditDto> GetDoctorByIdAsync(int id)
        {
            var doctor = await doctorRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Doctor not found");

            return mapper.Map<DoctorEditDto>(doctor);
        }

        public async Task CreateDoctorAsync(DoctorEditDto doctorDto)
        {
            await TryValidateData(doctorDto);

            var doctor = mapper.Map<Doctor>(doctorDto);

            await doctorRepository.AddAsync(doctor);
        }

        public async Task UpdateDoctorAsync(int id, DoctorEditDto doctorDto)
        {
            await TryValidateData(doctorDto);

            var doctor = await doctorRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Doctor not found");

            doctor.CabinetId = doctorDto.CabinetId;
            doctor.SpecializationId = doctorDto.SpecializationId;
            doctor.UchastokId = doctorDto.UchastokId;

            await doctorRepository.UpdateAsync(doctor);
        }

        public async Task DeleteDoctorAsync(int id)
        {
            await doctorRepository.DeleteAsync(id);
        }

        private async Task TryValidateData(DoctorEditDto doctorDto)
        {
            if (doctorDto.CabinetId != null && !await cabinetRepository.ExistsAsync(doctorDto.CabinetId.Value))
                throw new ArgumentException("Cabinet with specified ID does not exist.");

            if (doctorDto.SpecializationId != null && !await specializationRepository.ExistsAsync(doctorDto.SpecializationId.Value))
                throw new ArgumentException("Specialization with specified ID does not exist.");

            if (doctorDto.UchastokId != null && !await uchastokRepository.ExistsAsync(doctorDto.UchastokId.Value))
                throw new ArgumentException("Uchastok with specified ID does not exist.");
        }

        private static IEnumerable<DoctorListDto> SortDoctors(IEnumerable<DoctorListDto> doctors, string sortBy) => sortBy switch
        {
            "CabinetNumber" => doctors.OrderBy(d => d.CabinetNumber),
            "SpecializationName" => doctors.OrderBy(d => d.SpecializationName),
            "UchastokNumber" => doctors.OrderBy(d => d.UchastokNumber),
            _ => doctors.OrderBy(d => d.Id)
        };
    }
}
