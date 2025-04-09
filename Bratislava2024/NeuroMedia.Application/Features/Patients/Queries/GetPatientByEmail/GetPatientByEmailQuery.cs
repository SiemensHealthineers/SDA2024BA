using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Patients.Queries.GetPatientByEmail
{
    public record GetPatientByEmailQuery(string Email) : IRequest<GetPatientByEmailDto>;

    internal class GetPatientByEmailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetPatientByEmailQuery, GetPatientByEmailDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<GetPatientByEmailDto> Handle(GetPatientByEmailQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Patient>().Entities
                .IgnoreQueryFilters()
                .ProjectTo<GetPatientByEmailDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Email == query.Email, cancellationToken);
        }
    }
}

