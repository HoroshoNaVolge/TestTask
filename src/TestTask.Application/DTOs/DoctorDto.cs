using System.Text.Json.Serialization;

namespace TestTask.Application.DTOs
{
    public abstract class DoctorBaseDto
    {
        public int? CabinetId { get; set; }
        public int? SpecializationId { get; set; }
        public int? UchastokId { get; set; }
    }

    public class DoctorEditDto : DoctorBaseDto
    {
        [JsonIgnore]
        public int Id { get; set; }
    }

    public class DoctorCreateDto : DoctorBaseDto
    {
        public string? FullName { get; set; }
    }

    public class DoctorListDto
    {
        public int Id { get; set; }
        public string? CabinetNumber { get; set; }
        public string? SpecializationName { get; set; }
        public string? UchastokNumber { get; set; }
    }
}
