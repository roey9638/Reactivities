using System;

namespace Domain;

public class ActivityAttendee
{
    public string? UserId { get; set; } // The ID of the user attending the activity.
    public User User { get; set; } = null!; // The actual User object (more info about the user).
    public string? ActivityId { get; set; } // The ID of the activity the user is attending.
    public Activity Activity { get; set; } = null!;  // The actual Activity object (more info about the activity).
    public bool IsHost { get; set; } // True if the user is the host (organizer) of the activity.
    public DateTime DateJoined { get; set; } = DateTime.UtcNow; // The date/time the user joined the activity (defaults to now).
}
