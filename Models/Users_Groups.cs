using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Dw23787.Models
{

    [PrimaryKey(nameof(UserFK), nameof(GroupFK))]
    public class Users_Groups
    {

        public Users_Groups()
        {
            UserAdmin = true;
        }

        // Pode ser redundante...
        public Boolean UserAdmin { get; set; }

        [ForeignKey(nameof(UserId))]
        [Display(Name = "User")]
        public int UserFK { get; set; }

        public Users UserId { get; set; }

        [ForeignKey(nameof(GroupId))]
        [Display(Name = "Group")]
        public string GroupFK { get; set; }

        public Groups GroupId { get; set; }

    }
}
