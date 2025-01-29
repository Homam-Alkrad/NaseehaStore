using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaseehaStore.Models;
using NaseehaStore.Models.Data;
using NaseehaStore.Models.ViewModels;
using System.Diagnostics;

namespace NaseehaStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NaseehaStoreContext _context;


        public HomeController(ILogger<HomeController> logger, NaseehaStoreContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Shop()
        {
            // Fetch the list of courses from the database
            var courses = _context.Courses.ToList();

            // Pass the courses to the view
            return View(courses);
        }
        // طريقة GET لعرض صفحة البحث
        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        // طريقة POST لمعالجة بيانات البحث
        [HttpPost]
        public IActionResult Search(SearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                // تحقق من الطلب بناءً على OrderId وPhoneNumber
                var order = _context.Orders
                    .Include(o => o.Student)
                    .FirstOrDefault(o => o.Id == model.OrderId && o.Student.MainPhone == model.PhoneNumber);

                if (order != null)
                {
                    // إعادة التوجيه إلى إجراء يعرض معلومات الطلب
                    return RedirectToAction("OrderInfo", new { id = order.Id });
                }
                else
                {
                    // عرض رسالة خطأ إذا لم يتم العثور على الطلب
                    ModelState.AddModelError("", "لم يتم العثور على طلب بالبيانات المدخلة.");
                }
            }

            return View(model);
        }

        public IActionResult OrderInfo(int id)
        {
            var order = _context.Orders
                .Include(o => o.Student)
                .Include(o => o.Course)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound("الطلب غير موجود.");
            }

            // تحديد رسالة الحالة بناءً على حالة الطلب
            string statusMessage;
            if (!order.IsDelivered && !order.IsShipped)
            {
                statusMessage = "يرجى العلم بأننا قد تلقينا طلبك وهو الآن قيد التجهيز";
            }
            else if (order.IsDelivered && !order.IsShipped)
            {
                statusMessage = "تم تجهيز طلبك وسنقوم بشحنه إليك في أقرب وقت ممكن";
            }
            else if (order.IsDelivered && order.IsShipped)
            {
                statusMessage = "تم تجهيز طلبك وشحنه وهو في طريقه إليك مع مندوب التوصيل";
            }
            else
            {
                statusMessage = "حالة الطلب غير معروفة. يرجى التواصل مع الدعم";
            }

            // إنشاء نموذج العرض
            var viewModel = new OrderInfoViewModel
            {
                OrderId = order.Id,
                StudentName = order.Student.FullName,
                Location = order.Student.Location,
                Details = order.Course.Description,
                Price = order.Price,
                OrderStatusMessage = statusMessage
            };

            return View(viewModel);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
