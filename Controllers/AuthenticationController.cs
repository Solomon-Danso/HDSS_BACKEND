using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HDSS_BACKEND.Data;
using HDSS_BACKEND.Models;
using Microsoft.AspNetCore.Mvc;
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
    else if(UserId.Role == constant.Director){
        var Director = context.SchoolDirectors.FirstOrDefault(a=>a.DirectorID == UserId.UserId); 
    }
    

    return Ok();
    



        }





    }
}