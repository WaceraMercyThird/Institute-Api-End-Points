using Microsoft.EntityFrameworkCore;

namespace Institute.Datas.Models;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options, DbSet<Student> students) : base(options)
    {
        Students = students;
    }

    public DbSet<Student> Students { get; set; }
}


