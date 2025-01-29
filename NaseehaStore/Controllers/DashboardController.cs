using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaseehaStore.Models.Data;
using System.Data;

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
                .Where(o => o.IsExportedToExcel == false || o.IsExportedToExcel == null) // Filter orders
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
        public IActionResult AllOrders()
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

        public IActionResult SetOrderAsPrepared(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Toggle the IsDelivered state
            order.IsPrepared = !order.IsPrepared;
            _context.SaveChanges();

            return RedirectToAction("Index", "Dashboard");
        }
        public IActionResult SetAsDelivered(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Toggle the IsShipped state
            order.IsShipped = !order.IsShipped;
            order.IsExportedToExcel = !order.IsExportedToExcel;
            _context.SaveChanges();

            return RedirectToAction("Index", "Dashboard");
        }
        public IActionResult PreparedOrders()
        {
            // Fetch only confirmed orders that have not been exported to Excel
            var confirmedOrders = _context.Orders
                .Include(o => o.Student)
                .Where(o => o.IsPrepared && (o.IsExportedToExcel == false || o.IsExportedToExcel == null))
                .ToList();

            return View(confirmedOrders); // Reuse the Index view for displaying orders
        }

        public IActionResult ShippedOrders()
        {
            // Fetch only delivered orders that have not been exported to Excel
            var deliveredOrders = _context.Orders
                .Include(o => o.Student)
                .Where(o => o.IsShipped)
                .ToList();

            return View(deliveredOrders); // Reuse the Index view for displaying orders
        }

        [HttpPost]
        public FileResult ExportToExcel()
        {
            // Fetch data for orders that are prepared but not shipped and haven't been exported to Excel
            var orders = _context.Orders
                .Include(o => o.Student)
                .Where(o => o.IsPrepared && !o.IsShipped && (o.IsExportedToExcel == null || o.IsExportedToExcel == false))
                .Select(o => new
                {
                    o.Id,
                    StudentName = o.Student.FullName,
                    o.Student.Location,
                    o.Price,
                    OrderDate = o.OrderDate.ToString("yyyy-MM-dd")
                })
                .ToList();

            // Create a DataTable for Excel export
            DataTable dt = new DataTable("DeliveredOrders");
            dt.Columns.AddRange(new DataColumn[]
            {
        new DataColumn("Order ID"),
        new DataColumn("Student Name"),
        new DataColumn("Location"),
        new DataColumn("Price"),
        new DataColumn("Order Date")
            });

            foreach (var order in orders)
            {
                dt.Rows.Add(order.Id, order.StudentName, order.Location, order.Price, order.OrderDate);
            }

            // Mark orders as shipped and exported
            var exportedOrderIds = orders.Select(o => o.Id).ToList();
            var exportedOrders = _context.Orders.Where(o => exportedOrderIds.Contains(o.Id)).ToList();
            foreach (var order in exportedOrders)
            {
                order.IsShipped = true; // Mark as shipped
                order.IsExportedToExcel = true; // Mark as exported
            }
            _context.SaveChanges();

            // Generate Excel file
            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DeliveredOrders.xlsx");
                }
            }
        }
    }
}
