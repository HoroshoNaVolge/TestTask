using AutoMapper;
using TestTask.Domain.Entities;

namespace TestTask.Application.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor, DoctorListDto>()
                            .ForMember(dest => dest.CabinetNumber, opt => opt.MapFrom(src => src.Cabinet!.Number))
                            .ForMember(dest => dest.SpecializationName, opt => opt.MapFrom(src => src.Specialization!.Name))
                            .ForMember(dest => dest.UchastokNumber, opt => opt.MapFrom(src => src.Uchastok!.Number));

            CreateMap<DoctorListDto, Doctor>()
                .ForMember(dest => dest.CabinetId, opt => opt.MapFrom(src => src.CabinetNumber))
                .ForMember(dest => dest.SpecializationId, opt => opt.MapFrom(src => src.SpecializationName))
                .ForMember(dest => dest.UchastokId, opt => opt.MapFrom(src => src.UchastokNumber));

            CreateMap<Doctor, DoctorEditDto>()
                .ForMember(dest => dest.CabinetId, opt => opt.MapFrom(src => src.CabinetId))
                .ForMember(dest => dest.SpecializationId, opt => opt.MapFrom(src => src.SpecializationId))
                .ForMember(dest => dest.UchastokId, opt => opt.MapFrom(src => src.UchastokId));

            CreateMap<DoctorEditDto, Doctor>()
                .ForMember(dest => dest.CabinetId, opt => opt.MapFrom(src => src.CabinetId))
                .ForMember(dest => dest.SpecializationId, opt => opt.MapFrom(src => src.SpecializationId))
                .ForMember(dest => dest.UchastokId, opt => opt.MapFrom(src => src.UchastokId));

            CreateMap<Patient, PatientListDto>()
                .ForMember(dest => dest.UchastokNumber, opt => opt.MapFrom(src => src.Uchastok!.Number));

            CreateMap<PatientListDto, Patient>()
                .ForMember(dest => dest.UchastokId, opt => opt.MapFrom(src => src.UchastokNumber));

            CreateMap<Patient, PatientEditDto>()
                .ForMember(dest => dest.UchastokId, opt => opt.MapFrom(src => src.UchastokId));

            CreateMap<PatientEditDto, Patient>()
                .ForMember(dest => dest.UchastokId, opt => opt.MapFrom(src => src.UchastokId));
        }
    }
}
