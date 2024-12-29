using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using restaurantOrderManagement.Models;  // Import your model or namespace for session handling

using System.Globalization;
using restaurantOrderManagement.Utility_Classes;
using restaurantOrderManagement.ViewModels;
using System.ComponentModel;

namespace restaurantOrderManagement.Controllers
{
    public class WaiterController : Controller
    {
        public IActionResult WelcomeWaiter()
        {
            var sessionDetails = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails");

            if (sessionDetails == null || sessionDetails.vUserRole != UserRole.Waiter)
            {
                return RedirectToAction("LoginPage", "Login");
            }

            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

            string userName = sessionDetails.UserID;
            userName = ti.ToTitleCase(userName);
            Console.WriteLine(userName);

            ViewBag.UserId = sessionDetails.UserID;

            return View();
        }

        public IActionResult WaiterAddMenuView()
        {
            var sessionDetails = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails");

            if(sessionDetails == null || sessionDetails.vUserRole != UserRole.Waiter)
            {
                return RedirectToAction("LoginPage", "Login");
            }

            ViewBag.userId = sessionDetails.UserID;

            return View();
        }

        public IActionResult WaiterAddMenu(MenuModel menu)
        {
            menu.Opmode = 3;
            UserSec userSession = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails");
            string userid = userSession.UserID;
            userid = StringUtility.toTitleCase(userid);
            menu.createdby = userid;

            int res = DBOperations<MenuModel>.DMLOperation(menu, Constant.usp_Menu);

            return RedirectToAction("WelcomeWaiter", "Waiter");
        }

        public IActionResult WaiterTakeOrder()
        {
            var sessionDetails = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails");
            if(sessionDetails == null || sessionDetails.vUserRole != UserRole.Waiter)
            {
                return RedirectToAction("LoginPage", "Login");
            }

            string userid = StringUtility.toTitleCase(sessionDetails.UserID);
            MenuModel menuDummy = new MenuModel();
            menuDummy.Opmode = 0;
            List<MenuModel> menuList = DBOperations<MenuModel>.GetAllOrByRange(menuDummy, Constant.usp_Menu);
            return View(menuList);
        }

        public IActionResult CurrentOrders()
        {
            System.Diagnostics.Debug.WriteLine("Showing Current Orders !!");

            string userId = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails").UserID;

            List<CurrentOrdersModel> currOrders = new List<CurrentOrdersModel>();
            CurrentOrdersModel currOrderDummy = new CurrentOrdersModel();
            currOrderDummy.OpMode = 1;
            currOrders = DBOperations<CurrentOrdersModel>.GetAllOrByRange(currOrderDummy, Constant.usp_CurrentOrders);

            System.Diagnostics.Debug.WriteLine("Return current orders view !!");
            return View(currOrders);
        }

        [HttpGet]
        public IActionResult ShowBill(int tableNumber)
        {
            List<BillItemsModel> billItemList = new List<BillItemsModel>();
            BillItemsModel billItemDummy = new BillItemsModel();
            billItemDummy.OpMode = 0;
            billItemDummy.TableNumber = tableNumber;
            billItemList = DBOperations<BillItemsModel>.GetAllOrByRange(billItemDummy, Constant.usp_BillItems);

            int taxPercentage = 5;
            decimal totalAmt = 0;
            foreach (var item in billItemList) 
            {
                totalAmt += (item.foodprice * item.Quantity);
            }
            decimal amtIncludingGST = (totalAmt * (100 + taxPercentage)) / 100;

            OrderHeaderModel orderHeaderDummy = new OrderHeaderModel();
            orderHeaderDummy.Opmode = 3;
            orderHeaderDummy.TableNumber = tableNumber;
            orderHeaderDummy.OrderDate = DateTime.Now;
            orderHeaderDummy.CreatedOn = DateTime.Now;
            OrderHeaderModel orderDetails = DBOperations<OrderHeaderModel>.GetSpecific(orderHeaderDummy, Constant.usp_OrderHeader);
            long orderId = orderDetails.OrderID;
            ViewBag.OrderID = orderId; // Adding orderID to ViewBag to pass to Rate Us
            DateTime orderTime = orderDetails.CreatedOn;

            InvoiceModel invoice = new InvoiceModel();
            invoice.OpMode = 1;
            invoice.OrderID = orderId;
            invoice.InvoiceDate = DateTime.Now;
            invoice.AmountExcludingGST = totalAmt;
            invoice.GSTAmount = amtIncludingGST - totalAmt;
            invoice.AmountIncludingGST = amtIncludingGST;
            invoice.PaymentMode = 0; // default: cash
            invoice.CreatedBy = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails").UserID;
            invoice.CreatedOn = DateTime.Now;
            int res = DBOperations<InvoiceModel>.DMLOperation(invoice, Constant.usp_Invoice);

            invoice.OpMode = 0;
            invoice.InvoiceID = 0;
            long invoiceId = DBOperations<InvoiceModel>.GetSpecific(invoice, Constant.usp_Invoice).InvoiceID;

            BillViewModel billDetails = new BillViewModel();
            billDetails.InvoiceID = invoiceId;
            billDetails.OrderID = orderId;
            billDetails.BillItems = billItemList;
            billDetails.TableNumber = tableNumber;
            billDetails.TaxPercentage = taxPercentage;
            billDetails.AmountExcludingGST = totalAmt;
            billDetails.AmountIncludingGST = amtIncludingGST;
            billDetails.OrderTime = orderTime;

            ViewBag.tableNumber = tableNumber; 

            return View(billDetails);
        }
    }
}
