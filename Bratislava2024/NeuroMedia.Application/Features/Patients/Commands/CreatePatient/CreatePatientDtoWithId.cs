using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Application.Common.Mappings;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Patients.Commands.CreatePatient
{
    public class CreatePatientDtoWithId : CreatePatientDto, IMapFrom<Patient>
    {
        public int Id { get; init; }

        //public void Mapping(Profile profile)
        //{
        //    profile.CreateMap<Patient, CreatePatientDtoWithId>()
        //            .ReverseMap();
        //}
    }
}
