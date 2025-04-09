using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NeuroMediaMobileApp.Models.Entities;
using System.Net.Http.Json;
using NeuroMediaMobileApp.Services.Interfaces;
using NeuroMediaMobileApp.Models.DTOs;

namespace NeuroMediaMobileApp.Services
{
    public class VisitService : IVisitService
    {
        private readonly IHttpClientService _httpClient;

        public VisitService(IHttpClientService httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Visit>> GetAllVisitsDataAsync(int patientId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Visit>>($"PatientsVisits/{patientId}");
                return response ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return [];
            }
        }

        public async Task<GetActualVisitDto?> GetActualVisitAsync(int patientId)
        {
            return await _httpClient.GetFromJsonAsync<GetActualVisitDto>($"VisitDetails/{patientId}");
        }

        public async Task<List<PendingTaskDto>> GetPendingTasksAsync(int patientId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<PendingTaskDto>>($"VisitDetails/pending-tasks/{patientId}");
                return response ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return [];
            }
        }
    }
}
