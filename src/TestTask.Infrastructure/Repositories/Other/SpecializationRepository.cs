using TestTask.Domain.Entities.Other;
using TestTask.Infrastructure.Data;

namespace TestTask.Infrastructure.Repositories.Other
{
    public class SpecializationRepository(ApplicationDbContext context) : BaseCommonRepository<Specialization>(context)
    {
    }
}
