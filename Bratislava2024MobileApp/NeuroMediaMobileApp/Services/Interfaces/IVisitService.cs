using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMediaMobileApp.Models.DTOs;
using NeuroMediaMobileApp.Models.Entities;

namespace NeuroMediaMobileApp.Services.Interfaces
{
    public interface IVisitService
    {
        public Task<List<Visit>> GetAllVisitsDataAsync(int patientId);
        public Task<GetActualVisitDto?> GetActualVisitAsync(int patientId);
        public Task<List<PendingTaskDto>> GetPendingTasksAsync(int patientId);
    }
}
