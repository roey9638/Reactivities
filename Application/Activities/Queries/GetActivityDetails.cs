using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Queries
{
    public class GetActivityDetails
    {
        // The [Query] DO [return] [Data]. That's why in the [IRequest<>] we have the [Result<Activity>]
        public class Query: IRequest<Result<Activity>> 
        {
            // Here i added [Id] because i want to know the [Specific Activity]. 
            //And it will be [Available] in the [Handler]
            public required string Id { get; set; }
        }

        public class Handler(AppDbContext context) : IRequestHandler<Query, Result<Activity>>
        {
            public async Task<Result<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await context.Activities.FindAsync([request.Id], cancellationToken);

                if (activity == null)
                {
                    return Result<Activity>.Failure("Activity Not Found", 404);
                }

                return Result<Activity>.Success(activity);
            }
        }
    }
}