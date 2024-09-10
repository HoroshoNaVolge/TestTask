using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TestTask.Domain.Entities.Persons;
using TestTask.Infrastructure.Data;

namespace TestTask.Infrastructure.Repositories.Persons
{
    public class PatientRepository(ApplicationDbContext context, IMemoryCache cache) : BasePersonRepository<Patient>(context, cache)
    {
        private static readonly string PatientsCacheKeyPrefix = "PatientsCache";

        protected override IQueryable<Patient> ApplyIncludes(IQueryable<Patient> query)
        {
            return query.Include(p => p.Uchastok);
        }

        protected override IQueryable<Patient> ApplySorting(IQueryable<Patient> query, string sortBy)
        {
            return sortBy switch
            {
                "UchastokNumber" => query.OrderBy(p => p.Uchastok!.Number),
                _ => query.OrderBy(p => p.Id)
            };
        }

        protected override string GetCacheKey(int pageNumber, int pageSize, string sortBy)
        {
            return $"{PatientsCacheKeyPrefix}_{pageNumber}_{pageSize}_{sortBy}";
        }
    }
}