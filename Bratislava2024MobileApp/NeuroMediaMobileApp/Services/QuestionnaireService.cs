using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using NeuroMediaMobileApp.Models.DTOs;
using NeuroMediaMobileApp.Services.Interfaces;

namespace NeuroMediaMobileApp.Services
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly IHttpClientService _httpClient;

        public QuestionnaireService(IHttpClientService httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<QuestionnaireDto> GetQuestionnaireDataAsync(int type)
        {
            var response = await _httpClient.GetFromJsonAsync<QuestionnaireDto>($"Questionnaires/{type}");
            return response;
        }
        public async Task<HttpResponseMessage> SendQuestionnaireResults(string blobPath, AnswersDto dto)
        {
            return await _httpClient.PostAsJsonAsync($"Questionnaires/Results?blobPath={Uri.EscapeDataString(blobPath)}", dto);
        }
    }
}
