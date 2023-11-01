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
        public async Task<IActionResult> CreateGroup([FromBody]GroupChat request){
            var c = new GroupChat{
                GroupId = ChatIdGenerator(),
                GroupName = request.GroupName,
                CreatorId = request.CreatorId,
                CreatorName = request.CreatorName,
                DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy")

            };
            
            var s = new GroupParticipant{
            GroupId = c.GroupId,
            GroupName = c.GroupName,
            UserId = c.CreatorId,
            UserName = c.CreatorName,
            DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
            Role = constant.GrpAdmin,
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
            var grp = context.GroupParticipants.Where(a=>a.UserId==ID).OrderByDescending(r=>r.Id).ToList();
            return Ok(grp);
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