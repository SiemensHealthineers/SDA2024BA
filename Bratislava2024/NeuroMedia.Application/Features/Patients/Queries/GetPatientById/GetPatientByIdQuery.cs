using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Patients.Queries.GetPatientById
{
    public record GetPatientByIdQuery(int Id) : IRequest<GetPatientByIdDto>;

    internal class GetPatientByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetPatientByIdQuery, GetPatientByIdDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<GetPatientByIdDto> Handle(GetPatientByIdQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Patient>().Entities
                .IgnoreQueryFilters()
                .ProjectTo<GetPatientByIdDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);
        }
    }
}
