using System.ComponentModel.DataAnnotations;
using TestTask.Domain.Interfaces.Common;

namespace TestTask.Application.Validation
{
    public static class ValidationHelper
    {
        public static void ValidatePageParameters(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Page number must be greater than zero.");

            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than zero.");
        }

        public class ValidDateAttribute : ValidationAttribute
        {
            private readonly int _maxAgeInYears;

            public ValidDateAttribute(int maxAgeInYears)
            {
                _maxAgeInYears = maxAgeInYears;
                ErrorMessage = $"Date cannot be more than {_maxAgeInYears} years old.";
            }

            protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
            {
                if (value is DateTime date)
                {
                    var currentDate = DateTime.Now;
                    var maxDate = currentDate.AddYears(-_maxAgeInYears);

                    if (date < maxDate)
                        return new ValidationResult(ErrorMessage);
                    
                    return ValidationResult.Success!;
                }

                return new ValidationResult("Invalid date format.");
            }
        }
        public static class EntityValidator
        {
            public static async Task EnsureExistsAsync<T>(ICommonRepository<T> repository, int? id, string entityName) where T : class
            {
                if (id != null && !await repository.ExistsAsync(id.Value))
                    throw new ArgumentException($"{entityName} with specified ID does not exist.");
            }
        }
    }
}
