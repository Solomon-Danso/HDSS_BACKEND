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
          public string? Picture {get; set; }
       
       

    }

    public class GroupChatDto
    {
        public int Id {get;set;}
        public string? GroupId {get;set;}
        public string? GroupName { get; set; }
        public string? CreatorId {get; set; }
        public string? CreatorName { get; set; }
          public string? DateAdded {get;set;}
          public IFormFile? Picture {get; set; }
       

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
        public string? Picture {get; set;}
          public string? LastMessage {get; set; }
          public string? LastSenderPicture {get; set; }
          public string? LastSenderName {get; set; }
          public string? LastSenderDate {get; set; }
          public string? LastSenderId {get;set;}
          public int TotalUnreadMessage {get; set;}
        public DateTime DandT {get; set;}
          public string? InGroup {get; set;}
         


    }

    public class GroupMessage{
         public int Id {get;set;}
        public string? GroupId {get;set;}
        public string? GroupName {get;set;}
        public string? UserId {get;set;}
        public string? UserName {get;set;}
        public string? DateAdded {get;set;}
        public string? Message {get; set;}
        public string? Picture {get; set;}
        public DateTime? DandT {get; set;}
          public string? Status {get; set;}
          public string? MessageType {get; set;}
    }

    public class UserPersonalMessage{
         public int Id {get;set;}
        public string? GroupId {get;set;}
        public string? GroupName {get;set;}
        public string? UserId {get;set;}
        public string? UserName {get;set;}
        public string? DateAdded {get;set;}
        public string? Message {get; set;}
        public string? Status {get; set;}
        public string? Picture {get; set;}
        public DateTime? MTime {get; set;}
          public string? MessageType {get; set;}
          public string? Mode {get; set;}
    }

 

public class ImagePreviewer {
    public int Id {get; set;}
    public string? PreviewId{get;set;}
    public string? Picture {get; set;}
}


public class ImagePreviewerDto {
    public int Id {get; set;}
    public string? PreviewId{get;set;}
    public IFormFile? Picture {get; set;}
}







}