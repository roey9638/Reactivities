using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security;

public class IsHostRequirement : IAuthorizationRequirement
{

}

public class IsHostRequirementHandler(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<IsHostRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
    {
        // Here I'm [Getting] the [Currently] [Loged In] [User]
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null) return;

        // We use to to have [Access] to the [Root Values] in are [Controllers]. VVV
        // For [Example] -> in the [ActivitiesController] a [Root Value] will be [HttpPut("{id}")].
        var httpContext = httpContextAccessor.HttpContext;

        // If the [id] [value] [is not string] then will simply [return]. VVV
        // But if it is a [string] then the [id] [value] will be [assigned] to the [activityId].
        if (httpContext?.GetRouteValue("id") is not string activityId) return;

        // Here [x.UserId == userId] we want to [Grab] the [Currently] [Loged In] [attendee]. VVV
        // And also Here [x.ActivityId == activityId] we want to [Grab] the [activity] that [Passed] in the [Root Value].
        /* 
            // [x.UserId == userId] If the [Currently] [Loged In] is [true] FOR EXAMPLE --> [Loged In] as [Bob] VVV
            // But [x.ActivityId == activityId] we [PASS] An [Activity] that [Bob] [isn't] [attending]. Then [attendee] will be [false]
        */
        var attendee = await dbContext.ActivityAttendees
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == userId && x.ActivityId == activityId);

        if (attendee == null) return;

        if (attendee.IsHost) context.Succeed(requirement);
    }
}
