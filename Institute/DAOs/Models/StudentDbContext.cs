#nullable disable
using Institute.Datas.Models;
using Microsoft.EntityFrameworkCore;

namespace Institute;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }

    internal Task GetStudent(Guid id)
    {
        throw new NotImplementedException();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().HasIndex(p => p.Name).IsUnique();

        modelBuilder.Entity<Student>().Property(p => p.Name).HasMaxLength(30);
    }
}


