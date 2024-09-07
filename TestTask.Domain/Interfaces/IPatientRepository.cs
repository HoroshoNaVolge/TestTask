﻿using TestTask.Domain.Entities;

namespace TestTask.Domain.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync(int pageNumber, int pageSize);
        Task<Patient?> GetByIdAsync(int id);
        Task AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(int id);
    }
}
