using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace beltexam.Models
{
    public class Activity : BaseEntity
    {
        public int ActivityId { get ; set ; }
        public string Title { get ; set ; }
        public string Time { get ; set ; }
        public string Date { get ; set ; }
        public string Duration { get ; set ;}
        public string Description { get ; set ;}
        // there will be only one coordinator, so only 1 person can delete
        public int UserId { get ; set ; }
        public User Coordinator { get ; set ; }
        
        // many Users can be guests
        [InverseProperty("Activity")]
        public List<UserActivity> Participants { get; set; }

        public Activity()
        {
            Participants = new List<UserActivity>();
        }
    }
}