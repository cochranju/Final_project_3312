namespace GradesApp.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Grade> Grades { get; set; }
    }
}
