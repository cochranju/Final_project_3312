using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradesApp.Controllers
{
    public class GradesController : Controller
    {
        // Database context for managing the connection to the database
        private readonly GradesContext _context;

        

        // GET: Grades
        public async Task<IActionResult> Index()
        {
            // Query the database for all grades
            var grades = from g in _context.Grades
                         .Include(g => g.Student)
                         .Include(g => g.Course)
                         .Include(g => g.Assignment)
                         select g;

            // Return the view with the grades
            return View(await grades.ToListAsync());
        }

        // GET: Grades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Query the database for the grade with the specified ID
            var grade = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Course)
                .Include(g => g.Assignment)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (grade == null)
            {
                return NotFound();
            }
// GET: Grades/Create
public IActionResult Create()
{
    // Get the list of students, courses, and assignments to populate the drop-down lists
    ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name");
    ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
    ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Name");

    // Return the view for creating a new grade
    return View();
}

// POST: Grades/Create
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("Id,StudentId,CourseId,AssignmentId,Value")] Grade grade)
{
    if (ModelState.IsValid)
    {
        // Add the new grade to the database
        _context.Add(grade);
        await _context.SaveChangesAsync();

        // Redirect to the index page
        return RedirectToAction(nameof(Index));
    }

    // Get the list of students, courses, and assignments to populate the drop-down lists
    ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name", grade.StudentId);
    ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", grade.CourseId);
    ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Name", grade.AssignmentId);

    // Return the// GET: Grades/Edit/5
public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    // Query the database for the grade with the specified ID
    var grade = await _context.Grades.FindAsync(id);

    if (grade == null)
    {
        return NotFound();
    }

    // Get the list of students, courses, and assignments to populate the drop-down lists
    ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name", grade.StudentId);
    ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", grade.CourseId);
    ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Name", grade.AssignmentId);

    // Return the view for editing the grade
    return View(grade);
}

// POST: Grades/Edit/5
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,CourseId,AssignmentId,Value")] Grade grade)
{
    if (id != grade.Id)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            // Update the grade in the database
            _context.Update(grade);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GradeExists(grade.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        // Redirect to the index page
        return RedirectToAction(nameof(Index));
    }

    // Get the list of students, courses, and assignments to populate the drop-down lists
    ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name", grade.StudentId);
    ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", grade.CourseId);
    ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Name", grade.AssignmentId);

    // Return the view with the validation errors
    return View(grade);
}

// GET: Grades/Delete/5
public async Task<IActionResult> Delete(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    // Query the database for the grade with the specified ID
    var grade = await _context.Grades
        .Include(g => g.Student)
        .Include(g => g.Course)
        .Include(g => g.Assignment)
        .FirstOrDefaultAsync(m => m.Id == id);

    if (grade == null)
    {
        return NotFound();
    }


// POST: Grades/Delete/5
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    // Query the database for the grade with the specified ID
    var grade = await _context.Grades.FindAsync(id);

    // Delete the grade from the database
    _context.Grades.Remove(grade);
    await _context.SaveChangesAsync();

    // Redirect to the index page
    return RedirectToAction(nameof(Index));
}

// GET: Grades/Calculate/5
public async Task<IActionResult> Calculate(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    // Query the database for the grade with the specified ID
    var grade = await _context.Grades
        .Include(g => g.Student)
        .Include(g => g.Course)
        .Include(g => g.Assignment)
        .FirstOrDefaultAsync(m => m.Id == id);

    if (grade == null)
    {
        return NotFound();
    }

    // Get the current total grade and the desired grade from the model
    var model = new CalculateGradeViewModel
    {
        CurrentTotalGrade = grade.Course.TotalGrade,
        DesiredGrade = grade.Course.DesiredGrade
    };

   
// Return the view for calculating the required grade
return View(model);
}

// POST: Grades/Calculate/5
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Calculate(int id, CalculateGradeViewModel model)
{
    if (id == null)
    {
        return NotFound();
    }

    // Query the database for the grade with the specified ID
    var grade = await _context.Grades
        .Include(g => g.Course)
        .FirstOrDefaultAsync(m => m.Id == id);

    if (grade == null)
    {
        return NotFound();
    }

    // Calculate the required grade
    var requiredGrade = (model.DesiredGrade - grade.Course.TotalGrade + model.CurrentTotalGrade) / model.CurrentTotalGrade * 100;

    // Add the calculated grade to the view model
    model.RequiredGrade = requiredGrade;

    // Return the view with the calculated grade
    return View(model);
}

// Helper method for checking if a grade exists
private bool GradeExists(int id)
{
    return _context.Grades.Any(e => e.Id == id);
}

}
}
}
}
}
