#nullable disable
using Institute.Datas.Models;
using Institute.Datas.Services;
using Institute.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Institute.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : Controller
{
    public readonly IStudentService _IStudentService;
    public readonly ApiDbContext _context;
    private readonly ILogger<Student> _logger;

    public StudentsController(IStudentService service, ApiDbContext context, ILogger<Student> logger
        )
    {
        _IStudentService = service;
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<List<Student>> GetStudents()
    {
        _logger.LogError("Serilog works on get apis");

        try
        {
            return await _IStudentService.GetStudents();
        }
        catch (Exception e)
        {
            _logger.LogError($"{e.Message}");
            throw;
        }

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudentbyId(Guid id)
    {
        if (_context.Students == null)
        {
            return NotFound();
        }
        try
        {
            var student = await _IStudentService.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }

            return student;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


    }



    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Student>> CreateStudent(StudentDto student)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(400);
        }



        Student stud = new Student()
        {
            Id = Guid.NewGuid(),
            Name = student.Name,
            Age = student.Age,
            ClassRoom = student.ClassRoom
        };

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