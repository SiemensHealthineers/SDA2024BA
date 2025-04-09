using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMediaMobileApp.Models.Entities;

namespace NeuroMediaMobileApp.Services.Interfaces
{
    public interface IPatientService
    {
        public Task<Patient> GetPatientDataAsync(int patientId);
        public Task<List<Patient>> GetAllPatientsDataAsync();
    }
}
