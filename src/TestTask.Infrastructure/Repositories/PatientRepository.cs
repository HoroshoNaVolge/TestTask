﻿using TestTask.Domain.Entities;
using TestTask.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TestTask.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace TestTask.Infrastructure.Repositories
{
    public class PatientRepository(ApplicationDbContext context, IMemoryCache cache) : IPatientRepository
    {
        private static readonly string PatientsCacheKey = "DoctorsCache";

        public async Task<IReadOnlyCollection<Patient>> GetAllAsync(int pageNumber, int pageSize, string sortBy, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var cacheKey = $"{PatientsCacheKey}_{pageNumber}_{pageSize}_{sortBy}";

            if (cache.TryGetValue(cacheKey, out IReadOnlyCollection<Patient>? cachedPatients))
                return cachedPatients!;

            IQueryable<Patient> query = context.Patients.Include(d => d.Uchastok);

            query = ApplySorting(query, sortBy);

            var patients = await query
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync(cancellationToken);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };

            cache.Set(cacheKey, patients, cacheEntryOptions);

            return patients;
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await context.Patients
                .Include(p => p.Uchastok)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> AddAsync(Patient patient)
        {
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
            return patient.Id;
        }

        public async Task UpdateAsync(Patient patient)
        {
            context.Patients.Update(patient);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var patient = await context.Patients.FindAsync(id);
            if (patient != null)
            {
                context.Patients.Remove(patient);
                await context.SaveChangesAsync();
            }
        }

        private static IQueryable<Patient> ApplySorting(IQueryable<Patient> query, string sortBy)
        {
            return sortBy switch
            {
                "UchastokNumber" => query.OrderBy(p => p.Uchastok!.Number),
                _ => query.OrderBy(p => p.Id)
            };
        }
    }
}
