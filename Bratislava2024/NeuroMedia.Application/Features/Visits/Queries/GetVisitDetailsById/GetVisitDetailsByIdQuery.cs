using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.VisitDetails.Queries.GetVisitDetailsById
{
    public record GetVisitDetailsByIdQuery(int PatientId, int Id) : IRequest<GetVisitDetailsByIdDto?>;

    internal class GetVisitDetailsByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetVisitDetailsByIdQuery, GetVisitDetailsByIdDto?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<GetVisitDetailsByIdDto?> Handle(GetVisitDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Visit>().Entities
                 .Include(x => x.Questionnaires)
                 .Include(x => x.Videos)
                 .IgnoreQueryFilters()
                 .Where(x => x.Id == request.Id && x.PatientId == request.PatientId)
                 .ProjectTo<GetVisitDetailsByIdDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
