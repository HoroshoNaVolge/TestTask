namespace TestTask.Domain.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public int? CabinetId { get; set; }
        public int? SpecializationId { get; set; }
        public int? UchastokId { get; set; }
        
        public Cabinet? Cabinet { get; set; }
        public Specialization? Specialization { get; set; }
        public Uchastok? Uchastok { get; set; }
    }
}
