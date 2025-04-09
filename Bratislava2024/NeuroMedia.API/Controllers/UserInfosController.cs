using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NeuroMedia.API.Policies;
using NeuroMedia.Application.Features.UserInfos.Queries.GetAllUserInfos;
using NeuroMedia.Application.Features.UserInfos.Queries.GetUserInfoById;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.API.Controllers
{
    [Route("api/[controller]")]
    public class UserInfosController(IMediator mediator) : ApiController
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [CustomRoleAuthorize(Roles.UserManagementGroup)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllUserInfosQuery()));
        }

        [HttpGet("{id:int}")]
        [CustomRoleAuthorize(Roles.UserManagementGroup)]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _mediator.Send(new GetUserInfoByIdQuery(id)));
        }
    }
}
