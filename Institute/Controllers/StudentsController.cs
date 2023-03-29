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

    public StudentsController(IStudentService service, ApiDbContext context)
    {
        _IStudentService = service;
        _context = context;
    }
    
    [HttpGet]
    public async Task<List<Student>> GetStudents()
    {
        return await _IStudentService.GetStudents();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudentbyId(Guid id)
    {
        if (_context.Students == null)
        {
            return NotFound();
        }
        var student = await _IStudentService.GetStudent(id);
        if (student == null)
        {
            return NotFound();
        }

        return student;
    }

    [HttpPost]
    public async Task<ActionResult<Student>> CreateStudent(Student student)
    {
        if (! ModelState.IsValid)
        {
            return StatusCode(400);
        }

        try
        {
            await _context.Students.AddAsync(student);
        
            await _context.SaveChangesAsync();
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

        try
        {
            if (id != student.Id)
            {
                return NotFound();
            }

             
                
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StudentExist(id))
            {
                return NotFound();
            }
            else
            {
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
        var todoItem = await _context.Students.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.Students.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    private bool StudentExist(Guid id)
    {
        return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}