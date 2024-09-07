using TestTask.Domain.Entities;
using TestTask.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TestTask.Domain.Interfaces;

namespace TestTask.Infrastructure.Repositories
{
    public class DoctorRepository(ApplicationDbContext context) : IDoctorRepository
    {
        public async Task<IEnumerable<Doctor>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await context.Doctors
                .Include(d =>d.Uchastok)
                .Include(d => d.Cabinet)
                .Include(d => d.Specialization)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await context.Doctors
                .Include(d => d.Cabinet)
                .Include(d => d.Specialization)
                .Include(d=>d.Uchastok)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddAsync(Doctor doctor)
        {
            context.Doctors.Add(doctor);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            context.Doctors.Update(doctor);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var doctor = await context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                context.Doctors.Remove(doctor);
                await context.SaveChangesAsync();
            }
        }
    }
}
