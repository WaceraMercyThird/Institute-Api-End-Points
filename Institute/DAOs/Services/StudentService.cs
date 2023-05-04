using Institute.Datas.Models;
using Microsoft.EntityFrameworkCore;

namespace Institute.Datas.Services;

public class StudentService : IStudentService
{
    public readonly ApiDbContext  _context;

    public StudentService(ApiDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Student>> GetStudents()
    {
        return await _context.Students.ToListAsync();
    }

    public async Task<Student> GetStudent(Guid id)
    {
        return await _context.Students.FindAsync(id);

    }
}

