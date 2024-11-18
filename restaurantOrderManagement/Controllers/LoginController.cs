using Core;
using Microsoft.AspNetCore.Mvc;
using restaurantOrderManagement.Models;

namespace restaurantOrderManagement.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Validate(string username, string password)
        {
            try
            {
                UserLogin obj = new UserLogin
                {
                    Opmode = 0,
                    UserId = username,
                    PassCode = password
                };

                var objLogin = DBOperations<UserLogin>.GetSpecific(obj, Constant.usp_Login);
                UserSec objsession = new UserSec();

                if (objLogin != null && objLogin.PassCode == obj.PassCode)
                {
                    objsession.Id = objLogin.Id;
                    objsession.MobileNumber = objLogin.MobileNumber;
                    objsession.Email = objLogin.Email;
                    objsession.UserId = objLogin.UserId;
                    objsession.UserType = objLogin.UserType;

                    if (objLogin.UserType == 1) // Waiter user
                    {
                        objsession.vUserRole = UserRole.waiter;
                        HttpContext.Session.SetObjectAsJson("SessionDetails", objsession);

                        // Return JSON response with redirect URL
                        return Json(new { success = 1, message = "Login Successful", redirectUrl = Url.Action("WelcomeWaiter", "Waiter") });

                    }
                }
                else
                {
                    throw new Exception("Invalid login credential!");
                }

                return Json(new { success = 0, message = "User not found!" });
            }
            catch (Exception ex)
            {
                var ERRUSERID = "TRY LOGIN FROM - " + username;
                Utility.SaveSystemErrorLog(ex, ERRUSERID);
                return Json(new { success = 0, message = ex.Message });
            }
        }


        public IActionResult WelcomeWaiter()
        {
            // Retrieve session details
            var sessionDetails = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails");

            // Check if session is null or user is not a waiter, redirect to LoginPage
            if (sessionDetails == null || sessionDetails.vUserRole != UserRole.waiter)
            {
                return RedirectToAction("LoginPage");
            }

            // Pass user ID to the view
            ViewBag.UserId = sessionDetails.UserId;
            return View("~/Views/waiter/WelcomeWaiter.cshtml");
        }






    }
}
