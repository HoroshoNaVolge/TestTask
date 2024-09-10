using TestTask.Domain.Entities.Other;

namespace TestTask.Domain.Entities.Persons
{
    public class Patient : IEntity
    {
        public int Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? Address { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Gender { get; set; }
        public int? UchastokId { get; set; }

        public Uchastok? Uchastok { get; set; }
    }
}
