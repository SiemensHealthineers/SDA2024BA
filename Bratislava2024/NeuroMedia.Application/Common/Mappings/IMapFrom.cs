using AutoMapper;
using AutoMapper.Internal;

namespace NeuroMedia.Application.Common.Mappings
{
    public interface IMapFrom<T>
    {
        virtual void Mapping(Profile profile)
        {
            profile.CreateMap(typeof(T), GetType())
                .ReverseMap();
        }
    }
}
