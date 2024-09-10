using TestTask.Domain.Entities.Other;
using TestTask.Infrastructure.Data;

namespace TestTask.Infrastructure.Repositories.Other
{
    public class UchastokRepository(ApplicationDbContext context) : BaseCommonRepository<Uchastok>(context)
    {
    }
}
