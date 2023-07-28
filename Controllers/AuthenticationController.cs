using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HDSS_BACKEND.Data;
using HDSS_BACKEND.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HDSS_BACKEND.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly DataContext context;
        Constants constant = new Constants();
        public AuthenticationController(DataContext ctx){
            context = ctx;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]AuthenticationModel request){
        
        var UserId = context.AuthenticationModels.FirstOrDefault(a=>a.UserId == request.UserId);
        if (UserId == null){
        return BadRequest("Login Failed ");
        }
        
    if (!BCrypt.Net.BCrypt.Verify(request.UserPassword, UserId.UserPassword))
    {
        return BadRequest("Incorrect password");
    } 

    
    if(UserId.Role == constant.Teacher){
        var Teacher = context.Teachers.FirstOrDefault(a=>a.StaffID == UserId.UserId);
        return Ok(Teacher);
    }
    else if(UserId.Role == constant.Student){
        var Student = context.Students.FirstOrDefault(a=>a.StudentId == UserId.UserId);
        return Ok(Student);
    }
    else if(UserId.SpecificUserRole == constant.Director){
        var Director = context.SchoolDirectors.FirstOrDefault(a=>a.DirectorID == UserId.UserId);
        return Ok(Director); 
    }
   // bool check = await context.SuperiorAccounts.AnyAsync(a=>a.SpecificRole == constant.SuperiorUser);
    else if (UserId.SpecificUserRole== constant.SuperiorUser && UserId.Role == constant.Admin){
        var Superior =  context.SuperiorAccounts.FirstOrDefault(a=>a.Email == UserId.UserId);
        return Ok(Superior);
    }
    else if(UserId.Role == constant.Admin){
        var Manager = context.Managers.FirstOrDefault(a=>a.ManagerID == UserId.UserId);
        return Ok(Manager);
    }
   

    

    return Ok();

        }

    [HttpGet("Teacher")]
    public async Task<IActionResult>Teacher(){
        return Ok(constant.Teacher);
    }
    [HttpGet("Student")]
    public async Task<IActionResult>Student(){
        return Ok(constant.Student);
    }
     [HttpGet("Admin")]
    public async Task<IActionResult>Admin(){
        return Ok(constant.Admin);
    }






    }
}