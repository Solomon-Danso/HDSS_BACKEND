using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HDSS_BACKEND.Data;
using HDSS_BACKEND.HyChat.Models;
using HDSS_BACKEND.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HDSS_BACKEND.HyChat.Controllers
{
    [ApiController]
    [Route("api/HyChat")]
    public class HyChatControllers : ControllerBase
    {
        private readonly DataContext context;
        Constants constant = new Constants();
        public HyChatControllers(DataContext ctx)
        {
            context = ctx;
        }

                [HttpPost("CreateGroup")]
        public async Task<IActionResult> CreateGroup([FromForm]GroupChatDto request, string ID){
       
         
         if (request.Picture == null || request.Picture.Length == 0)
    {
        return BadRequest("Invalid slide");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LMS", "Groups");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original slide extension
    var slideExtension = Path.GetExtension(request.Picture.FileName);

    // Generate a unique slide name
    var slideName = Guid.NewGuid().ToString() + slideExtension;

    // Save the uploaded slide to the uploads directory
    var slidePath = Path.Combine(uploadsDirectory, slideName);
    using (var stream = new FileStream(slidePath, FileMode.Create))
    {
        await request.Picture.CopyToAsync(stream);
    }
   
 var student = context.Students.FirstOrDefault(a=>a.StudentId==ID);
           if (student == null){
            return BadRequest("Student Not Found");
           }
           
            var c = new GroupChat{
                GroupId = ChatIdGenerator(),
                GroupName = request.GroupName,
                CreatorId = student.StudentId,
                CreatorName = student.FirstName+" "+student.OtherName+" "+student.LastName,
                DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
                Picture = Path.Combine("LMS/Groups", slideName),

            };
            
            var s = new GroupParticipant{
            GroupId = c.GroupId,
            GroupName = c.GroupName,
            UserId = c.CreatorId,
            UserName = c.CreatorName,
            DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
            Role = constant.GrpAdmin,
            Picture = c.Picture
           };


            context.GroupParticipants.Add(s);
            context.Groups.Add(c);
            await context.SaveChangesAsync();
            return Ok("Group created successfully");


   
    
    }


        [HttpPost("AddGroupMember")]
        public async Task<IActionResult> AddMember([FromBody]GroupParticipant request, string GID){

           var g = context.Groups.FirstOrDefault(a=>a.GroupId==GID);
           if (g == null){
            return BadRequest("No Group Found");
           }

           var s = new GroupParticipant{
            GroupId = g.GroupId,
            GroupName = g.GroupName,
            UserId = request.UserId,
            UserName = request.UserName,
            DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
            Role = constant.GrpMember,
            Picture = g.Picture
           };

           bool checker = await context.GroupParticipants.AnyAsync(a=>a.GroupId==s.GroupId&&a.UserId==s.UserId);
           if(checker){
            return BadRequest($"{s.UserName} is already a member of {s.GroupName}");
           }
           else{
            context.GroupParticipants.Add(s);
            await context.SaveChangesAsync();
           }
           return Ok("Member Added successfully");

        }

        [HttpGet("MyGroups")]
        public async Task<IActionResult> GetMyGroup(string ID){

            var grp = context.GroupParticipants.Where(a=>a.UserId==ID).ToList();
            
            foreach(var a in grp){
                var b = context.GroupMessages.OrderBy(t=>t.Id).LastOrDefault(r=>r.GroupId==a.GroupId);
                
                a.LastMessage = b?.Message;
                a.LastSenderPicture = b?.Picture;
                a.LastSenderName = b?.UserName;
                a.LastSenderDate = b?.DateAdded;
                a.LastSenderId = b?.UserId;
                var c = context.GroupMessages.Where(q=>q.GroupId==a.GroupId&&q.UserId==ID&&q.Status==constant.UnRead).Count();
                a.TotalUnreadMessage = c;
                await context.SaveChangesAsync();

            }
            var final = context.GroupParticipants.Where(a=>a.UserId==ID).OrderByDescending(r=>r.Id).ToList();
            

            return Ok(final);
        }

         [HttpGet("GroupMembers")]
        public async Task<IActionResult> GroupMembers(string ID){
            var grp = context.GroupParticipants.Where(a=>a.GroupId==ID).OrderBy(r=>r.UserName).ToList();
            
            
            return Ok(grp);
        }

        [HttpGet("GroupInfo")]
        public async Task<IActionResult>GetGroupInfo(string ID){
            var grp = context.Groups.FirstOrDefault(a=>a.GroupId==ID);
            if (grp == null){
                return BadRequest("Group not found");
            }
            return Ok(grp);
        }

        [HttpPost("Message")]
        public async Task<IActionResult>Message([FromBody]GroupMessage request,string ID, string GID){
        
        var c = context.Students.FirstOrDefault(a=>a.StudentId==ID);
        if (c == null){
            return BadRequest("Student not found");
        }
        var g = context.Groups.FirstOrDefault(a=>a.GroupId==GID);
        if (g == null){
            return BadRequest("Group not found");
        }
DateTime now = DateTime.Now;
        var s = new GroupMessage{
        GroupId = g.GroupId,
        GroupName = g.GroupName,
        UserId = c.StudentId,
        UserName = c.FirstName+" "+c.OtherName+" "+c.LastName,
        DateAdded = now.ToString("hh:mm tt"),
        Message = request.Message,
        Status = constant.UnRead,
        Picture = c.ProfilePic

        };

        context.GroupMessages.Add(s);
        await context.SaveChangesAsync();
        return Ok("Message sent successfully");


        }


[HttpGet("UnReadCounter")]
public async Task<IActionResult>UnReadCounter(string ID, string GID){
var c = context.GroupMessages.Where(a=>a.GroupId==GID&&a.UserId==ID&&a.Status==constant.UnRead).Count();
return Ok(c);
}

[HttpGet("ReadMessage")]
public async Task<IActionResult>ReadMessage(string ID, string GID){
var c = context.GroupMessages.Where(a=>a.GroupId==GID&&a.UserId==ID&&a.Status==constant.UnRead).ToList();
foreach(var a in c){
a.Status = constant.Read;
}
await context.SaveChangesAsync();
return Ok(c);
}





        private string ChatIdGenerator()
{
    byte[] randomBytes = new byte[2]; // Increase the array length to 2 for a 4-digit random number
    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
    {
        rng.GetBytes(randomBytes);
    }

    ushort randomNumber = BitConverter.ToUInt16(randomBytes, 0);
    int fullNumber = randomNumber; // 109000 is added to ensure the number is 5 digits long

    return fullNumber.ToString("D5");
}



    }
}