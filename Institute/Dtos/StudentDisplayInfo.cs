using System.ComponentModel.DataAnnotations;

namespace Institute.Dtos
{
    public class StudentDisplayInfo
    {

        
            public string Name { get; set; }
        public int age { get; set; }
            public string ClassRoom { get; set; }
            public int Grades { get; set; }

        public class Person
        {
            public string firstName { get; set; }
            public string lastname { get; set; }
        }
           
        
        
    }
}
