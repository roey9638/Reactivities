using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Commands
{
    public class CreateActivity
    {
        // The [Command] DO NOT [return] anything. That's why in the [IRequest] we don't have anything
        public class Command: IRequest<Result<string>>
        {
            // Here i added [Activity] because i want to Add [a new Activity] . 
            //And it will be [Available] in the [Handler]
            public required CreateActivityDto ActivityDto { get; set; }
        }

        // We don't have [anything] after the [Command] Because we [Don't] [return] anything.
        public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor) : IRequestHandler<Command, Result<string>>
        {
            public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Here i will [Get] the [Currently] [Loged In] [User]
                var user = await userAccessor.GetUserAsync();

                var activity = mapper.Map<Activity>(request.ActivityDto);

                context.Activities.Add(activity);

                // Her I'm just Creatin a new [attendee]
                var attendee = new ActivityAttendee
                {
                    ActivityId = activity.Id, // The [new ActivityAttendee] his [ActivityId] will be the new [Activity (his Id)] that we [Creating]
                    UserId = user.Id, // The [UserId] will be the [Currently] [Loged In] [User]. Because we [Creating Activity]
                    IsHost = true // / The [IsHost] will be the [true]. Because we [Creating Activity]. So the [Currently] [Loged In] that [Creating Activity] will be the [host]
                };

                // Here I'm just [Adding] the [new] [attendee] to the [Attendees] [List / Collection] to the [current] [activity] that we [Creating Activity]
                activity.Attendees.Add(attendee);

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                // Here I'm [Checking] for any [Errors]
                if (!result)
                {
                    return Result<string>.Failure("Failed To Create The Activity", 400);
                }

                return Result<string>.Success(activity.Id);
            }
        }
    }
}