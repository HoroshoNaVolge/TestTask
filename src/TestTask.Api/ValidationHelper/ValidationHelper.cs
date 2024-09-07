using Microsoft.AspNetCore.Mvc;

namespace TestTask.Api.Validation
{
    public static class ValidationHelper
    {
        public static ActionResult ValidatePageParameters(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                return new BadRequestObjectResult("Page number must be greater than zero.");

            if (pageSize <= 0)
                return new BadRequestObjectResult("Page size must be greater than zero.");
            
            return null!;
        }
    }
}
