#nullable disable
using Institute.Datas.Models;
using Institute.Datas.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
        _logger.LogInformation("Serilog works on get apis");

        try
        {
            return await _IStudentService.GetStudents();
        }
        catch (Exception e)
        {
            _logger.LogInformation($"{e.Message}");
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
    public ActionResult<Student> CreateStudent(Student student)
    {
        if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }

            try
            {
                 _context.Students.Add(student);

                 _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return CreatedAtAction(nameof(GetStudentbyId), new { id = student.Id }, student);

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
        if(_context.Students == null)
        {
            return NotFound();
        }
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
            Console.WriteLine(e);
            throw;
        }
       
    }
    private bool StudentExist(Guid id)
    {
        return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}