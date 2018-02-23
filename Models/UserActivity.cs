using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace beltexam.Models
{
    public class UserActivity : BaseEntity
    {
        public int UserActivityId { get ; set ; }
        [ForeignKey("User")]
        public int UserId { get ; set ; }
        public User User { get ; set ; }
        [ForeignKey("Activity")]
        public int ActivityId { get ; set ; }
        public Activity Activity { get ; set ; }
    }
}