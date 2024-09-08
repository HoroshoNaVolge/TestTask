using TestTask.Domain.Entities;
using TestTask.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TestTask.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace TestTask.Infrastructure.Repositories
{
    public class DoctorRepository(ApplicationDbContext context, IMemoryCache cache) : IDoctorRepository
    {
        private static readonly string DoctorsCacheKey = "DoctorsCache";

        public async Task<IReadOnlyCollection<Doctor>> GetAllAsync(int pageNumber, int pageSize, string sortBy, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var cacheKey = $"{DoctorsCacheKey}_{pageNumber}_{pageSize}_{sortBy}";

            if (cache.TryGetValue(cacheKey, out IReadOnlyCollection<Doctor>? cachedDoctors))
                return cachedDoctors!;

            IQueryable<Doctor> query = context.Doctors
                .Include(d => d.Uchastok)
                .Include(d => d.Cabinet)
                .Include(d => d.Specialization);

            query = ApplySorting(query, sortBy);

            var doctors = await query
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync(cancellationToken);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };

            cache.Set(cacheKey, doctors, cacheEntryOptions);

            return doctors;
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await context.Doctors
                .Include(d => d.Cabinet)
                .Include(d => d.Specialization)
                .Include(d => d.Uchastok)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<int> AddAsync(Doctor doctor)
        {
            context.Doctors.Add(doctor);
            await context.SaveChangesAsync();
           
            cache.Remove(DoctorsCacheKey);

            return doctor.Id;
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            context.Doctors.Update(doctor);
            await context.SaveChangesAsync();

            cache.Remove(DoctorsCacheKey);
        }

        public async Task DeleteAsync(int id)
        {
            var doctor = await context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                context.Doctors.Remove(doctor);
                await context.SaveChangesAsync();
                
                cache.Remove(DoctorsCacheKey);
            }
        }

        private static IQueryable<Doctor> ApplySorting(IQueryable<Doctor> query, string sortBy)
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
