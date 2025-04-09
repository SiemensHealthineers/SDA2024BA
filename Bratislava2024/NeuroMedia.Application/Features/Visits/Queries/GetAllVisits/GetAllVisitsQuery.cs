using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using NeuroMedia.Application.Features.Patients.Queries.GetPatientById;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Visits.Queries
{
    public record GetAllVisitsQuery(int PatientId) : IRequest<List<GetAllVisitsDto>>;

    internal class GetAllVisitsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllVisitsQuery, List<GetAllVisitsDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;


        public async Task<List<GetAllVisitsDto>> Handle(GetAllVisitsQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Visit>().Entities
                 .IgnoreQueryFilters()
                .ProjectTo<GetAllVisitsDto>(_mapper.ConfigurationProvider)
                .Where(x => x.PatientId == query.PatientId)
                .ToListAsync(cancellationToken);
        }
    }

}
