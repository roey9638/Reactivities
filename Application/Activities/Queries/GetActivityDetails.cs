using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Queries
{
    public class GetActivityDetails
    {
        // The [Query] DO [return] [Data]. That's why in the [IRequest<>] we have the [<Activity>]
        public class Query: IRequest<Activity> 
        {
            // Here i added [Id] because i want to know the [Specific Activity]. 
            //And it will be [Available] in the [Handler]
            public required string Id { get; set; }
        }

        public class Handler(AppDbContext context) : IRequestHandler<Query, Activity>
        {
            public async Task<Activity> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await context.Activities.FindAsync([request.Id], cancellationToken);

                if (activity == null)
                {
                    throw new Exception("Activity Not Found");
                }

                return activity;
            }
        }
    }
}