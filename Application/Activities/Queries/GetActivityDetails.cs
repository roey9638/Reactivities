using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries
{
    public class GetActivityDetails
    {
        // The [Query] DO [return] [Data]. That's why in the [IRequest<>] we have the [Result<Activity>]
        public class Query : IRequest<Result<ActivityDto>>
        {
            // Here i added [Id] because i want to know the [Specific Activity]. 
            //And it will be [Available] in the [Handler]
            public required string Id { get; set; }
        }

        public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor) : IRequestHandler<Query, Result<ActivityDto>>
        {
            public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                // The [.ProjectTo<ActivityDto>(mapper.ConfigurationProvider)] VVV
                // This directly convert [Activities] into [ActivityDto] objects in the database query.
                var activity = await context.Activities
                    /* The [ProjectTo<>] is [Baiscally] the [.Select()] [Query] 
                        that [allows] us to [pick] [specific] [Properties] */
                    .ProjectTo<ActivityDto>(mapper.ConfigurationProvider,
                        new { currentUserId = userAccessor.GetUserId() })
                    .FirstOrDefaultAsync(x => request.Id == x.Id, cancellationToken);

                if (activity == null)
                {
                    return Result<ActivityDto>.Failure("Activity Not Found", 404);
                }

                return Result<ActivityDto>.Success(activity);
            }
        }
    }
}