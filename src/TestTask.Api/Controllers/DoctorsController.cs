using Microsoft.AspNetCore.Mvc;
using TestTask.Application.Interfaces;
using TestTask.Application.DTOs;
using TestTask.Application.Validation;
using TestTask.Domain.Entities.Persons;

namespace TestTask.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController(IBaseService<DoctorListDto, DoctorEditDto, DoctorCreateDto, Doctor> doctorService, ILogger<DoctorsController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorListDto>>> GetDoctors(CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "UchastokNumber")
        {
            var validationResult = ValidationHelper.ValidatePageParameters(pageNumber, pageSize);
            if (validationResult != null)
                return validationResult;

            try
            {
                var doctors = await doctorService.GetAllAsync(pageNumber, pageSize, sortBy, cancellationToken);
                return Ok(doctors);
            }
            catch (OperationCanceledException)
            {
                return NoContent();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorEditDto>> GetDoctor(int id)
        {
            var doctor = await doctorService.GetByIdAsync(id);
            if (doctor == null) return NotFound();
            return Ok(doctor);
        }

        [HttpPost]
        public async Task<ActionResult> CreateDoctor(DoctorCreateDto doctorDto)
        {
            try
            {
                var createdDoctorId = await doctorService.CreateAsync(doctorDto);
                return CreatedAtAction(nameof(GetDoctor), new { id = createdDoctorId }, new { id = createdDoctorId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while creating doctor");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDoctor(int id, DoctorEditDto doctorDto)
        {
            try
            {
                await doctorService.UpdateAsync(id, doctorDto);
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
                logger.LogError(ex, "An unexpected error occurred while updating doctor with ID {DoctorId}", doctorDto.Id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDoctor(int id)
        {
            var existingDoctor = await doctorService.GetByIdAsync(id);
            if (existingDoctor == null)
                return NotFound();

            try
            {
                await doctorService.DeleteAsync(id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while deleting doctor with ID {id}", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
