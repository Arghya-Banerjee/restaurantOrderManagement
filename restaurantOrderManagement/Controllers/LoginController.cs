using Core;
using Microsoft.AspNetCore.Mvc;
using restaurantOrderManagement.Models;
using restaurantOrderManagement.Utility_Classes;
using System.Globalization;

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
                        //return Json(new { success = 1, message = "Login Successful", redirectUrl = Url.Action("WelcomeWaiter", "Waiter") });
                        
                        ViewBag.UserId = StringUtility.toTitleCase(objsession.UserId);
                        return View("~/Views/waiter/WelcomeWaiter.cshtml");
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
    }
}
