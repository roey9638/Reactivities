using Application.Activities.DTOs;
using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Commands
{
    public class EditActivity
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required EditActivityDto ActivityDto { get; set; }
        }

        public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await context.Activities.FindAsync([request.ActivityDto.Id], cancellationToken);

                if (activity == null) return Result<Unit>.Failure("Activity Not Found", 404);

                // This will [Mapp] the [properties] [FROM] [request.Activity] and [Update to] -> [activity]
                mapper.Map(request.ActivityDto, activity);

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if (!result)
                {
                    return Result<Unit>.Failure("Failed To Update The Activity", 400);
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}