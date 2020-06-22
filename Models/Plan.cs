using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CsharpBelt.Models
{
    public class Plan
    {
        [Key]
        public int PlanId {get;set;}
        [Required]
        public string Title {get;set;}
        [Required]
        public string Description {get;set;}
        [Required]
        [Val]
        public DateTime Date {get;set;}
        [Required]
        public string Duration {get;set;}
        public int UserId {get;set;}
        public User Planner {get;set;}
        public List<Invite> Guests {get;set;}
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpatedAt {get; set;} = DateTime.Now;
    }
}