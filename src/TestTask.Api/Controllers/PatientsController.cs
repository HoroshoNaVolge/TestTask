using Microsoft.AspNetCore.Mvc;
using TestTask.Application.Interfaces;
using TestTask.Application.DTOs;

namespace TestTask.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController(IPatientService patientService, ILogger<PatientsController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientListDto>>> GetPatients([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "UchastokName")
        {
            var patients = await patientService.GetPatientsAsync(pageNumber, pageSize, sortBy);
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientEditDto>> GetPatient(int id)
        {
            var patient = await patientService.GetPatientByIdAsync(id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePatient(PatientEditDto patientDto)
        {
            try
            {
                await patientService.CreatePatientAsync(patientDto);
                return CreatedAtAction(nameof(GetPatient), new { id = patientDto.Id }, patientDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while creating patient with ID {PatientId}", patientDto.Id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePatient(int id, PatientEditDto patientDto)
        {
            var existingpatient = await patientService.GetPatientByIdAsync(id);
            if (existingpatient == null)
                return NotFound();

            try
            {
                await patientService.UpdatePatientAsync(id, patientDto);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while updating patient with ID {PatientId}", patientDto.Id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient(int id)
        {
            var existingpatient = await patientService.GetPatientByIdAsync(id);
            if (existingpatient == null)
                return NotFound();

            try
            {
                await patientService.DeletePatientAsync(id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while deleting patient with ID {id}", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
