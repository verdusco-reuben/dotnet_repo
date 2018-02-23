using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace beltexam.Models
{
    public class User : BaseEntity
    {
        [Key]        
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public List<Activity> ActivitiesICreated { get; set; }
        [InverseProperty("User")]
        public List<UserActivity> ActivitiesImJoining { get ; set ; }
        
        // UpdatedAt & CreatedAt inside BaseEntity
        
        public User()
        {
            ActivitiesICreated = new List<Activity>();
            ActivitiesImJoining = new List<UserActivity>();
        }
    }
}