using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Commands
{
    public class CreateActivity
    {
        // The [Command] DO NOT [return] anything. That's why in the [IRequest] we don't have anything
        public class Command: IRequest<string>
        {
            // Here i added [Activity] because i want to Add [a new Activity] . 
            //And it will be [Available] in the [Handler]
            public required Activity Activity { get; set; }
        }

        // We don't have [anything] after the [Command] Because we [Don't] [return] anything.
        public class Handler(AppDbContext context) : IRequestHandler<Command, string>
        {
            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                context.Activities.Add(request.Activity);

                await context.SaveChangesAsync(cancellationToken);

                return request.Activity.Id;
            }
        }
    }
}