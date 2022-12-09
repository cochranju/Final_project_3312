namespace GradesApp.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Grade> Grades { get; set; }
    }
}
