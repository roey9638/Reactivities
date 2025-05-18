using Application.Core;
using MediatR;
using Persistence;

namespace Application.Activities.Commands
{
    public class DeleteActivity
    {
        public class Command: IRequest<Result<Unit>>
        {
            public required string Id { get; set; } 
        }

        public class Handler(AppDbContext context) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await context.Activities.FindAsync([request.Id], cancellationToken);

                if (activity == null)
                {
                    return Result<Unit>.Failure("Activity Not Found", 404);
                }

                context.Remove(activity);

                
                /* // The [Result] is a [bool]: [true] if rows were [affected] (i.e., save was successful), false otherwise.
                // The [SaveChangesAsync] [returns] an [int], which is the [number] of [rows] [affected] in the [database] VVV
                // This [checks] whether more than [0] [rows] were affected, indicating that [something] was actually [saved]. */
                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if (!result)
                {
                    return Result<Unit>.Failure("Failed To Delete The Activity", 400);
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}