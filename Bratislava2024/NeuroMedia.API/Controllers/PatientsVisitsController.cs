using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NeuroMedia.API.Policies;
using NeuroMedia.Application.Features.Patients.Commands.CreatePatient;
using NeuroMedia.Application.Features.Patients.Commands.DeactivatePatient;
using NeuroMedia.Application.Features.Patients.Commands.DeletePatient;
using NeuroMedia.Application.Features.Patients.Commands.UpdatePatient;
using NeuroMedia.Application.Features.Patients.Queries.GetAllPatients;
using NeuroMedia.Application.Features.Patients.Queries.GetPatientById;
using NeuroMedia.Application.Features.Visits.Commands.CreateVisit;
using NeuroMedia.Application.Features.Visits.Queries;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Enums;
namespace NeuroMedia.API.Controllers
{
    [Route("api/[controller]")]

    public class PatientsVisitsController(IMediator mediator) : ApiController
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{patientId:int}")]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> GetAll(int patientId)
        {
            return await MediatorTrySend(_mediator, new GetAllVisitsQuery(patientId));
        }
        [HttpPost]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> Create([FromBody] CreateVisitDto dto)
        {
            return await MediatorTrySend(_mediator, new CreateVisitCommand(dto, User, new LogRow().UpdateCallerProperties()));
        }
    }
}
