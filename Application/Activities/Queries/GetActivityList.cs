using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries
{
    public class GetActivityList
    {
        public class Query: IRequest<List<Activity>> {}

        // The [Query] in [IRequestHandler<Query, List<Activity>>]. Is to know what is [Query/Request]
        // And The [List<Activity>>] Is to know what to [Return].
        public class Handler(AppDbContext context) : IRequestHandler<Query, List<Activity>>
        {
            public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await context.Activities.ToListAsync(cancellationToken);
            }
        }
    }
}