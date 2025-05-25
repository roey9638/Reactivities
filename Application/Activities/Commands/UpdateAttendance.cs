using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Commands;

public class UpdateAttendance
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string Id { get; set; }
    }

    public class Handler(IUserAccessor userAccessor, AppDbContext context) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities
                .Include(x => x.Attendees)
                .ThenInclude(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (activity == null) return Result<Unit>.Failure("Activity Not Found", 404);

            // Here i will [Get] the [Currently] [Loged In] [User]
            var user = await userAccessor.GetUserAsync();

            // [activity.Attendees] This is a list (or collection) of people [attending] an [activity].
            // This tries to [find] the [current] [user] in the [list] of [attendees] for an [activity].
            /* 
               // The [activity.Attendees] is A [list] of [people] [attending] the [activity]. 
               // The [.FirstOrDefault] [Looks through] that [list] and [Returns] the [first person] who [matches] the [condition]. If no one [matches], it returns [null] 
               // The [x => x.UserId == user.Id] it’s [looking] for [someone] whose [UserId] [matches] the [current] [user’s ID] 
            */
            var attendance = activity.Attendees.FirstOrDefault(x => x.UserId == user.Id);

            // [activity.Attendees] This is a list (or collection) of people [attending] an [activity].
            // So if the [x.IsHost &&] is [true] AND VVV
            // [x.UserId == user.Id] the [userId] is [currently] the [Loged In] [User]. [isHost] will be [true]
            /* 
               // The [x => x.IsHost && x.UserId == user.Id] [Looks] for [someone] who VVV
               // [x.IsHost] is [marked] as the [host] AND VVV
               // [x.UserId == user.Id] has the [same] [user ID] as the [current] [user] 
            */
            var isHost = activity.Attendees.Any(x => x.IsHost && x.UserId == user.Id);

            if (attendance != null)
            {
                if (isHost) activity.IsCancelled = !activity.IsCancelled;
                else activity.Attendees.Remove(attendance);
            }
            else
            {
                activity.Attendees.Add(new ActivityAttendee
                {
                    UserId = user.Id,
                    ActivityId = activity.Id,
                    IsHost = false
                });
            }

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            return result
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure("Problem updationg the DB", 400);
        }
    }
}
