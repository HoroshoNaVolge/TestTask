using Microsoft.AspNetCore.Mvc;
using TestTask.Application.Interfaces;
using TestTask.Application.DTOs;
using TestTask.Domain.Entities.Persons;

namespace TestTask.Api.Controllers
{
    [Route("api/[controller]")]
    public class PatientsController(IBaseService<PatientListDto, PatientEditDto, PatientCreateDto, Patient> patientService, ILogger<PatientsController> logger)
        : BaseController<PatientListDto, PatientEditDto, PatientCreateDto, Patient>(patientService, logger)
    {
    }
}
