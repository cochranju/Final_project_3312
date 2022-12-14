namespace GradesApp.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Name { get; set; } 

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public ICollection<Grade> Grades { get; set; }
    }
}
