using System.Text.Json.Serialization;
using static TestTask.Application.Validation.ValidationHelper;

namespace TestTask.Application.DTOs
{

    public abstract class PatientBaseDto
    {
        public int? UchastokId { get; set; }
    }
    public class PatientEditDto : PatientBaseDto
    {
        [JsonIgnore]
        public int Id { get; set; }
    }

    public class PatientCreateDto : PatientBaseDto
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; } 
        public string? MiddleName { get; set; }
        public string? Address { get; set; }

        [ValidDate(120, ErrorMessage = "Date cannot be more than 120 years old.")]
        public DateTime BirthDate { get; set; } = DateTime.UtcNow;
        public string? Gender { get; set; }
    }

    public class PatientListDto
    {
        public int Id { get; set; }
        public string? UchastokNumber { get; set; }
    }
}
