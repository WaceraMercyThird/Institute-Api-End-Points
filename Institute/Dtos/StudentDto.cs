using System.ComponentModel.DataAnnotations;

namespace Institute.Dtos
{
    public class StudentDto
    {
        [Required]
        public string Name { get; set; }
        public int Age { get; set; }
        public string ClassRoom { get; set; }
        public int Grades { get; set; }
    }
}
