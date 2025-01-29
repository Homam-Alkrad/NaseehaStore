using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaseehaStore.Models.Data;

namespace NaseehaStore.Controllers
{
    public class DashboardController : Controller
    {
        private readonly NaseehaStoreContext _context;

        public DashboardController(NaseehaStoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Fetch all orders, including related student and course data
            var orders = _context.Orders
                .Include(o => o.Course)
                .Include(o => o.Student)
                .ToList();

            // List of cities in Jordan
            var cities = new List<string>
        {
            "عمّان", "إربد", "الزرقاء", "المفرق", "عجلون", "جرش", "مادبا", "البلقاء", "الكرك", "الطفيلة", "معان", "العقبة"
        };

            // Pass orders and cities to the view
            ViewBag.Cities = cities;

            return View(orders);
        }
        // Delete Action
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        // Action to get all delivered orders
        public IActionResult DeliveredOrders()
        {
            // Fetch all delivered orders, including related Course and Student
            var deliveredOrders = _context.Orders
                .Include(o => o.Course)
                .Include(o => o.Student)
                .Where(o => o.IsDelivered)
                .ToList();

            return View(deliveredOrders); // Pass to the view
        }
        // Action to get all shipped orders
        public IActionResult ShippedOrders()
        {
            // Fetch all shipped orders, including related Course and Student
            var shippedOrders = _context.Orders
                .Include(o => o.Course)
                .Include(o => o.Student)
                .Where(o => o.IsShipped)
                .ToList();

            return View(shippedOrders); // Pass to the view
        }
    }
}
