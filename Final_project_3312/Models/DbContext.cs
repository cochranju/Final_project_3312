using Microsoft.EntityFrameworkCore;

namespace GradesApp.Models
{
    public class GradesContext : DbContext
    {
        public GradesContext(DbContextOptions<GradesContext> options)
            : base(options)
        { }

        public DbSet<Grade> Grades { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
    }
}