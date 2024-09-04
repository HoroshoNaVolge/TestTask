using System.Text.Json.Serialization;

namespace TestTask.Application.DTOs
{
    public class PatientListDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? UchastokNumber { get; set; }
    }

    public class PatientEditDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int? UchastokId { get; set; }
    }
}
