using Microsoft.EntityFrameworkCore;
using TestTask.Domain.Interfaces;
using TestTask.Infrastructure.Data;

namespace TestTask.Infrastructure.Repositories
{
    public class CabinetRepository(ApplicationDbContext context) : ICabinetRepository
    {
        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Cabinets.AnyAsync(c => c.Id == id);
        }
    }
}
