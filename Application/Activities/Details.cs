using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        // The [Query] DO [return] [Data]. That's why in the [IRequest<>] we have the [<Activity>]
        public class Query: IRequest<Activity> 
        {
            // Here i added [Id] because i want to know the [Specific Activity]. 
            //And it will be [Available] in the [Handler]
            public Guid Id { get; set; }
        }
        

        public class Handler : IRequestHandler<Query, Activity>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Activity> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Activities.FindAsync(request.Id);
            }
        }
    }
}