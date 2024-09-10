using System.Text.Json.Serialization;
using static TestTask.Application.Validation.ValidationHelper;

namespace TestTask.Application.DTOs
{
    public class PatientBaseDto
    {
        public int? UchastokId { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }

        [ValidDate(120, ErrorMessage = "Date cannot be more than 120 years old.")]
        public DateTime BirthDate { get; set; } = DateTime.UtcNow;
    }

    public class PatientEditDto : PatientBaseDto
    {
        [JsonIgnore]
        public int Id { get; set; }
    }

    public class PatientListDto : PatientBaseDto
    {
        public int Id { get; set; }
        public string? UchastokNumber { get; set; }  // Значение из связанной таблицы, а не ID
    }
}
