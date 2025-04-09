using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Interfaces.Repositories
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient?> GetByEmailAsync(string email);
        Task<Patient?> GetByEmailAsync(string email, int excludeId);
    }
}
