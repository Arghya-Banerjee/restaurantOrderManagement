using Core;
using Microsoft.AspNetCore.Mvc;
using restaurantOrderManagement.Models;
using restaurantOrderManagement.Utility_Classes;

namespace restaurantOrderManagement.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        [Route("~/")]
        [Route("Home")]
        public IActionResult LoginPage()
        {
            var sessionDetails = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails");

            if (sessionDetails == null)
            {
                return View();
            }

            // If user is already Logged in Redirect to desired Dashboard
            switch (sessionDetails.UserType)
            {
                case 1: // Waiter user
                    ViewBag.UserId = StringUtility.toTitleCase(sessionDetails.UserID);
                    return RedirectToAction("Welcome", "Waiter");

                case 4: // Manager user
                    ViewBag.UserId = StringUtility.toTitleCase(sessionDetails.UserID);
                    return RedirectToAction("Welcome", "Manager");

                default:
                    return View();
            }
        }

        [HttpPost]
        public IActionResult Validate(string username, string password)
        {
            try
            {
                UserLogin obj = new UserLogin
                {
                    Opmode = 0,
                    UserID = username,
                    PassCode = password
                };

                var objLogin = DBOperations<UserLogin>.GetSpecific(obj, Constant.usp_Login);
                UserSec objsession = new UserSec();

                if (objLogin != null && objLogin.PassCode == obj.PassCode)
                {
                    objsession.Id = objLogin.Id;
                    objsession.MobileNumber = objLogin.MobileNumber;
                    objsession.Email = objLogin.Email;
                    objsession.UserID = objLogin.UserID;
                    objsession.UserType = objLogin.UserType;

                    if (objLogin.UserType == 1) // Waiter user
                    {
                        objsession.vUserRole = UserRole.Waiter;
                        HttpContext.Session.SetObjectAsJson("SessionDetails", objsession);
                        
                        ViewBag.UserId = StringUtility.toTitleCase(objsession.UserID);
                        return RedirectToAction("Welcome", "Waiter");
                    }
                    if (objLogin.UserType == 4) // Manager user
                    {
                        objsession.vUserRole = UserRole.Manager;
                        HttpContext.Session.SetObjectAsJson("SessionDetails", objsession);

                        ViewBag.UserId = StringUtility.toTitleCase(objsession?.UserID);
                        return RedirectToAction("Welcome", "Manager");
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

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("LoginPage", "Login");
        }
    }
}
