using Institute.Datas.Models;

namespace Institute.Datas.Services;

public interface IStudentService
{
    public Task<List<Student>> GetStudents();

    public Task<Student> GetStudent(Guid id);
}