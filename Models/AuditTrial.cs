using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HDSS_BACKEND.Models
{
   public class AuditTrial{
      public int Id{get;set;}
      public string? IpAddress {get; set;}
       public string? BrowserType {get; set;}
       public string? DeviceType {get; set;}
       public string? UserLocation {get; set;}
       public string? Country {get; set;}
       public string? City{get;set;}
       public string? ActionDescription {get; set;}
       public string? Maker {get; set;}
       public string? Role {get;set;}
       public string? Level{get; set;}
       public string? Email {get; set;}
       public string? TheDateStamp {get;set;}
       public  string?  TheTimeStamp { get; set; }
       public string? UserId {get;set;}
       public string? ProfilePic {get;set;}
       
    }


public class IpApiLocationResponse
{
    public double lat { get; set; } // Latitude
    public double lon { get; set; } // Longitude
    public string? country {get;set;}
    public string? city {get;set;}

}

}