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
using NeuroMedia.Application.Features.Patients.Queries.GetAllDeactivatedPatients;
using NeuroMedia.Application.Features.Patients.Queries.GetDeactivatedPatientById;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Enums;
using Microsoft.Graph.Security.Labels.RetentionLabels.Item.RetentionEventType;
using NeuroMedia.Application.Features.Patients.Commands.ReactivatePatient;
using NeuroMedia.Application.Features.Patients.Queries.GetPatientByEmail;

namespace NeuroMedia.API.Controllers
{
    [Route("api/[controller]")]
    public class PatientsController(IMediator mediator) : ApiController
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> GetAll()
        {
            return await MediatorTrySend(_mediator, new GetAllPatientsQuery());
        }

        [HttpGet("{id:int}")]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> GetById(int id)
        {
            return await MediatorTrySend(_mediator, new GetPatientByIdQuery(id));
        }

        [HttpGet("{email}")]
        //[CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            return await MediatorTrySend(_mediator, new GetPatientByEmailQuery(email));
        }

        [HttpPost]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> Create([FromBody] CreatePatientDto dto)
        {
            return await MediatorTrySend(_mediator, new CreatePatientCommand(dto, User, new LogRow().UpdateCallerProperties()));
        }

        [HttpPut("{id:int}")]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePatientDto dto)
        {
            return await MediatorTrySend(_mediator, new UpdatePatientCommand(id, dto, User, new LogRow().UpdateCallerProperties()));
        }

        [HttpDelete("{id:int}")]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> Delete(int id)
        {
            return await MediatorTrySend(_mediator, new DeletePatientCommand(id, new LogRow().UpdateCallerProperties()));
        }

        [HttpPut("deactivate/{id:int}")]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> Deactivate(int id)
        {
            return await MediatorTrySend(_mediator, new DeactivatePatientCommand(id, new LogRow().UpdateCallerProperties()));
        }

        [HttpPut("reactivate/{id:int}")]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> Reactivate(int id)
        {
            return await MediatorTrySend(_mediator, new ReactivatePatientCommand(id, new LogRow().UpdateCallerProperties()));
        }

        [HttpGet("deactivated")]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> GetAllDeactivated()
        {
            return await MediatorTrySend(_mediator, new GetAllDeactivatedPatientsQuery());
        }

        [HttpGet("deactivated/{id:int}")]
        [CustomRoleAuthorize(Roles.InstitutionGroup)]
        public async Task<IActionResult> GetDeactivatedById(int id)
        {
            return await MediatorTrySend(_mediator, new GetDeactivatedPatientByIdQuery(id));
        }
    }
}
