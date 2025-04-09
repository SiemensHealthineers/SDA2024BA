using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroMedia.Domain.Entities;

using Microsoft.EntityFrameworkCore;

using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Persistence.Contexts;

namespace NeuroMedia.Persistence.Repositories
{
    public class PatientRepository(ApplicationDbContext dbContext) : GenericRepository<Patient>(dbContext), IPatientRepository
    {
        public async Task<Patient?> GetByEmailAsync(string email)
        {
            return await _dbContext.Patients
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(p => p.Email == email);
        }

        public async Task<Patient?> GetByEmailAsync(string email, int excludeId)
        {
            return await _dbContext.Patients
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(p => p.Email == email && p.Id != excludeId);
        }
    }
}
