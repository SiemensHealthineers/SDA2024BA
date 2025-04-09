using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMediaMobileApp.Models.DTOs;

namespace NeuroMediaMobileApp.Services.Interfaces
{
    public interface IQuestionnaireService
    {
        public Task<QuestionnaireDto> GetQuestionnaireDataAsync(int type);
        public Task<HttpResponseMessage> SendQuestionnaireResults(string blobPath, AnswersDto dto);
    }
}
