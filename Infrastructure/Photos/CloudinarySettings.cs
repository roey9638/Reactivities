using System;

namespace Infrastructure.Photos;

// All of this [Properties] has to be [Matched] to what we have in the [appsettings.json]
public class CloudinarySettings
{
    public required string CloudName { get; set; }
    public required string ApiKey { get; set; }
    public required string ApiSecret { get; set; }
}
