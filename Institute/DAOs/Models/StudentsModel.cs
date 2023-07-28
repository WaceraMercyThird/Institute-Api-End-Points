#nullable disable
using System.ComponentModel.DataAnnotations;

namespace Institute.Datas.Models
{
    public class Student

    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string ClassRoom { get; set; }
        public int Grades { get; set; }
        public string Novel { get; set; }
    }
}