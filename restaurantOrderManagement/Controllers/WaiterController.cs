using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using restaurantOrderManagement.Models;  // Import your model or namespace for session handling

using System.Globalization;
using restaurantOrderManagement.Utility_Classes;

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

        public IActionResult WaiterAddMenuView()
        {
            var sessionDetails = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails");

            if(sessionDetails == null || sessionDetails.vUserRole != UserRole.waiter)
            {
                return RedirectToAction("LoginPage", "Login");
            }

            ViewBag.userId = sessionDetails.UserId;

            return View();
        }

        public IActionResult WaiterAddMenu(MenuModel menu)
        {
            menu.Opmode = 3;
            UserSec userSession = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails");
            string userid = userSession.UserId;
            userid = StringUtility.toTitleCase(userid);
            menu.createdby = userid;

            int res = DBOperations<MenuModel>.DMLOperation(menu, Constant.usp_Menu);

            return RedirectToAction("WelcomeWaiter", "Waiter");
        }
    }
}
