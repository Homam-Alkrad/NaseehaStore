using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaseehaStore.Models.Data;
using System.Linq;

namespace NaseehaStore.Controllers
{
    public class CoursesController : Controller
    {
        private readonly NaseehaStoreContext _context;

        public CoursesController(NaseehaStoreContext context)
        {
            _context = context;
        }

        // GET: Courses
        public IActionResult Index()
        {
            var courses = _context.Courses.ToList();
            return View(courses);
        }
        [HttpGet]
        public IActionResult CreateCourse()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                // Save the uploaded image path
                var file = Request.Form.Files.FirstOrDefault();
                if (file != null)
                {
                    var filePath = Path.Combine("wwwroot/images", file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    course.ImagePath = $"/images/{file.FileName}";
                }

                // Add the course to the database
                _context.Courses.Add(course);
                _context.SaveChanges();

                // Redirect to a success or course list page
                return RedirectToAction("Index");
            }

            return View(course);
        }
        // GET: Courses/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound("Course not found.");
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        public IActionResult Edit(Course course)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }

            var existingCourse = _context.Courses.Find(course.Id);
            if (existingCourse == null)
            {
                return NotFound("Course not found.");
            }

            existingCourse.CourseName = course.CourseName;
            existingCourse.Description = course.Description;
            existingCourse.Price = course.Price;
            existingCourse.ImagePath = course.ImagePath;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Courses/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound("Course not found.");
            }
            return View(course);
        }

        public IActionResult ConfirmDelete(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound("Course not found.");
            }

            _context.Courses.Remove(course);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
