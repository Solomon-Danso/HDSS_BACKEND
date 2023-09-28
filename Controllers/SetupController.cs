using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HDSS_BACKEND.Data;
using HDSS_BACKEND.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HDSS_BACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SetupController : ControllerBase
    {
        private readonly DataContext context;
        public SetupController(DataContext ctx){
            context = ctx;
        }


    [HttpPost("registerInstitution")]
        public async Task<IActionResult> Institution([FromForm]InstituitionDto request){
    


    if (request.File == null || request.File.Length == 0)
    {
        return BadRequest("Invalid file");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Institution", "Profile");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original file extension
    var fileExtension = Path.GetExtension(request.File.FileName);

    // Generate a unique file name
    var fileName = Guid.NewGuid().ToString() + fileExtension;

    // Save the uploaded file to the uploads directory
    var filePath = Path.Combine(uploadsDirectory, fileName);
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await request.File.CopyToAsync(stream);
    }

 var insti = new Instituition
    {
        Logo = Path.Combine("Institution/Profile", fileName),
        SchoolName = request.SchoolName,
        Location = request.Location,
        AdminName = request.AdminName,
        Position = request.Position
        
    };

    var counter = context.Instituitions.Count();
    if(counter>0){
        var ist = context.Instituitions.FirstOrDefault(r=>r.Id>0);
        if (ist == null){
            return BadRequest("Data not found");
        }
        ist.Logo = insti.Logo;
        ist.SchoolName = insti.SchoolName;
        ist.Location = insti.Location;
        ist.AdminName = insti.AdminName;
        ist.Position = insti.Position;
    }
    else{
        context.Instituitions.Add(insti);
    }

    
    await context.SaveChangesAsync();
    return Ok(insti);
    
    }

[HttpPost("AssignRoles")]
public async Task<IActionResult>AssignRoles([FromBody]Role request){

var role = new Role{
FullName = request.FullName,
StaffId = request.StaffId,
Position = request.Position
};
bool checker = await context.Roles.AnyAsync(a=>a.FullName==role.FullName&&a.StaffId==role.StaffId&&a.Position==role.Position);
if (checker){
    return BadRequest("User With That Role already exists");
}
else{
context.Roles.Add(role);
await context.SaveChangesAsync();
}


return Ok(role);


}

[HttpGet("UserRoles")]
public async Task<IActionResult>UserRoles(){
    var userRole = context.Roles.OrderBy(x=>x.Position).ToList();
    return Ok(userRole);
}

[HttpGet("SpecificUserRole")]
public async Task<IActionResult>SpecificUser(string UserId){
    var userRole = context.Roles.Where(x=>x.StaffId==UserId).ToList();
    return Ok(userRole);
}

[HttpDelete("DeleteSpecificUserRole")]
public async Task<IActionResult>DeleteSpecificUser(string UserId, string Position){
    var userRole = context.Roles.FirstOrDefault(x=>x.StaffId==UserId&&x.Position==Position);
    if (userRole == null){
        return BadRequest("User Role not found");
    }
    context.Roles.Remove(userRole);
    await context.SaveChangesAsync();
    return Ok("Deleted Successfully");
}







    }
}