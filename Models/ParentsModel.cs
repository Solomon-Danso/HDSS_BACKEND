using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HDSS_BACKEND.Models
{
    public class ParentsModel
    {
        public int Id { get; set; }
        public string? ParentId { get; set; }
        public string? StudentPicture { get; set; }
        public string? StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? StudentLevel { get; set; }
        public string? FathersName { get; set; }
        public string? FatherOccupation { get; set; }
        public string? MothersName { get; set; }
        public string? MotherOccupation { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianOccupation { get; set; }
        public string? ParentLocation {get;set;}
        public string? ParentDigitalAddress {get;set;}
        public string? ParentReligion {get; set;}
        public string? ParentEmail {get; set;}
        public string? EmergencyContactName {get;set;}
        public string? EmergencyPhoneNumber {get; set;}
        public string? EmergencyAlternatePhoneNumber {get; set;}
        public string? RelationshipWithChild {get; set;}
        public int? NumberOfWards {get; set;} 
        public string? Role {get; set;}
        public string? DateAdded {get; set;}


    }
}