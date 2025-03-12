using Application.Activities.DTOs;
using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Commands
{
    public class CreateActivity
    {
        // The [Command] DO NOT [return] anything. That's why in the [IRequest] we don't have anything
        public class Command: IRequest<Result<string>>
        {
            // Here i added [Activity] because i want to Add [a new Activity] . 
            //And it will be [Available] in the [Handler]
            public required CreateActivityDto ActivityDto { get; set; }
        }

        // We don't have [anything] after the [Command] Because we [Don't] [return] anything.
        public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<string>>
        {
            public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = mapper.Map<Activity>(request.ActivityDto);

                context.Activities.Add(activity);

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                // Here I'm [Checking] for any [Errors]
                if (!result)
                {
                    return Result<string>.Failure("Failed To Create The Activity", 400);
                }

                return Result<string>.Success(activity.Id);
            }
        }
    }
}