using Core;
using Microsoft.AspNetCore.Mvc;
using restaurantOrderManagement.Models;
using restaurantOrderManagement.Utility_Classes;
using restaurantOrderManagement.ViewModels;
using System.Globalization;

namespace restaurantOrderManagement.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult WelcomeManager()
        {
            var sessionDetails = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails");

            if (sessionDetails == null || sessionDetails.vUserRole != UserRole.Manager)
            {
                return RedirectToAction("LoginPage", "Login");
            }

            string userName = sessionDetails.UserId;
            userName = StringUtility.toTitleCase(userName);

            ViewBag.UserId = sessionDetails.UserId;

            return View();
        }

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
    }
}
