#nullable disable
using Akka.Actor;
using Akka.DI.Core;
using AutoMapper;
using Institute.Actor;
using Institute.Datas.Models;
using Institute.Datas.Services;
using Institute.Dtos;
using Institute.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Institute.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class StudentsController : ControllerBase
{
    public readonly IStudentService _IStudentService;

    public readonly ApiDbContext _context;

    private readonly ILogger<Student> _logger;

    private readonly IMapper _mapper;

    private object studentDtoMapped;

    private readonly IActorRef _emailActor;




    public StudentsController(
        IStudentService service,

        ApiDbContext context,

        ILogger<Student> logger,

        IMapper mapper,

        IActorRefFactory actorRefFactory

        )
    {
        _IStudentService = service;

        _context = context;

        _logger = logger;

        _mapper = mapper;

        _emailActor = actorRefFactory.ActorOf<EmailActor>();
    }

    [HttpGet]
    public async Task<List<Student>> GetStudents()
    {
        _logger.LogError("Serilog works on get apis");

        try
        {
            var students = await _IStudentService.GetStudents();
            return students;
        }
        catch (Exception e)
        {
            _logger.LogError($"{e.Message}");
            throw;
        }

    }

    [HttpGet]
    public async Task<ActionResult<StudentDisplayInfo>> GetStudentbyId(Guid id)
    {
        if (_context.Students == null)
        {
            return NotFound();
        }
        try
        {
            var student = await _IStudentService.GetStudent(id);


            var studentToDisplay = new StudentDisplayInfo()
            {
                Name = student.Name,
                age = student.Age,
                ClassRoom = student.ClassRoom,
                Grades = student.Grades,


            };

            if (student == null)
            {
                return NotFound();
            }

            return studentToDisplay;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    [HttpGet]
    public async Task<IActionResult> GeneratePdf()
    {
        var students = await _IStudentService.GetStudents();

        // Generate the PDF using the service method
        var pdfBytes = await _IStudentService.GeneratePdf(students);

        // Create an HTTP response with the PDF as content
        return File(pdfBytes, "application/pdf", "students.pdf");
    }

    //[HttpGet]
    //public async Task<byte[]> GeneratePdf()
    //{
    //    var students = await _IStudentService.GetStudents();

    //    // Generate the PDF using the service method
    //    var pdfBytes = await _IStudentService.GeneratePdf(students);

    //    // Return the PDF as a byte array
    //
    //  return pdfBytes;
    //}
    [HttpGet]
    public async Task<IActionResult> SendReport(string emailAddress)
    {
        try
        {
            var students = await _IStudentService.GetStudents();

            // Generate the PDF using the service method
            var pdfBytes = await _IStudentService.GeneratePdf(students);

            // Compose the email message
            var emailMessage = new ReportResponse
            {
                To = emailAddress,
                Subject = "Your Students PDF",
                Body = "Please find the attached PDF with student details.",
                Attachment = pdfBytes,
            };

            // Send the email message to the Email Actor using Ask pattern
            var result = await _emailActor.Ask<string>(emailMessage);



            return Ok(result);

        }

        catch (Exception e)
        {

            _logger.LogError(JsonConvert.SerializeObject(e));

            return StatusCode(500);
        }

    }




    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateStudent(StudentDto student)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(400);
        }

        //Student stud = new Student()
        //{
        //    Id = Guid.NewGuid(),
        //    Name = student.Name,
        //    Age = student.Age,
        //    ClassRoom = student.ClassRoom,
        //    Grades = student.Grades
        //};
        var stud = _mapper.Map<Student>(student);

        try
        {


            await _context.Students.AddAsync(stud);

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(JsonConvert.SerializeObject(e));

            return StatusCode(500);
        }

        return CreatedAtAction(nameof(GetStudentbyId), new { id = stud.Id }, student);


    }

    [HttpPut]
    public async Task<ActionResult<Student>> UpdateStudent(Guid id, [FromBody] Student student)
    {
        _context.Entry(student).State = EntityState.Modified;

        _logger.LogInformation("Serilog works on put apis");

        try
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            _context.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StudentExist(id))
            {
                return NotFound();
            }
            else
            {
                _logger.LogInformation($"Student exist {id}");
                throw;
            }
        }

        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(Guid id)
    {
        if (_context.Students == null)
        {
            return NotFound();
        }
        _logger.LogError("Serilog from delete method works");
        try
        {
            var todoItem = await _context.Students.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Students.Remove(todoItem);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(JsonConvert.SerializeObject(e.Message));
            throw;
        }

    }
    private bool StudentExist(Guid id)
    {
        return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
    }



}



