using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistence;

namespace Application.Profiles.Commands
{
    public class AddPhoto
    {
        public class Command : IRequest<Result<Photo>>
        {
            public required IFormFile File { get; set; }
        }

        public class Handler(IUserAccessor userAccessor, AppDbContext context, IPhotoService photoService) : IRequestHandler<Command, Result<Photo>>
        {
            public async Task<Result<Photo>> Handle(Command request, CancellationToken cancellationToken)
            {
                var uploadResults = await photoService.UploadPhoto(request.File);

                if (uploadResults == null) return Result<Photo>.Failure("Failed to upload Photo", 400);

                var user = await userAccessor.GetUserAsync();

                var photo = new Photo
                {
                    Url = uploadResults.Url,
                    PublicId = uploadResults.PublicId,
                    UserId = user.Id
                };

                // What this means [??=] is if [user.ImageUrl] is [null] will [assign] what ever is to the [right] of [??=].
                user.ImageUrl ??= photo.Url;

                context.Photos.Add(photo);

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                return result
                    ? Result<Photo>.Success(photo)
                    : Result<Photo>.Failure("Problem saving photo to DB", 400);
            }
        }
    }
}