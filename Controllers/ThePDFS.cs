using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HDSS_BACKEND.Data;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;


namespace HDSS_BACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThePDFS : ControllerBase
    {
        private readonly DataContext context;
     
        public ThePDFS(DataContext ctx){
            context = ctx;
        } 

        [HttpGet("generator")]
        public async Task<IActionResult>GenPdfWithImage(string Id){

        var student = context.Students.FirstOrDefault(r=>r.StudentId==Id);
        var inst = context.Instituitions.FirstOrDefault(r=>r.Id>0);
        var auth = context.AuthenticationModels.FirstOrDefault(r=>r.UserId==Id);
        if(student==null || inst==null || auth==null){
            return BadRequest("Student Not Found");
        }
          
       var adfee = context.AdmissionFees.FirstOrDefault(r => r.Level == student.Level && r.AcademicYear == student.TheAcademicYear);
var otherfee = context.Fees.Where(r => r.Level == student.Level && r.AcademicYear == student.TheAcademicYear && r.AcademicTerm == student.TheAcademicTerm).Sum(r => r.Amount);

double? fees;

if (adfee != null)
{
    fees = adfee.Amount + otherfee;
}
else
{
    fees = otherfee;
}



       
    
       
        

            var document = new PdfDocument();

           
           string studentImage = "http://" + HttpContext.Request.Host.Value + "/" + student.ProfilePic;
           string schoolImage = "http://" + HttpContext.Request.Host.Value + "/"+ inst.Logo;
           var schoolname = inst.SchoolName;
           var Location = inst.Location;
           var Registerer = inst.AdminName;
           var fullname =  student.FirstName+" "+ student.OtherName+ " "+student.LastName;


string htmlelement = $@"<!DOCTYPE html>
<html>
<head>
<style>
    body {{
        font-family: 'Open Sans', sans-serif;
        font-size: 14px;
        background-color: #f4f4f4;
        margin: 0;
        padding: 0;
    }}

    .container {{
        max-width: 600px;
        margin: 0 auto;
        background-color: #ffffff;
        padding: 20px;
        border-radius: 10px;
        box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
        animation: fade-in 0.5s ease-out;
        
    }}

    .container img {{
        width: 120px;
        height: auto;
        display: block;
        margin: 0 auto;
    }}

    .header {{
        font-size: 1.6rem;
        font-weight: bold;
        margin-top: 20px;
    }}

    .center{{
        text-align: center;
    }}

   


    .school-name {{
        font-size: 1.2rem;
        font-weight: bold;
        margin-top: 20px;
    }}

    .content {{
        text-align: justify;
    }}

    .contentp {{
        font-size: 18px;
        margin-bottom: 20px;
        line-height: 1.5;
        font-weight: 900;
    }}


    .spacer{{
        margin-left: 200px;
    }}
    .bold{{
        
        font-size: 18px;
    }}
    .lightbold{{
        
        font-size: 15px;
    }}
    .school{{
        
        font-size: 30px;
    }}

    /* Add individual styles for .left, .center, and .right */
.left {{
    float: left; /* Float the left div to the left */
    width: 30%; /* Adjust the width as needed */
    padding: 10px; /* Add padding for spacing */
    box-sizing: border-box; /* Include padding and border in the width */
    text-align: left; /* Align text to the left within the left div */
}}



.right {{
    float: right; /* Float the right div to the left */
    padding: 10px; /* Add padding for spacing */
    box-sizing: border-box; /* Include padding and border in the width */
    text-align: right; /* Align text to the right within the right div */
    margin-left:120px;
    margin-top: -2300rem;

}}




</style>
</head>
<body>
    <div class='container'>
    <div class='center'> 
       <img src='{schoolImage}' alt='School Logo' />
</div>
<br/>
        <div >
           <span class='bold'>Dear {fullname},</span>
            <span class='spacer lightbold'>{student.AdmissionDate}</span>
        </div>

<br/>
<div class='center'> 
      <div class='school'>{schoolname}</div>
      <div class='bold'>{Location}</div>

</div>
<hr/>
       
        <div class='content'>
           <p class='contentp'>1. We are delighted to extend an offer of admission to <b>{schoolname}</b> for this academic year. It brings us great pleasure to welcome you to our school community, and we are excited about the prospect of having you as a student in our school.</p>
           <p class='contentp'>2. Your application to {schoolname} was thoroughly reviewed by our admissions committee, and we were impressed by your academic potential, your eagerness to learn, and the positive qualities you exhibited during the admission process. Your enthusiasm for education and your readiness to engage with our school curriculum make you an excellent fit for our school.</p>
           <p class='contentp'>3. Your student identification number is {student.StudentId}. This is the number you will use throughout your studies at our school. Your password is {auth.RawPassword}; please be sure not to misplace this information.</p>
           <p class='contentp'>4. For the {student.TheAcademicYear} academic year, the Admission Fee is {adfee?.Amount}, and the Tuition Fee, along with other fees, for {student.Level} {student.TheAcademicTerm} is {otherfee}.</p>
           <p class='contentp'>5. Your total provisional fees for {student.Level} {student.TheAcademicTerm} in the {student.TheAcademicYear} academic year amount to {fees}. Please make this payment to the school's accountant only.</p>
            <p class='contentp'>6. If you have any questions or require further assistance, please feel free to contact our admissions office. We look forward to having you join our {schoolname} family and embark on an exciting educational adventure. We are confident that your time with us will be filled with learning, growth, and lasting friendships.</p>
            <p class='contentp'>7. Congratulations once again on your admission to {schoolname}. We can't wait to get to know you better and watch you thrive as a member of our school community.</p>
       
        <div class='right'>
        Yours Faithfully,<br/>

        .........................<br/>
        {inst.AdminName}<br/>
        {inst.Position}
        </div>
       
        </div>
        <br/><br/>

        <div class='center'> 
       <img src='{studentImage}' alt='Student Image' />
        </div>

 
    </div>
</body>
</html>";



            PdfGenerator.AddPdfPages(document,htmlelement,PageSize.A4);
            byte[] response = null;
            using (MemoryStream ms = new MemoryStream()){
               document.Save(ms);
               response = ms.ToArray();
            }
            return File(response,"application/pdf",$"{fullname}.pdf");

        }


    }
}