using System;

namespace Application.Profiles.DTOs;

// This [Class] is to [track] the [Result] we getting from [Cloudinary]
public class PhotoUploadResult
{
    public required string PublicId { get; set; }
    public required string Url { get; set; }
}
