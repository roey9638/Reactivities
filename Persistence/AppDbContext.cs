using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
    {
        public required DbSet<Activity> Activities { get; set; }
        public required DbSet<ActivityAttendee> ActivityAttendees { get; set; }
        public required DbSet<Photo> Photos { get; set; }


        // This will [Provide] a [Configutation] to [Configure] our [Relationship] of are [Tables]
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // This will make the [Primary Key] for the [ActivityAttendee] [Table]
            builder.Entity<ActivityAttendee>(x => x.HasKey(a => new { a.ActivityId, a.UserId }));


            // configuring the [relationship] [between] [ActivityAttendee] and [User].!!!!
            /* 
                1) // The [ActivityAttendee] will have (1) [User] So [ActivityAttendee] ---> [User] 
                2) // And that [User] will have [Many] [Activities] So [User] ---> [ActivityAttendee]
            */
            builder.Entity<ActivityAttendee>()
            .HasOne(x => x.User) // Each [ActivityAttendee] is related to [one] [User].
            .WithMany(x => x.Activities) // Each User can have many [Activities]
            .HasForeignKey(x => x.UserId); // The link (foreign key) between them is the UserId field in ActivityAttendee


            // configuring the [relationship] [between] [ActivityAttendee] and [Activity].!!!!
            /* 
                1) // The [ActivityAttendee] will have (1) [Activity] So [ActivityAttendee] ---> [Activity] 
                2) // And that [Activity] will have [Many] [Attendees] So [Activity] ---> [ActivityAttendee]
            */
            builder.Entity<ActivityAttendee>()
                .HasOne(x => x.Activity) // Each [ActivityAttendee] is related to one [Activity]
                .WithMany(x => x.Attendees) // Each [Activity] can have [many] [attendees]
                .HasForeignKey(x => x.ActivityId); // The [foreign key] in [ActivityAttendee] that [links] to the [Activity] is [ActivityId].
        }
    }
}