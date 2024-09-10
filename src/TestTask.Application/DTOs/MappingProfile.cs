using AutoMapper;
using TestTask.Domain.Entities.Persons;

namespace TestTask.Application.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor, DoctorBaseDto>()
                .ForMember(dest => dest.CabinetId, opt => opt.MapFrom(src => src.CabinetId))
                .ForMember(dest => dest.SpecializationId, opt => opt.MapFrom(src => src.SpecializationId))
                .ForMember(dest => dest.UchastokId, opt => opt.MapFrom(src => src.UchastokId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

            CreateMap<DoctorBaseDto, Doctor>()
                .ForMember(dest => dest.CabinetId, opt => opt.MapFrom(src => src.CabinetId))
                .ForMember(dest => dest.SpecializationId, opt => opt.MapFrom(src => src.SpecializationId))
                .ForMember(dest => dest.UchastokId, opt => opt.MapFrom(src => src.UchastokId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

            CreateMap<Doctor, DoctorListDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.CabinetNumber, opt => opt.MapFrom(src => src.Cabinet!.Number))
                .ForMember(dest => dest.SpecializationName, opt => opt.MapFrom(src => src.Specialization!.Name))
                .ForMember(dest => dest.UchastokNumber, opt => opt.MapFrom(src => src.Uchastok!.Number));


            CreateMap<DoctorEditDto, Doctor>()
                .IncludeBase<DoctorBaseDto, Doctor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Doctor, DoctorEditDto>()
                .IncludeBase<Doctor, DoctorBaseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<Patient, PatientBaseDto>()
                .ForMember(dest => dest.UchastokId, opt => opt.MapFrom(src => src.UchastokId));

            CreateMap<PatientBaseDto, Patient>()
                .ForMember(dest => dest.UchastokId, opt => opt.MapFrom(src => src.UchastokId));

            CreateMap<Patient, PatientListDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate));

            CreateMap<PatientBaseDto, Patient>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate));

            CreateMap<PatientEditDto, Patient>()
                .IncludeBase<PatientBaseDto, Patient>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Patient, PatientEditDto>()
                .IncludeBase<Patient, PatientBaseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
