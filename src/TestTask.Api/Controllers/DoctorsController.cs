using Microsoft.AspNetCore.Mvc;
using TestTask.Application.Interfaces;
using TestTask.Application.DTOs;
using TestTask.Domain.Entities.Persons;

namespace TestTask.Api.Controllers
{
    [Route("api/[controller]")]
    public class DoctorsController(
        IBaseService<DoctorListDto, DoctorEditDto, DoctorBaseDto, Doctor> doctorService, 
        ILogger<DoctorsController> logger) 
        : BaseController<DoctorListDto, DoctorEditDto, DoctorBaseDto, Doctor>(doctorService, logger)
    {
    }
}
