using System.ComponentModel.DataAnnotations;

namespace CsharpBelt.Models
{
    public class Invite
    {
        [Key]
        public int InviteId {get;set;}
        public int UserId {get;set;}
        public int PlanId {get;set;}
        public User User {get;set;}
        public Plan Plan {get;set;}  
    }
}