using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TestTask.Domain.Entities.Persons;
using TestTask.Infrastructure.Data;

namespace TestTask.Infrastructure.Repositories.Persons
{
    public class DoctorRepository : BasePersonRepository<Doctor>
    {
        private static readonly string DoctorsCacheKeyPrefix = "DoctorsCache";

        public DoctorRepository(ApplicationDbContext context, IMemoryCache cache)
            : base(context, cache)
        {
        }

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

        protected override string GetCacheKey(int pageNumber, int pageSize, string sortBy)
        {
            return $"{DoctorsCacheKeyPrefix}_{pageNumber}_{pageSize}_{sortBy}";
        }
    }
}