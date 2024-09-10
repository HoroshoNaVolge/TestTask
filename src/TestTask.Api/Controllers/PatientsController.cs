using Microsoft.AspNetCore.Mvc;
using TestTask.Application.Interfaces;
using TestTask.Application.DTOs;
using TestTask.Application.Validation;
using TestTask.Domain.Entities.Persons;

namespace TestTask.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController(IBaseService<PatientListDto, PatientEditDto, PatientCreateDto, Patient> patientService, ILogger<PatientsController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientListDto>>> GetPatients(CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "UchastokNumber")
        {
            var validationResult = ValidationHelper.ValidatePageParameters(pageNumber, pageSize);
            if (validationResult != null)
                return validationResult;

            try
            {
                var patients = await patientService.GetAllAsync(pageNumber, pageSize, sortBy, cancellationToken);
                return Ok(patients);
            }

            catch (OperationCanceledException)
            {
                return NoContent();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientEditDto>> GetPatient(int id)
        {
            var patient = await patientService.GetByIdAsync(id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePatient(PatientCreateDto patientDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdPatientId = await patientService.CreateAsync(patientDto);
                return CreatedAtAction(nameof(GetPatient), new { id = createdPatientId }, new { id = createdPatientId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while creating patient");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePatient(int id, PatientEditDto patientDto)
        {
            try
            {
                await patientService.UpdateAsync(id, patientDto);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while updating patient with ID {patientDto.Id}", patientDto.Id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient(int id)
        {
            var existingpatient = await patientService.GetByIdAsync(id);
            if (existingpatient == null)
                return NotFound();

            try
            {
                await patientService.DeleteAsync(id);
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
