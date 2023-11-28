using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Transactions;
using HDSS_BACKEND.Data;
using HDSS_BACKEND.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HDSS_BACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Accounting : ControllerBase
    {
        private readonly DataContext context;
          string Country;
        string City;
        double latitude;
        double logitude;
        Constants constant = new Constants();
        public Accounting(DataContext ctx){
            context = ctx;
        }

        [HttpPost("AddFees")]
        public async  Task<IActionResult> AddFees([FromBody] Fee request){
            var fee = new Fee{
                Title = request.Title,
                Amount = request.Amount,
                AcademicYear = request.AcademicYear,
                AcademicTerm = request.AcademicTerm,
                Level = request.Level,

            };
            context.Fees.Add(fee);
            await context.SaveChangesAsync();

            return Ok("Fees added successfully");
        }

         [HttpPost("UpdatedFees")]
        public async  Task<IActionResult> UpdatedFees([FromBody] Fee request, int Id){
            var fee = context.Fees.FirstOrDefault(r => r.Id == Id);
            if(fee == null){
                return BadRequest("Fee not found");
            }

                fee.Title = request.Title;
                fee.Amount = request.Amount;
                fee.AcademicYear = request.AcademicYear;
                fee.AcademicTerm = request.AcademicTerm;
                fee.Level = request.Level;

            
            
            await context.SaveChangesAsync();

            return Ok("Fees Updated successfully");
        }

        [HttpGet("viewFees")]
        public async Task<IActionResult> GetFees(string Level, string AcaYear, string Term){
            var fees = context.Fees.Where(r=>r.Level == Level && r.AcademicYear == AcaYear && r.AcademicTerm==Term).OrderByDescending(r=>r.Id).ToList();
            return Ok(fees);
        }

        [HttpDelete("deleteFees")]
        public async Task<IActionResult> DeleteFees(int Id){
            var fees = context.Fees.FirstOrDefault(r=>r.Id==Id);
            if(fees == null){
                return BadRequest("Fees not found");
            }
            context.Fees.Remove(fees);
            await context.SaveChangesAsync();
            return Ok("Fees deleted");
        }


        [HttpPost("AddAdmissionFees")]
        public async  Task<IActionResult> AddAdmissionFees([FromBody] AdmissionFee request){
            var fee = new AdmissionFee{
               
                Amount = request.Amount,
                AcademicYear = request.AcademicYear,
                Level = request.Level,

            };
            bool checker = await context.AdmissionFees.AnyAsync(r=>r.AcademicYear==fee.AcademicYear && r.Level==fee.Level);
            if(checker){
                return BadRequest("You have already set an admission fee for this level");
            }
            else{
                context.AdmissionFees.Add(fee);
            await context.SaveChangesAsync();
            }
            

            return Ok("AdmissionFees added successfully");
        }

         [HttpPost("UpdatedAdmissionFees")]
        public async  Task<IActionResult> UpdatedAdmissionFees([FromBody] AdmissionFee request, int Id){
            var fee = context.AdmissionFees.FirstOrDefault(r => r.Id == Id);
            if(fee == null){
                return BadRequest("AdmissionFee not found");
            }

               
                fee.Amount = request.Amount;
                fee.AcademicYear = request.AcademicYear;
               
                fee.Level = request.Level;

            
            
            await context.SaveChangesAsync();

            return Ok("AdmissionFees Updated successfully");
        }

        [HttpGet("viewAdmissionFees")]
        public async Task<IActionResult> GetAdmissionFees(string Level, string AcaYear, string Term){
            var fees = context.AdmissionFees.Where(r=>r.Level == Level && r.AcademicYear == AcaYear).OrderByDescending(r=>r.Id).ToList();
            return Ok(fees);
        }

        [HttpDelete("deleteAdmissionFees")]
        public async Task<IActionResult> DeleteAdmissionFees(int Id){
            var fees = context.AdmissionFees.FirstOrDefault(r=>r.Id==Id);
            if(fees == null){
                return BadRequest("AdmissionFees not found");
            }
            context.AdmissionFees.Remove(fees);
            await context.SaveChangesAsync();
            return Ok("AdmissionFees deleted");
        }

        [HttpPost("PayFees")]
        public async Task<IActionResult>FeesPayment(string StudentId, [FromBody]Payment request, string StaffId){
            var stu = context.Students.FirstOrDefault(r=>r.StudentId==StudentId);
            var Staff = context.Admins.FirstOrDefault(r=>r.AdminID==StaffId&&r.SpecificRole==constant.Accountant||r.SpecificRole==constant.SuperiorUser);
             if(Staff==null){
                return BadRequest("You dont have the permission to perform this activity");
            }
            
            if (stu == null){
                return BadRequest("Student not found");
            }
           

            var Bill = new BillingCard{
                StudentId = stu.StudentId,
                OpeningBalance = stu.Balance,
                AcademicYear = stu.TheAcademicYear,
                AcademicTerm = stu.TheAcademicTerm,
                Level = stu.Level,
                TransactionDate = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
                Action = request.Action,
                TransactionId = TransactionIdGenerator(),
                StaffId = Staff.AdminID,
                StaffName = Staff.Name,
                PaymentMethod = request.PaymentMethod

            };
            
            Bill.Transaction = request.Amount;
            Bill.ClosingBalance = Bill.OpeningBalance-Bill.Transaction;
            stu.Balance = Bill.ClosingBalance;
            if(Bill.PaymentMethod.IsNullOrEmpty()|| Bill.Action.IsNullOrEmpty()){
                return BadRequest("All Fields are required");
            }
            else{
                 context.BillingCards.Add(Bill);
            await context.SaveChangesAsync();
            }

           
            return Ok(Bill);

        }

        [HttpGet("PaymentHis")]
        public async Task<IActionResult> GetPaymentHis(string StudentId){
            var stu = context.BillingCards.Where(a=>a.StudentId==StudentId).ToList();
            return Ok(stu);
        }

        [HttpPost("BillAClass")]
        public async Task<IActionResult> Biller(string Level, string Term, string Year){
        var fees = context.Fees.Where(r=>r.Level==Level && r.AcademicTerm ==Term && r.AcademicYear==Year).Sum(r=>r.Amount);
        var studentList = context.Students.Where(r => r.Level==Level).ToList();
        foreach (var student in studentList){
            var Bill = new BillingCard{
                StudentId = student.StudentId,
                OpeningBalance = student.Balance,
                AcademicYear = Year,
                AcademicTerm = Term,
                Level = Level,
                TransactionDate = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
                Action = constant.TermFees,
                Bills = fees
            };
            Bill.Transaction = 0;
            var newBalance = student.Balance+fees;
            Bill.ClosingBalance = newBalance;
            context.BillingCards.Add(Bill);

            
            student.Balance = newBalance;
             student.Level = Level;
            student.TheAcademicTerm = Term;
            student.TheAcademicYear = Year;

        


            await context.SaveChangesAsync();
        }

            return Ok("Class Billed Successfully");
        }

        [HttpPost("RectifyBillAClass")]
        public async Task<IActionResult> RectifyBiller(string Level, string Term, string Year){
        var fees = context.Fees.Where(r=>r.Level==Level && r.AcademicTerm ==Term && r.AcademicYear==Year).Sum(r=>r.Amount);
        var studentList = context.Students.Where(r => r.Level==Level).ToList();
        foreach (var student in studentList){
            var newBalance = student.Balance-fees;

              var Bill = new BillingCard{
                StudentId = student.StudentId,
                OpeningBalance = student.Balance,
                AcademicYear = student.TheAcademicYear,
                AcademicTerm = student.TheAcademicTerm,
                Level = student.Level,
                TransactionDate = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
                Action = constant.TermFeesUnbilling,
                Bills=fees
            };
            Bill.Transaction = 0;
            Bill.ClosingBalance = newBalance;
            context.BillingCards.Add(Bill);

            student.Balance = newBalance;
           
            await context.SaveChangesAsync();
        }

            return Ok("Class UnBilled Successfully");
        }

       [HttpPost("DiscountedFees")]
public async Task<IActionResult> DiscountedFees(int discountRate, string StudentId)
{
    var student = context.Students.FirstOrDefault(r => r.StudentId == StudentId);
    if (student == null)
    {
        return BadRequest("Student not found");
    }

    if (student.Balance.HasValue) // Check if student.Balance is not null
    {
        double rate = (double)discountRate / 100;
        double discount = (double)(rate * student.Balance.Value); 
        double newBalance = student.Balance.Value - discount;
        student.Balance = newBalance;

        
        

        await context.SaveChangesAsync();

        return Ok(discount); // Return the result, or adjust this as needed
    }
    else
    {
        // Handle the case where student.Balance is null (if needed)
        return BadRequest("Student balance is null.");
    }
}


private string TransactionIdGenerator()
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

[ApiExplorerSettings(IgnoreApi = true)] 
public async Task StudentAuditor(string StudentId,string Action)
{


    var user = context.Students.FirstOrDefault(a => a.StudentId==StudentId);
    if (user ==null){
         BadRequest("Student not found");
    }

    var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
    var deviceCategory = GetDeviceCategory(userAgent);
     //var ipAddress = "41.155.45.174";
    if (string.IsNullOrEmpty(ipAddress))
    {
        ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

    }

        try
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetFromJsonAsync<IpApiLocationResponse>($"http://ip-api.com/json/{ipAddress}");

            Country = response.country;
            City = response.city;
            latitude = response.lat;
            logitude = response.lon;
        }
    }
    catch (HttpRequestException ex)
    {
        // Handle the exception or log it as needed
        // You can set default values for Country, City, latitude, and longitude here
        // For example:
        Country = "Unknown";
        City = "Unknown";
        latitude = 0.0;
        logitude = 0.0;
    }

//fast
 string formattedTime = DateTime.Now.ToString(@"hh:mm:ss tt", CultureInfo.InvariantCulture);


    var audit = new AuditTrial
    {
        IpAddress = ipAddress,
        BrowserType = userAgent,
        DeviceType = deviceCategory,
        UserLocation = $"https://www.google.com/maps?q={latitude},{logitude}",
        Country = Country,
        City = City,
        Maker = user.FirstName+" "+user.OtherName+" "+user.LastName,
        TheDateStamp = DateTime.Today.Date.ToString("MMMM dd, yyyy"),
       TheTimeStamp = formattedTime,
        Email= user.Email,
        ActionDescription = Action,
        Role = user.Role,
        Level = user.Level,
        ProfilePic = user.ProfilePic

    };

    context.AuditTrials.Add(audit);
    await context.SaveChangesAsync();

   
}

[ApiExplorerSettings(IgnoreApi = true)] 
public async Task AdminAuditor(string StaffId, string Action)
{
    var user = context.Admins.FirstOrDefault(a => a.AdminID == StaffId);
    if (user == null)
    {
          BadRequest("User not found");
    }

    var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
    var deviceCategory = GetDeviceCategory(userAgent);

    // Ensure ipAddress is not null or empty
    if (string.IsNullOrEmpty(ipAddress))
    {
        ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
    }

    try
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetFromJsonAsync<IpApiLocationResponse>($"http://ip-api.com/json/{ipAddress}");

            if (response != null)
            {
                Country = response.country;
                City = response.city;
                latitude = response.lat;
                logitude = response.lon;
            }
            else
            {
                // Log a message when the response is null
                // You can replace this with your preferred logging mechanism
                Console.WriteLine("IP API response is null");
                Country = "Unknown";
                City = "Unknown";
                latitude = 0.0;
                logitude = 0.0;
            }
        }
    }
    catch (HttpRequestException ex)
    {
        // Log the exception
        // You can replace this with your preferred logging mechanism
        Console.WriteLine($"HttpRequestException: {ex.Message}");

        // Set default values for Country, City, latitude, and longitude
        Country = "Unknown";
        City = "Unknown";
        latitude = 0.0;
        logitude = 0.0;
    }

    // Fast
    string formattedTime = DateTime.Now.ToString(@"hh:mm:ss tt", CultureInfo.InvariantCulture);

    var audit = new AuditTrial
    {
        IpAddress = ipAddress,
        BrowserType = userAgent,
        DeviceType = deviceCategory,
        UserLocation = $"https://www.google.com/maps?q={latitude},{logitude}",
        Country = Country,
        City = City,
        Maker = user.Name,
        TheDateStamp = DateTime.Today.Date.ToString("MMMM dd, yyyy"),
        TheTimeStamp = formattedTime,
        Email = user.Email,
        ActionDescription = Action,
        Role = user.Role,
        ProfilePic = user.ProfilePic
    };

    context.AuditTrials.Add(audit);
    await context.SaveChangesAsync();

      Ok();
}

[ApiExplorerSettings(IgnoreApi = true)] 
public async Task TeacherAuditor(string StudentId,string Action)
{


    var user = context.Teachers.FirstOrDefault(a => a.StaffID==StudentId);
    if (user ==null){
         BadRequest("Teacher not found");
    }

    var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
    var deviceCategory = GetDeviceCategory(userAgent);
     //var ipAddress = "41.155.45.174";
    if (string.IsNullOrEmpty(ipAddress))
    {
        ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

    }

        try
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetFromJsonAsync<IpApiLocationResponse>($"http://ip-api.com/json/{ipAddress}");

            Country = response.country;
            City = response.city;
            latitude = response.lat;
            logitude = response.lon;
        }
    }
    catch (HttpRequestException ex)
    {
        // Handle the exception or log it as needed
        // You can set default values for Country, City, latitude, and longitude here
        // For example:
        Country = "Unknown";
        City = "Unknown";
        latitude = 0.0;
        logitude = 0.0;
    }

//fast
 string formattedTime = DateTime.Now.ToString(@"hh:mm:ss tt", CultureInfo.InvariantCulture);


    var audit = new AuditTrial
    {
        IpAddress = ipAddress,
        BrowserType = userAgent,
        DeviceType = deviceCategory,
        UserLocation = $"https://www.google.com/maps?q={latitude},{logitude}",
        Country = Country,
        City = City,
        Maker = user.FirstName+" "+user.OtherName+" "+user.LastName,
        TheDateStamp = DateTime.Today.Date.ToString("MMMM dd, yyyy"),
       TheTimeStamp = formattedTime,
        Email= user.Email,
        ActionDescription = Action,
        Role = user.Role,
       
        ProfilePic= user.FilePath

    };

    context.AuditTrials.Add(audit);
    await context.SaveChangesAsync();

   
}

[ApiExplorerSettings(IgnoreApi = true)] 
public async Task AuthAuditor(string StaffId,string Action)
{


    var user = context.AuthenticationModels.FirstOrDefault(a => a.UserId==StaffId);
    if (user ==null){
         BadRequest("Student not found");
    }

    var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
    var deviceCategory = GetDeviceCategory(userAgent);
     //var ipAddress = "41.155.45.174";
    if (string.IsNullOrEmpty(ipAddress))
    {
        ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

    }

        try
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetFromJsonAsync<IpApiLocationResponse>($"http://ip-api.com/json/{ipAddress}");

            Country = response.country;
            City = response.city;
            latitude = response.lat;
            logitude = response.lon;
        }
    }
    catch (HttpRequestException ex)
    {
        // Handle the exception or log it as needed
        // You can set default values for Country, City, latitude, and longitude here
        // For example:
        Country = "Unknown";
        City = "Unknown";
        latitude = 0.0;
        logitude = 0.0;
    }

//fast
 string formattedTime = DateTime.Now.ToString(@"hh:mm:ss tt", CultureInfo.InvariantCulture);


    var audit = new AuditTrial
    {
        IpAddress = ipAddress,
        BrowserType = userAgent,
        DeviceType = deviceCategory,
        UserLocation = $"https://www.google.com/maps?q={latitude},{logitude}",
        Country = Country,
        City = City,
        Maker = user.Name,
        TheDateStamp = DateTime.Today.Date.ToString("MMMM dd, yyyy"),
       TheTimeStamp = formattedTime,
        ActionDescription = Action,
        Role = user.SpecificUserRole,
        

    };

    context.AuditTrials.Add(audit);
     try
    {
        // Existing code for AuthAuditor
        // ...
        await context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        // Log any exceptions here
        Console.WriteLine(ex.Message);
    }

   
}



private string GetDeviceCategory(string userAgent)
{
    userAgent = userAgent.ToLower();

    if (userAgent.Contains("mobile") || userAgent.Contains("android") || userAgent.Contains("iphone") || userAgent.Contains("ipad") || userAgent.Contains("ipod"))
    {
        return "Mobile";
    }
    else if (userAgent.Contains("tablet"))
    {
        return "Tablet";
    }
    else
    {
        return "Desktop";
    }
}




    }
}