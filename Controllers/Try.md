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
        if(student==null || inst==null){
            return BadRequest("Student Not Found");
        }

            var document = new PdfDocument();

           
           string studentImage = "http://" + HttpContext.Request.Host.Value + "/" + student.ProfilePic;
           string schoolImage = "http://" + HttpContext.Request.Host.Value + "/"+ inst.Logo;
           var schoolname = inst.SchoolName;
           var Location = inst.Location;
           var Registerer = inst.AdminName;

        string htmlelement = @"
    <div style='width:100%; padding: 20px; font-family: Arial, sans-serif; background-image: url(" + schoolImage + @"); background-size: cover; background-position: center;'>
        <div style='text-align: center;'>
            <img src='" + schoolImage + @"' alt='School Logo' style='width: 120px; height: auto;' />
            <h1 style='font-size: 24px; margin-top: 20px;'>" + schoolname + @"</h1>
            <p>" + Location + @"</p>
        </div>
        <hr style='border: 1px solid #ccc; margin: 20px 0;' />
        <p style='font-size: 16px; margin-bottom: 20px;'>
            Dear [Student Name],<br /><br />
            We are pleased to inform you that you have been accepted for admission to " + schoolname + @". 
            Your academic achievements and extracurricular activities have impressed our admissions committee.
        </p>
        <p style='font-size: 16px; margin-bottom: 20px;'>
            <strong>Admission Details:</strong><br />
            Admission ID: [Admission ID]<br />
            Program: [Program Name]<br />
            Start Date: [Start Date]<br />
        </p>
        <p style='font-size: 16px; margin-bottom: 20px;'>
            Please find attached your admission letter for your reference. 
            We look forward to welcoming you to our school community.
        </p>
        <div style='text-align: center;'>
            <img src='" + studentImage + @"' alt='Student Photo' style='width: 100px; height: 100px; border-radius: 50%; margin-top: 20px;' />
        </div>
        <p style='font-size: 16px; margin-top: 20px;'>
            Sincerely,<br />
            " + Registerer + @"<br />
            Head Teacher<br />
            " + schoolname + @"
        </p>
    </div>";




            PdfGenerator.AddPdfPages(document,htmlelement,PageSize.A4);
            byte[] response = null;
            using (MemoryStream ms = new MemoryStream()){
               document.Save(ms);
               response = ms.ToArray();
            }
            return File(response,"application/pdf","AddmissionLetter.pdf");

        }


    }
}