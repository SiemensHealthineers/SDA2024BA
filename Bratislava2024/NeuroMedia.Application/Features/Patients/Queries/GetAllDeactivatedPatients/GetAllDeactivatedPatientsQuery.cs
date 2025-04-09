using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

using MediatR;
using NeuroMedia.Application.Features.Patients.Queries.GetAllPatients;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Patients.Queries.GetAllDeactivatedPatients
{
    public record GetAllDeactivatedPatientsQuery : IRequest<List<GetAllPatientsDto>>;

    internal class GetAllDeactivatedPatientsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllDeactivatedPatientsQuery, List<GetAllPatientsDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<List<GetAllPatientsDto>> Handle(GetAllDeactivatedPatientsQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Patient>().Entities
                   .IgnoreQueryFilters()
                   .Where(p => !p.IsActive)
                   .ProjectTo<GetAllPatientsDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);
        }
    }
}
