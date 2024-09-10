using Microsoft.EntityFrameworkCore;
using TestTask.Domain.Interfaces.Common;
using TestTask.Infrastructure.Data;

namespace TestTask.Infrastructure.Repositories.Other
{
    public abstract class BaseCommonRepository<TEntity>(ApplicationDbContext context) : ICommonRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext context = context;

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Set<TEntity>().AnyAsync(e => EF.Property<int>(e, "Id") == id);
        }
    }
}
