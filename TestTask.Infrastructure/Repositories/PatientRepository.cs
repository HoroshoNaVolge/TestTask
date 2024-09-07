using TestTask.Domain.Entities;
using TestTask.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TestTask.Domain.Interfaces;

namespace TestTask.Infrastructure.Repositories
{
    public class PatientRepository(ApplicationDbContext context) : IPatientRepository
    {
        public async Task<IEnumerable<Patient>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await context.Patients
                .Include(p => p.Uchastok)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await context.Patients
                .Include(p => p.Uchastok)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Patient patient)
        {
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
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
    }
}
