using OfficeOpenXml;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HDSS_BACKEND.Models;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{

    
    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadExcel()
    {
        try
        {
            var file = Request.Form.Files[0]; // Assuming you're sending the file in the body of a POST request.

            if (file.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first sheet.

                        int rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++) // Assuming the first row contains headers.
                        {
                            var student = new Student
                            {
                                StudentId = worksheet.Cells[row, 1].Value?.ToString(),
                                FirstName = worksheet.Cells[row, 2].Value?.ToString(),
                                OtherName = worksheet.Cells[row, 3].Value?.ToString(),
                                LastName = worksheet.Cells[row, 4].Value?.ToString(),
                                DateOfBirth = worksheet.Cells[row, 5].Value?.ToString(),
                                Gender = worksheet.Cells[row, 6].Value?.ToString(),
                                HomeTown = worksheet.Cells[row, 7].Value?.ToString(),
                                Location = worksheet.Cells[row, 8].Value?.ToString(),
                                Country = worksheet.Cells[row, 9].Value?.ToString(),
                                FathersName = worksheet.Cells[row, 10].Value?.ToString(),
                                FatherOccupation = worksheet.Cells[row, 11].Value?.ToString(),
                                MothersName = worksheet.Cells[row, 12].Value?.ToString(),
                                MotherOccupation = worksheet.Cells[row, 13].Value?.ToString(),
                                GuardianName = worksheet.Cells[row, 14].Value?.ToString(),
                                GuardianOccupation = worksheet.Cells[row, 15].Value?.ToString(),
                                ParentLocation = worksheet.Cells[row, 16].Value?.ToString(),
                                ParentDigitalAddress = worksheet.Cells[row, 17].Value?.ToString(),
                                ParentReligion = worksheet.Cells[row, 18].Value?.ToString(),
                                ParentEmail = worksheet.Cells[row, 19].Value?.ToString(),
                                EmergencyContactName = worksheet.Cells[row, 20].Value?.ToString(),
                                EmergencyPhoneNumber = worksheet.Cells[row, 21].Value?.ToString(),
                                EmergencyAlternatePhoneNumber = worksheet.Cells[row, 22].Value?.ToString(),
                                RelationshipWithChild = worksheet.Cells[row, 23].Value?.ToString(),
                                ParentPhoneNumber = worksheet.Cells[row, 24].Value?.ToString(),
                                Balance = Convert.ToDouble(worksheet.Cells[row, 25].Value),
                                Religion = worksheet.Cells[row, 26].Value?.ToString(),
                                Email = worksheet.Cells[row, 27].Value?.ToString(),
                                PhoneNumber = worksheet.Cells[row, 28].Value?.ToString(),
                                AlternatePhoneNumber = worksheet.Cells[row, 29].Value?.ToString(),
                                MedicalIInformation = worksheet.Cells[row, 30].Value?.ToString(),
                                Level = worksheet.Cells[row, 31].Value?.ToString(),
                                amountOwing = Convert.ToDouble(worksheet.Cells[row, 32].Value),
                                creditAmount = Convert.ToDouble(worksheet.Cells[row, 33].Value),
                                AdmissionDate = worksheet.Cells[row, 34].Value?.ToString(),
                                SchoolBankAccount = Convert.ToDouble(worksheet.Cells[row, 35].Value),
                                ProfilePic = worksheet.Cells[row, 36].Value?.ToString(),
                                Role = worksheet.Cells[row, 37].Value?.ToString(),
                                TheAcademicYear = worksheet.Cells[row, 38].Value?.ToString(),
                                TheAcademicTerm = worksheet.Cells[row, 39].Value?.ToString()
                            };

                            // Process the student data (e.g., save to a database).
                            // Your logic here...

                            // For demonstration purposes, print the student data to the console.
                            Console.WriteLine($"Student ID: {student.StudentId}, Name: {student.FirstName} {student.LastName}");
                        }
                    }
                }
                return Ok("File uploaded and processed successfully.");
            }
            else
            {
                return BadRequest("File is empty.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }



}
