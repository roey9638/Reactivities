using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Persistence;

namespace Application.Profiles.Commands;

public class FollowToggle
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string TargetUseId { get; set; } // The [User] that's going to be [Followed]
    }

    public class Handler(AppDbContext context, IUserAccessor userAccessor) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var observer = await userAccessor.GetUserAsync(); // This is the [Follower] [User]

            var target = await context.Users.FindAsync([request.TargetUseId], cancellationToken); // This is the [User] that we [Gonna] [Follow]

            if (target == null) return Result<Unit>.Failure("Target User Not Found", 400);

            // It [checks] if [one] [user] is [following] [another] in the [database]
            var following = await context.UserFollowings.FindAsync([observer.Id, target.Id], cancellationToken);

            if (following == null) context.UserFollowings.Add(new UserFollowing
            {
                ObserverId = observer.Id,
                TargetId = target.Id
            });
            else context.UserFollowings.Remove(following);

            return await context.SaveChangesAsync(cancellationToken) > 0
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure("Problem updating following", 400);
        }
    }
}
