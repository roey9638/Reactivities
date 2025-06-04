using System;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.Profiles.Commands;

public class DeletePhoto
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string PhotoId { get; set; }
    }

    public class Handler(AppDbContext context, IUserAccessor userAccessor, IPhotoService photoService) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userAccessor.GetUserWithPhotosAsync();

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);

            if (photo == null) return Result<Unit>.Failure("Cannot Find Photo", 400);

            if (photo.Url == user.ImageUrl) return Result<Unit>.Failure("Cannot Delete Main Photo", 400);

            /* 
               The reason we [Passing] the [PublicId] of the [Photo] and [Not] his [Id] Because VVV  
               We want to [Delete] the [Photo] from [Cloudinary].
               The [Id] is what's [Stored] in the [DataBase]
            */
            await photoService.DeletePhoto(photo.PublicId);

            user.Photos.Remove(photo);

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            return result
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure("Problem deleting photo", 400);
        }
    }
}