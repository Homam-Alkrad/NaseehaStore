using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaseehaStore.Models.Data;
using NaseehaStore.Models.ViewModels;
using System;

namespace NaseehaStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly NaseehaStoreContext _context;
        private readonly WhatsAppService _whatsAppService;


        public OrderController(NaseehaStoreContext context, WhatsAppService whatsAppService)
        {
            _context = context;
            _whatsAppService = whatsAppService;

        }

        [HttpGet]
        public IActionResult Order(int id, decimal price)
        {
            // List of cities in Jordan
            var cities = new List<string>
            {
                "عمّان", "إربد", "الزرقاء", "المفرق", "عجلون", "جرش", "مادبا", "البلقاء", "الكرك", "الطفيلة", "معان", "العقبة"
            };

            // Pass orders and cities to the view
            ViewBag.Cities = cities;
            // Get course details
            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound(); // Return 404 if the course is not found
            }

            // Create and populate the ViewModel
            var viewModel = new OrderViewModel
            {
                CourseId = course.Id,
                CourseName = course.CourseName,
                Price = price,
                Student = new Student()
            };


            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> OrderConfirm(OrderViewModel model)
        {
            // List of cities in Jordan
            var cities = new List<string>
            {
                "عمّان", "إربد", "الزرقاء", "المفرق", "عجلون", "جرش", "مادبا", "البلقاء", "الكرك", "الطفيلة", "معان", "العقبة"
            };

            // Pass orders and cities to the view
            ViewBag.Cities = cities;

            if (!ModelState.IsValid)
            {
                return View("Order", model); // Return to the form if validation fails
            }

            // Save the student in the database
            _context.Students.Add(model.Student);
            await _context.SaveChangesAsync();

            // Create and save the order
            var order = new Order
            {
                CourseId = model.CourseId,
                StudentId = model.Student.Id,
                Price = model.Price,
                OrderDate = DateTime.Now,
                IsDelivered = false,
                IsShipped = false
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Send WhatsApp message
            var whatsAppService = HttpContext.RequestServices.GetService<WhatsAppService>();
            var message = $"تم تسجيل طلبك بنجاح!\n" +
                      $"الطلب: {model.CourseName}\n" +
                      $"رقم الطلب: {order.Id}\n" +
                      $"موقع النصيحة التعليمي";
            await _whatsAppService.SendMessageAsync(model.Student.MainPhone, message);

            // Redirect to the success page
            return RedirectToAction("OrderSuccess", new { orderId = order.Id });
        }

        public IActionResult OrderSuccess(int orderId)
        {
            // Attempt to find the order and include related Course entity
            var order = _context.Orders
                .Include(o => o.Course)
                .Include(o => o.Student)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound("The order you are looking for does not exist.");
            }

            // Pass data to the view using ViewBag
            ViewBag.OrderNumber = order.Id;
            ViewBag.CourseName = order.Course.CourseName;
            ViewBag.TotalPrice = order.Price;

            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var order = _context.Orders
                .Include(o => o.Course)
                .Include(o => o.Student)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }
            // List of cities in Jordan
            var cities = new List<string>
            {
                "عمّان", "إربد", "الزرقاء", "المفرق", "عجلون", "جرش", "مادبا", "البلقاء", "الكرك", "الطفيلة", "معان", "العقبة"
            };

            // Pass orders and cities to the view
            ViewBag.Cities = cities;
            var viewModel = new EditOrderViewModel
            {
                OrderId = order.Id,
                CourseId = order.CourseId,
                CourseName = order.Course.CourseName,
                Price = order.Price,
                Student = order.Student
            };

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Edit(EditOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Return to the form if validation fails
            }

            var order = _context.Orders
                .Include(o => o.Student)
                .FirstOrDefault(o => o.Id == model.OrderId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Update order and student details
            order.Price = model.Price;
            order.Student.FullName = model.Student.FullName;
            order.Student.MainPhone = model.Student.MainPhone;
            order.Student.SecondPhone = model.Student.SecondPhone;
            order.Student.City = model.Student.City;
            order.Student.Location = model.Student.Location;
            order.Student.Details = model.Student.Details;

            _context.SaveChanges();

            return RedirectToAction("Index"); // Redirect to orders list
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders
                .Include(o => o.Course)
                .Include(o => o.Student)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return View(order);
        }
        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            var order = _context.Orders
                .Include(o => o.Student)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Optionally, delete the student if no other orders are associated
            var student = order.Student;
            _context.Orders.Remove(order);

            if (!_context.Orders.Any(o => o.StudentId == student.Id))
            {
                _context.Students.Remove(student);
            }

            _context.SaveChanges();

            return RedirectToAction("Index","Dashboard"); // Redirect to orders list
        }
        public IActionResult ConfirmOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Toggle the IsDelivered state
            order.IsDelivered = !order.IsDelivered;
            _context.SaveChanges();

            return RedirectToAction("Index", "Dashboard");
        }
        public IActionResult MarkAsDelivered(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Toggle the IsShipped state
            order.IsShipped = !order.IsShipped;
            _context.SaveChanges();

            return RedirectToAction("Index", "Dashboard");
        }
        public IActionResult ConfirmedOrders()
        {
            // Fetch only confirmed orders
            var confirmedOrders = _context.Orders
                .Include(o => o.Student)
                .Where(o => o.IsDelivered)
                .ToList();

            return View(confirmedOrders); // Reuse the Index view for displaying orders
        }
        public IActionResult DeliveredOrders()
        {
            // Fetch only delivered orders
            var deliveredOrders = _context.Orders
                .Include(o => o.Student)
                .Where(o => o.IsShipped)
                .ToList();

            return View(deliveredOrders); // Reuse the Index view for displaying orders
        }


    }
}
