using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Here [AutoMapper] [Mapping] the [properties] inside the [class] [Activity].
            CreateMap<Activity, Activity>();
        }
    }
}