using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    // We using [query] in the [GetActivityList] [Class]. [Date] is a [type of] [DateTime]
    // So this [Index(nameof(Date))] is for when we [Pass] a [DateTime] iw will Jump [straight] to the [DateTime] we [gave it].
    [Index(nameof(Date))]
    public class Activity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Title { get; set; }
        public DateTime Date { get; set; }
        public required string Description { get; set; }
        public required string Category { get; set; }
        public bool IsCancelled { get; set; }

        // location props
        public required string City { get; set; }
        public required string Venue { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // navigation properties
        public ICollection<ActivityAttendee> Attendees { get; set; } = [];
        public ICollection<Comment> Comments { get; set; } = [];
    }
}