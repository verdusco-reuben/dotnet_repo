using System;
using System.ComponentModel.DataAnnotations;


namespace beltexam.Models
{
    public class ActivityViewModel : BaseEntity
    {
        [Required]
        [MinLength(2)]
        public string Title { get ; set ; }
        public string Time { get ; set ; }
        [Required]
        [FutureDate]
        public DateTime Date { get ; set ; }
        [Required]
        public int DurationValue { get ; set ; }
        [Required]
        public string Duration { get ; set ; }
        [Required]
        public string Description { get ; set ;}
    }
}