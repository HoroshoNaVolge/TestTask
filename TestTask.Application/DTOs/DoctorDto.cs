using System.Text.Json.Serialization;

namespace TestTask.Application.DTOs
{
    public class DoctorListDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? CabinetNumber { get; set; }
        public string? SpecializationName { get; set; }
        public string? UchastokNumber { get; set; }
    }

    public class DoctorEditDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int? CabinetId { get; set; }
        public int? SpecializationId { get; set; }
        public int? UchastokId { get; set; }
    }
}
