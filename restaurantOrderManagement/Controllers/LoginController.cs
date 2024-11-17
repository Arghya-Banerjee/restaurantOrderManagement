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
                UserLogin obj = new UserLogin();
                obj.Opmode = 0;
                obj.UserId = username;
                obj.PassCode = password;
                var objLogin = DBOperations<UserLogin>.GetSpecific(obj, Constant.usp_Login);

                UserSec objsession = new UserSec();
                
                if(objLogin != null && objLogin.PassCode == obj.PassCode)
                {
                    objsession.Id = objLogin.Id;
                    objsession.MobileNumber = objLogin.MobileNumber;
                    objsession.Email = objLogin.Email;
                    objsession.UserId = objLogin.UserId;
                    objsession.UserType = objLogin.UserType;

                    HttpContext.Session.SetObjectAsJson("SessionDetails", objsession);
                }
                else
                {
                    throw new Exception("Invalid login credential!");
                }

                return Json(new { success = 1, message = "Login Successfull" });
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
