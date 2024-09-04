using Microsoft.EntityFrameworkCore;
using TestTask.Domain.Interfaces;
using TestTask.Infrastructure.Data;

namespace TestTask.Infrastructure.Repositories
{
    public class SpecializationRepository(ApplicationDbContext context) : ISpecializationRepository
    {
        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Specializations.AnyAsync(s => s.Id == id);
        }
    }
}
