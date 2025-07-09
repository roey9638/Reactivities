using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries
{
    public class GetActivityList
    {
        public class Query : IRequest<Result<PagedList<ActivityDto, DateTime?>>>
        {
            public required ActivityParams Params { get; set; }
        }

        // The [Query] in [IRequestHandler<Query, List<Activity>>]. Is to know what is [Query/Request]
        // And The [List<Activity>>] Is to know what to [Return].
        public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor) : IRequestHandler<Query, Result<PagedList<ActivityDto, DateTime?>>>
        {
            public async Task<Result<PagedList<ActivityDto, DateTime?>>> Handle(Query request, CancellationToken cancellationToken)
            {

                var query = context.Activities
                    .OrderBy(x => x.Date) // [Sort] the [activities] by their [date] (from oldest to newest).
                    /* Here i want to [get] [only] the [activities] that there [Date] is [>=] to the [request.Params.Cursor].
                    // If it's not [>=] then we [get] [only] the [activities] that there [Date] is [>=] then [request.Params.StartDate] */
                    .Where(x => x.Date >= (request.Params.Cursor ?? request.Params.StartDate))
                    /* [Converts] the [result] into a [queryable] [object], which means you can VVV
                    /  [add more] [filters] or [changes] [later] [before] [running] the [query]. */
                    .AsQueryable();


                if (!string.IsNullOrEmpty(request.Params.Filter))
                {
                    /* This code checks if the user added a filter (like "isGoing" or "isHost") to a request. 
                    // Then it updates the query to match that filter. */
                    query = request.Params.Filter switch
                    {
                        // Only include activities where the current user is one of the attendees.
                        "isGoing" => query.Where(x => x.Attendees.Any(a => a.UserId == userAccessor.GetUserId())),

                        // Only include activities where the current user is the host.
                        "isHost" => query.Where(x => x.Attendees.Any(a => a.IsHost && a.UserId == userAccessor.GetUserId())),

                        // If no known filter is used (like some random word), do nothing.
                        _ => query
                    };
                }


                var projectedActivities = query.ProjectTo<ActivityDto>(mapper.ConfigurationProvider,
                            new { currentUserId = userAccessor.GetUserId() });


                var activities = await projectedActivities
                        .Take(request.Params.PageSize + 1)
                        .ToListAsync(cancellationToken);



                // This will [store] the [date] of the [next] [page’s] [first item], if there is one.
                DateTime? nextCursor = null;


                /* [Check] if we got [more items] than the [request.PageSize]. 
                // If [yes], it [means] [there’s] [another page] after this one. */
                if (activities.Count > request.Params.PageSize)
                {
                    // Save the [last] [item’s] [date] as the [next] [page’s] [starting point]
                    nextCursor = activities.Last().Date;

                    // Remove that extra item (the "+1") so that only the correct number of items (PageSize) is returned.
                    activities.RemoveAt(activities.Count - 1);
                }


                return Result<PagedList<ActivityDto, DateTime?>>.Success
                (
                    new PagedList<ActivityDto, DateTime?>
                    {
                        Items = activities,
                        NextCursor = nextCursor
                    }
                );
            }
        }
    }
}