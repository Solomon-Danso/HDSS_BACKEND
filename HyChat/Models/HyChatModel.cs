using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HDSS_BACKEND.HyChat.Models
{
    public class GroupChat
    {
        public int Id {get;set;}
        public string? GroupId {get;set;}
        public string? GroupName { get; set; }
        public string? CreatorId {get; set; }
        public string? CreatorName { get; set; }
          public string? DateAdded {get;set;}
       

    }

     public class SingleChat
    {
        public int Id {get;set;}
        public string? SingleId {get;set;}
        public string? SingleName { get; set; }

    }

    public class GroupParticipant{
        public int Id {get;set;}
        public string? GroupId {get;set;}
        public string? GroupName {get;set;}
        public string? UserId {get;set;}
        public string? UserName {get;set;}
        public string? DateAdded {get;set;}
        public string? Role{get; set;}

    }




}