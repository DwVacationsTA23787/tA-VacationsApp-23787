using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dw23787.Models
{
    public class Trips
    {
        [Key]
        public string Id { get; set; }

        public string TripName { get; set; }

        public string Description { get; set; }

        public TripCategory Category { get; set; }

        public TripTransport Transport { get; set; }

        public string InicialBudget { get; set; }

        public string FinalBudget { get; set; }

        public string? Banner { get; set; }

        public bool Closed { get; set; }

        // Navigation property to Groups (one-to-one relationship)
        public Groups Group { get; set; }

        // Foreign key for Groups (optional, depending on your approach)
        [ForeignKey(nameof(Group))]
        public string GroupId { get; set; }

        // Foreign key for User
        [ForeignKey(nameof(User))]
        public int UserFK { get; set; }
        public Users User { get; set; }

        public enum TripCategory
        {
            Adventure,
            Leisure,
            Cultural,
            Business,
            Family
        }

        public enum TripTransport
        {
            Plane,
            Bus,
            Train,
            Hitchhiking
        }
    }
}
