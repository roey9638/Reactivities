using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Commands
{
    public class AddComment
    {
        public class Command : IRequest<Result<CommentDto>>
        {
            public required string Body { get; set; }
            public required string ActivityId { get; set; }
        }

        public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor) : IRequestHandler<Command, Result<CommentDto>>
        {
            public async Task<Result<CommentDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await context.Activities
                    .Include(x => x.Comments)
                    .ThenInclude(x => x.User)
                    .FirstOrDefaultAsync(x => x.Id == request.ActivityId, cancellationToken);

                if (activity == null) return Result<CommentDto>.Failure("Could Not Find Activity", 404);

                var user = await userAccessor.GetUserAsync();

                var comment = new Comment
                {
                    UserId = user.Id,
                    ActivityId = activity.Id,
                    Body = request.Body
                };

                activity.Comments.Add(comment);

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                return result
                    ? Result<CommentDto>.Success(mapper.Map<CommentDto>(comment)) // I'm [Mapping] the [(comment)] from [type] of [Comment] to [CommentDto]
                    : Result<CommentDto>.Failure("Failed To Add Comment", 400);
            }
        }
    }
}