using Application.Activities.DTOs;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries
{
    public class GetActivityList
    {
        public class Query : IRequest<List<ActivityDto>> { }

        // The [Query] in [IRequestHandler<Query, List<Activity>>]. Is to know what is [Query/Request]
        // And The [List<Activity>>] Is to know what to [Return].
        public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor) : IRequestHandler<Query, List<ActivityDto>>
        {
            public async Task<List<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await context.Activities
                    .ProjectTo<ActivityDto>(mapper.ConfigurationProvider,
                        new { currentUserId = userAccessor.GetUserId() })
                    .ToListAsync(cancellationToken);
            }
        }
    }
}