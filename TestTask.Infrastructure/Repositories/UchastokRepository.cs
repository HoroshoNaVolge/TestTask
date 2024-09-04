using Microsoft.EntityFrameworkCore;
using TestTask.Domain.Interfaces;
using TestTask.Infrastructure.Data;

namespace TestTask.Infrastructure.Repositories
{
    public class UchastokRepository(ApplicationDbContext context) : IUchastokRepository
    {
        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Uchastoks.AnyAsync(u => u.Id == id);
        }
    }
}
