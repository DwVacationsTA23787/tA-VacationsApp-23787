using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dw23787.Models
{
    public class Groups
    {
        public Groups()
        {
            MessagesList = new HashSet<Messages>();
        }

        [Key]
        public string GroupId { get; set; }

        public string Name { get; set; }

        // Navigation property to Trips (one-to-one relationship)
        public Trips Trip { get; set; }

        // Collection navigation property for Messages (if needed)
        public ICollection<Messages> MessagesList { get; set; }
    }
}
