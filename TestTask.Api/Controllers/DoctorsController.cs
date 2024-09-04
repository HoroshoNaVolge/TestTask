using Microsoft.AspNetCore.Mvc;
using TestTask.Application.Interfaces;
using TestTask.Application.DTOs;

namespace TestTask.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController(IDoctorService doctorService, ILogger<DoctorsController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorListDto>>> GetDoctors([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "FullName")
        {
            var doctors = await doctorService.GetDoctorsAsync(pageNumber, pageSize, sortBy);
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorEditDto>> GetDoctor(int id)
        {
            var doctor = await doctorService.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound();
            return Ok(doctor);
        }

        [HttpPost]
        public async Task<ActionResult> CreateDoctor(DoctorEditDto doctorDto)
        {
            try
            {
                await doctorService.CreateDoctorAsync(doctorDto);
                return CreatedAtAction(nameof(GetDoctor), new { id = doctorDto.Id }, doctorDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while creating doctor with ID {DoctorId}", doctorDto.Id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDoctor(int id, DoctorEditDto doctorDto)
        {
            var existingDoctor = await doctorService.GetDoctorByIdAsync(id);
            if (existingDoctor == null)
                return NotFound();
            try
            {
                await doctorService.UpdateDoctorAsync(id, doctorDto);
                return Ok();
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
            var existingDoctor = await doctorService.GetDoctorByIdAsync(id);
            if (existingDoctor == null)
                return NotFound();

            try
            {
                await doctorService.DeleteDoctorAsync(id);
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
