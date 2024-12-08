using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        // The [Command] DO NOT [return] anything. That's why in the [IRequest] we don't have anything
        public class Command: IRequest
        {
            // Here i added [Activity] because i want to Add [a new Activity] . 
            //And it will be [Available] in the [Handler]
            public Activity Activity { get; set; }
        }

        // We don't have [anything] after the [Command] Because we [Don't] [return] anything.
        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;             
            }

            
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Activities.Add(request.Activity);

                await _context.SaveChangesAsync();
            }

        }
    }
}