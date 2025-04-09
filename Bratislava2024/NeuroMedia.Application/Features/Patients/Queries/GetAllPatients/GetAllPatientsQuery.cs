using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Patients.Queries.GetAllPatients
{
    public record GetAllPatientsQuery : IRequest<List<GetAllPatientsDto>>;

    internal class GetAllPatientsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllPatientsQuery, List<GetAllPatientsDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<List<GetAllPatientsDto>> Handle(GetAllPatientsQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Patient>().Entities
                   .ProjectTo<GetAllPatientsDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);
        }
    }
}
