using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using NeuroMedia.Application.Features.UserInfos.Queries.GetAllUserInfos;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.UserInfos.Queries.GetUserInfoById
{
    public record GetUserInfoByIdQuery(int Id) : IRequest<GetUserInfoByIdDto>;

    internal class GetUserInfoByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetUserInfoByIdQuery, GetUserInfoByIdDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<GetUserInfoByIdDto> Handle(GetUserInfoByIdQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<UserInfo>().Entities
                   .ProjectTo<GetUserInfoByIdDto>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);
        }
    }
}
