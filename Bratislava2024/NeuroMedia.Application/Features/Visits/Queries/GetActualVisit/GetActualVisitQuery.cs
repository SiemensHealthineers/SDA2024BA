using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Visits.Queries.GetActualVisit
{
    public record GetActualVisitQuery(int PatientId) : IRequest<GetActualVisitDto?>;

    public class GetActualVisitQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetActualVisitQuery, GetActualVisitDto?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<GetActualVisitDto?> Handle(GetActualVisitQuery request, CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;
            return await _unitOfWork.Repository<Visit>().Entities
                .Include(x => x.Questionnaires)
                .Include(x => x.Videos)
                .IgnoreQueryFilters()
                .Where(x => x.PatientId == request.PatientId && x.DateOfVisit <= today)
                .OrderByDescending(x => x.DateOfVisit == today)
                .ThenByDescending(x => x.DateOfVisit <= today)
                .ThenByDescending(x => x.DateOfVisit)
                .ProjectTo<GetActualVisitDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }

    }
}
