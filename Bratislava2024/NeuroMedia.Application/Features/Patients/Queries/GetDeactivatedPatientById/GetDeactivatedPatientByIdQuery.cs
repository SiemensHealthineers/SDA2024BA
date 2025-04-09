using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

using MediatR;
using NeuroMedia.Application.Features.Patients.Queries.GetPatientById;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Patients.Queries.GetDeactivatedPatientById
{
    public record GetDeactivatedPatientByIdQuery(int Id) : IRequest<GetPatientByIdDto>;

    internal class GetDeactivatedPatientByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetDeactivatedPatientByIdQuery, GetPatientByIdDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<GetPatientByIdDto> Handle(GetDeactivatedPatientByIdQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Patient>().Entities
                   .IgnoreQueryFilters()
                   .Where(p => !p.IsActive && p.Id == query.Id)
                   .ProjectTo<GetPatientByIdDto>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
