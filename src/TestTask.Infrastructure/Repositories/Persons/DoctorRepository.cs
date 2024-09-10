using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TestTask.Domain.Entities.Persons;
using TestTask.Infrastructure.Data;

namespace TestTask.Infrastructure.Repositories.Persons
{
    public class DoctorRepository(ApplicationDbContext context, IMemoryCache cache) : BasePersonRepository<Doctor>(context, cache)
    {
        private static readonly string DoctorsCacheKeyPrefix = "DoctorsCache";
        protected override string CacheKeyPrefix => DoctorsCacheKeyPrefix;

        protected override IQueryable<Doctor> ApplyIncludes(IQueryable<Doctor> query)
        {
            return query
                .Include(d => d.Uchastok)
                .Include(d => d.Cabinet)
                .Include(d => d.Specialization);
        }

        protected override IQueryable<Doctor> ApplySorting(IQueryable<Doctor> query, string sortBy)
        {
            return sortBy switch
            {
                "CabinetNumber" => query.OrderBy(d => d.Cabinet!.Number),
                "SpecializationName" => query.OrderBy(d => d.Specialization!.Name),
                "UchastokNumber" => query.OrderBy(d => d.Uchastok!.Number),
                _ => query.OrderBy(d => d.Id)
            };
        }
    }
}
