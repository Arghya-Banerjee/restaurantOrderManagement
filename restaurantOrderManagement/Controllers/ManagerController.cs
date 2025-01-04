using Core;
using Microsoft.AspNetCore.Mvc;
using restaurantOrderManagement.Models;
using restaurantOrderManagement.Utility_Classes;
using restaurantOrderManagement.ViewModels;
using System.Globalization;

namespace restaurantOrderManagement.Controllers
{
    [Route("[controller]")]
    public class ManagerController : Controller
    {
        [Route("Welcome")]
        [Route("WelcomeManager")]
        public IActionResult WelcomeManager()
        {
            var sessionDetails = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails");

            if (sessionDetails == null || sessionDetails.vUserRole != UserRole.Manager)
            {
                return RedirectToAction("LoginPage", "Login");
            }

            string userName = sessionDetails.UserID;
            userName = StringUtility.toTitleCase(userName);

            ViewBag.UserId = sessionDetails.UserID;

            return View();
        }

        [Route("ShowProductDetails")]
        public IActionResult ShowProductDetails()
        {
            var sessionDetails = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails");

            if(sessionDetails == null || sessionDetails.vUserRole != UserRole.Manager)
            {
                return RedirectToAction("LoginPage", "Login");
            }

            List<StockProductDetailsModel> stockProductDetails = new List<StockProductDetailsModel>();
            StockProductDetailsModel dummyStock = new StockProductDetailsModel();
            dummyStock.OpMode = 0; // Fetch all details dummy
            stockProductDetails = DBOperations<StockProductDetailsModel>.GetAllOrByRange(dummyStock, Constant.usp_StockManager);

            StockCategoryModel dummyCategory = new StockCategoryModel();
            List<StockCategoryModel> stockCategories = new List<StockCategoryModel>();
            dummyCategory.OpMode = 1; // Fetch all categories
            stockCategories = DBOperations<StockCategoryModel>.GetAllOrByRange(dummyCategory, Constant.usp_StockManager);

            var productDetails = new ProductDetailsViewModel
            {
                stockProductDetails = stockProductDetails,
                stockCategory = stockCategories
            };

            return View(productDetails);
        }

        [Route("ShowOrderHistory")]
        public IActionResult ShowOrderHistory()
        {
            var sessionDetails = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails");

            if (sessionDetails == null || sessionDetails.vUserRole != UserRole.Manager)
            {
                return RedirectToAction("LoginPage", "Login");
            }

            List<UserModel> userlist = DBOperations<UserModel>.GetAllOrByRange(new UserModel
            {
                OpMode = 0,
                UserType = 1 // Waiter
            }, Constant.usp_User);

            return View(userlist);
        }

        [Route("OrderHistory")]
        //[HttpGet]
        public IActionResult OrderHistory(string StartDate, string EndDate, string WaiterName = "All", string ViewType = "Aggregate")
        {
            var sessionDetails = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails");

            if (sessionDetails == null || sessionDetails.vUserRole != UserRole.Manager)
            {
                return RedirectToAction("LoginPage", "Login");
            }

            DateTime startDate = DateTime.Parse(StartDate);
            DateTime endDate = DateTime.Parse(EndDate);

            string waiter = WaiterName;
            string viewType = ViewType;

            if (ViewType == "Aggregate")
            {
                List<OrderHistoryModel> orderHistoryList = DBOperations<OrderHistoryModel>.GetAllOrByRange(new OrderHistoryModel
                {
                    OpMode = 0,
                    ViewType = 0,
                    StartDate = startDate,
                    EndDate = endDate,
                    UserID = waiter
                }, Constant.usp_OrderHistory);
                ViewBag.ViewTypeNum = 0;
                ViewBag.WaiterName = waiter;
                return View(orderHistoryList);
            }
            else
            {
                List<OrderHistoryModel> orderHistoryList = DBOperations<OrderHistoryModel>.GetAllOrByRange(new OrderHistoryModel
                {
                    OpMode = 0,
                    ViewType = 1,
                    StartDate = startDate,
                    EndDate = endDate,
                    UserID = waiter
                }, Constant.usp_OrderHistory);
                ViewBag.ViewTypeNum = 1;
                ViewBag.WaiterName = waiter;
                return View(orderHistoryList);
            }
        }
    }
}
