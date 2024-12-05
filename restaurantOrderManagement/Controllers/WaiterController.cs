using Microsoft.AspNetCore.Mvc;
using restaurantOrderManagement.Models;  // Import your model or namespace for session handling

using System.Globalization;

namespace restaurantOrderManagement.Controllers
{
    public class WaiterController : Controller
    {
        public IActionResult WelcomeWaiter()
        {
            var sessionDetails = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails");

            if (sessionDetails == null || sessionDetails.vUserRole != UserRole.waiter)
            {
                return RedirectToAction("LoginPage", "Login");
            }

            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

            string userName = sessionDetails.UserId;
            userName = ti.ToTitleCase(userName);
            Console.WriteLine(userName);

            ViewBag.UserId = sessionDetails.UserId;

            return View();
        }
    }
}
