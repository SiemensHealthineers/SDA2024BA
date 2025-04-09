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

namespace NeuroMediaMobileApp.Services
{
    public class PatientService : IPatientService
    {
        private readonly IHttpClientService _httpClient;

        public PatientService(IHttpClientService httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Patient> GetPatientDataAsync(int patientId)
        {
            var response = await _httpClient.GetFromJsonAsync<Patient>($"Patients/{patientId}");
            Console.Write(response.Name);
            return response;
        }
        public async Task<List<Patient>> GetAllPatientsDataAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Patient>>("Patients");
                return response ?? new List<Patient>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<Patient>();
            }
        }
    }
}
