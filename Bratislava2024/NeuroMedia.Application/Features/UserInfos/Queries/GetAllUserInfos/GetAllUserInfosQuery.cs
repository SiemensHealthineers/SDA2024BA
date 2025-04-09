using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.UserInfos.Queries.GetAllUserInfos
{
    public record GetAllUserInfosQuery : IRequest<List<GetAllUserInfosDto>>;

    internal class GetAllUserInfosQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllUserInfosQuery, List<GetAllUserInfosDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<List<GetAllUserInfosDto>> Handle(GetAllUserInfosQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<UserInfo>().Entities
                   .ProjectTo<GetAllUserInfosDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);
        }
    }
}
