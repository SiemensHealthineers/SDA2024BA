using MediatR;

using Microsoft.AspNetCore.Mvc;

using NeuroMedia.API.Policies;
using NeuroMedia.Application.Features.VisitDetails.Queries.GetVisitDetailsById;
using NeuroMedia.Application.Features.Visits.Queries.GetActualVisit;
using NeuroMedia.Domain.Enums;
using NeuroMedia.Application.Features.Visits.Queries.GetPendingTasks;

namespace NeuroMedia.API.Controllers
{
    [Route("api/[controller]")]
    public class VisitDetailsController(IMediator mediator) : ApiController
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{patientId:int}/{id:int}")]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> GetById(int patientId, int id)
        {
            return await MediatorTrySend(_mediator, new GetVisitDetailsByIdQuery(patientId, id));
        }

        [HttpGet("{patientId:int}")]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> GetActualVisit(int patientId)
        {
            return await MediatorTrySend(_mediator, new GetActualVisitQuery(patientId));
        }

        [HttpGet("pending-tasks/{patientId:int}")]
        //[CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> GetPendingTasks(int patientId)
        {
            return await MediatorTrySend(_mediator, new GetPendingTasksQuery(patientId));
        }
    }
}
