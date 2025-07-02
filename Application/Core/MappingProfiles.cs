using Application.Activities.DTOs;
using Application.Profiles.DTOs;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, Activity>();
            CreateMap<CreateActivityDto, Activity>();
            CreateMap<EditActivityDto, Activity>();


            /* // The [.ForMember(d => d.HostDisplayName, o => o.MapFrom(s =>  s.Attendees.FirstOrDefault(x => x.IsHost)!.User.DisplayName))] 
            // It [finds] the [host] from the [list] of [Attendees] in the [Activity] (the one where IsHost == true).
            // Then, it [takes] the [host's] [User.DisplayName] and [sets] that as the [HostDisplayName] in the [ActivityDto].  */
            CreateMap<Activity, ActivityDto>() // The (d) is for [detination]. And (o) if for [Options] */
                .ForMember(d => d.HostDisplayName, o => o.MapFrom(s =>
                    s.Attendees.FirstOrDefault(x => x.IsHost)!.User.DisplayName)) // It finds the [first attendee] where [IsHost] is [true]. Then it [gets] that [attendee’s] [User.DisplayName].
                .ForMember(d => d.HostId, o => o.MapFrom(s =>
                    s.Attendees.FirstOrDefault(x => x.IsHost)!.User.Id));


            /* 
                // I'm  [Mapping] from [ActivityAttendee] to [UserProfile] [Because] VVV
                // When I'm [Mapping] from [Activity] to [ActivityDto] From [Above]^^^^. VVV
                // The [Activity] has a [Collection] of [ActivityAttendee] So VVV
                // I'm [Mapping] the [Collection] of [ActivityAttendee] that the [Activity] has. VVV
                // To the [Collection] of [UserProfile] that the [ActivityDto].
            */
            CreateMap<ActivityAttendee, UserProfile>() // This starts a map from ActivityAttendee to UserProfile.
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName)) // It Copies the user's display name into UserProfile.DisplayName
            .ForMember(d => d.Bio, o => o.MapFrom(s => s.User.Bio)) // Copies the user's bio into UserProfile.Bio
            .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.User.ImageUrl)) // Copies the user's image URL into UserProfile.ImageUrl
            .ForMember(d => d.Id, o => o.MapFrom(s => s.User.Id)); // Copies the user's ID into UserProfile.Id


            CreateMap<User, UserProfile>();


            CreateMap<Comment, CommentDto>()
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
            .ForMember(d => d.UserId, o => o.MapFrom(s => s.User.Id))
            .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.User.ImageUrl));
        }
    }

    //.ForMember(d => d.HostDisplayName, o => o.MapFrom(s => 
    //s.Attendees.FirstOrDefault(x => x.IsHost)!.User.DisplayName))
}