using Core;
using Microsoft.AspNetCore.Mvc;
using restaurantOrderManagement.Models;
using restaurantOrderManagement.Utility_Classes;

namespace restaurantOrderManagement.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult RateUs(int orderId)
        {
            ViewBag.OrderId = orderId; // Adding orderId to pass to Submit Rating
            return View();
        }

        [HttpPost]
        [Route("Feedback/SubmitRating")]
        public IActionResult SubmitRating(int foodRating, int waiterRating, int restaurantRating, int orderId)
        {
            try
            {
                RatingsModel ratings = new RatingsModel
                {
                    OpMode = 1,
                    FoodRating = foodRating,
                    WaiterRating = waiterRating,
                    RestaurantRating = restaurantRating,
                    OrderID = orderId
                };
                DBOperations<RatingsModel>.DMLOperation(ratings, Constant.usp_Rating);

                OrderHeaderModel orderHeader = new OrderHeaderModel
                {
                    Opmode = 4,
                    OrderID = orderId,
                    CreatedOn = DateTime.Now,
                    OrderDate = DateTime.Now
                };
                int tableNumber = DBOperations<OrderHeaderModel>.GetSpecific(orderHeader, Constant.usp_OrderHeader).TableNumber;

                string xmlFilePath = $"wwwroot/cart_{tableNumber}.xml";
                try
                {
                    if (System.IO.File.Exists(xmlFilePath))
                    {
                        System.IO.File.Delete(xmlFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"File deletion error: {ex.Message}");
                }

                // Generate Bill Invoice Code

                // Get InvoiceID
                long invoiceId = DBOperations< InvoiceModel >.GetSpecific( new InvoiceModel 
                { 
                    OpMode = 0, OrderID = orderId, 
                    InvoiceDate = DateTime.Now, 
                    CreatedOn = DateTime.Now
                },Constant.usp_Invoice).InvoiceID;

                // Get Item data
                List < BillItemsModel > billItems = DBOperations< BillItemsModel >.GetAllOrByRange( new BillItemsModel
                {
                    OpMode = 0,
                    TableNumber = tableNumber
                }, Constant.usp_BillItems);

                // Get Invoice Details
                InvoiceModel invoiceDetails = DBOperations< InvoiceModel >.GetSpecific( new InvoiceModel
                {
                    OpMode = 2,
                    InvoiceID = invoiceId,
                    CreatedOn= DateTime.Now,
                    InvoiceDate= DateTime.Now
                }, Constant.usp_Invoice);

                InvoiceGenerateModel invoiceGenerateDetails = new InvoiceGenerateModel
                {
                    OpMode = 0,
                    OrderID = orderId,
                    InvoiceID = invoiceId,
                    InvoiceDate = invoiceDetails.InvoiceDate,
                    CreatedBy = invoiceDetails.CreatedBy,
                    CreatedOn = invoiceDetails.CreatedOn,
                    AmountExcludingGST = invoiceDetails.AmountExcludingGST,
                    AmountIncludingGST = invoiceDetails.AmountIncludingGST,
                    GSTAmount = invoiceDetails.GSTAmount,
                    PaymentMode = invoiceDetails.PaymentMode,
                    BillItems = billItems
                };

                // Call GenerateBill Function
                InvoiceUtility.GenerateBillInvoice(invoiceGenerateDetails);

                // Mark order as Completed (OrderStatus = 1)
                OrderHeaderModel changeStatusDummy = new OrderHeaderModel
                {
                    TableNumber = tableNumber,
                    Opmode = 2,
                    OrderStatus = 1,
                    OrderDate = DateTime.Now,
                    CreatedOn = DateTime.Now
                };
                DBOperations<OrderHeaderModel>.DMLOperation(changeStatusDummy, Constant.usp_OrderHeader);

                //System.Diagnostics.Debug.WriteLine("Redirecting to CurrentOrders...");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
