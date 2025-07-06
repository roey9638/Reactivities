using System;

namespace Domain;

public class UserFollowing
{
    public required string ObserverId { get; set; }
    public User Observer { get; set; } = null!; // This will be the [Following] [User]. So [Follower]
    public required string TargetId { get; set; } 
    public User Target { get; set; } = null!; // This will be the [User] that's been [Followed]. So [Following] 
}
