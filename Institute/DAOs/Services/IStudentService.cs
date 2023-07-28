using Institute.Datas.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Transactions;

namespace Institute.Datas.Services;

public interface IStudentService
{
    public Task<List<Student>> GetStudents();

    public Task<Student> GetStudent(Guid id);

    public Task<byte[]> GeneratePdf(List<Student> students);



}
