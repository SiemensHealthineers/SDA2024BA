using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

using AutoMapper;
using MediatR;
using NeuroMedia.Application.Features.Patients.Queries.GetPatientById;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Patients.Queries.GetAnyPatientById
{
    public record GetAnyPatientByIdQuery(int Id) : IRequest<GetPatientByIdDto>;

    internal class GetAnyPatientByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAnyPatientByIdQuery, GetPatientByIdDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<GetPatientByIdDto> Handle(GetAnyPatientByIdQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Patient>().Entities
                   .IgnoreQueryFilters()
                   .ProjectTo<GetPatientByIdDto>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);
        }
    }
}
