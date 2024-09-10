using TestTask.Domain.Entities.Other;
using TestTask.Infrastructure.Data;

namespace TestTask.Infrastructure.Repositories.Other
{
    public class CabinetRepository(ApplicationDbContext context) : BaseCommonRepository<Cabinet>(context)
    {
    }
}
