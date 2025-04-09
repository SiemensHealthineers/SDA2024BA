using MediatR;

using Microsoft.AspNetCore.Mvc;

using NeuroMedia.Application.Features.Questionnaires.Commands.UploadAnswers;
using NeuroMedia.Application.Features.Questionnaires.Commands.DeleteAnswers;
using NeuroMedia.Application.Features.Questionnaires.Queries.GetQuestionnaireByType;
using NeuroMedia.Application.Features.Questionnaires.Queries.GetQuestionnairesByVisitId;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Enums;
using NeuroMedia.Application.Features.Questionnaires.Commands.CreateQuestionnaireRecordsCommand;
using NeuroMedia.Application.Features.Questionnaires.Commands.EditQuestionnaireAnswers;
using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.Questionnaires.Queries.GetQuestionnaireResultsByBlobPath;
using NeuroMedia.API.Policies;

namespace NeuroMedia.API.Controllers
{
    [Route("api/[controller]")]
    public class QuestionnairesController(IMediator mediator) : ApiController
    {
        private readonly IMediator _mediator = mediator;
        [HttpGet("{type:int}")]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> GetQuestionnaireByType(QuestionnaireType type)
        {
            return await MediatorTrySend(_mediator, new GetQuestionnaireByTypeQuery(type, User, new LogRow().UpdateCallerProperties()));
        }

        [HttpPost("{visitId:int}/{type:int}")]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> CreateResultsForVisit(int visitId, QuestionnaireType type)
        {
            return await MediatorTrySend(_mediator, new CreateQuestionnaireRecordsCommand(visitId, type, User, new LogRow().UpdateCallerProperties()));
        }

        [HttpGet("Results/{visitId:int}")]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> GetAllQuestionnairesResultsByVisitId(int visitId)
        {
            return await MediatorTrySend(_mediator, new GetAllQuestionnairesResultsByVisitIdQuery(visitId, User, new LogRow().UpdateCallerProperties()));
        }

        [HttpGet("Results")]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> GetQuestionnaireResultsByBlobPath([FromQuery] string blobPath)
        {
            return await MediatorTrySend(_mediator, new GetQuestionnaireResultsByBlobPathQuery(blobPath, User, new LogRow().UpdateCallerProperties()));
        }

        [HttpPost("Results")]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> CreateResults(
            [FromQuery] string blobPath,
            [FromBody] AnswersDto dto)
        {
            return await MediatorTrySend(_mediator, new UploadAnswersCommand(blobPath, dto, User, new LogRow().UpdateCallerProperties()));
        }

        [HttpDelete("Results")]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> Delete([FromQuery] int questionnaireId)
        {
            return await MediatorTrySend(_mediator, new DeleteAnswersCommand(questionnaireId, User, new LogRow().UpdateCallerProperties()));
        }

        [HttpPut("Results")]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> EditResults([FromQuery] int questionnaireId, [FromBody] AnswersDto dto)
        {
            return await MediatorTrySend(_mediator,
                new EditAnswersCommand(questionnaireId, dto, User, new LogRow().UpdateCallerProperties()));
        }
    }
}
