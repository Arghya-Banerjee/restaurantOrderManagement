using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using restaurantOrderManagement.Models;
using Core;

public class CartController : Controller
{   
    //private readonly string xmlFilePath = "wwwroot/cart.xml";

    [HttpPost]
    public IActionResult AddToCart(int itemId, string itemName, decimal price, int quantity)
    {
        int tableNumber = HttpContext.Session.GetObjectFromJson<int>("TableNumber");
        string xmlFilePath = $"wwwroot/cart_{tableNumber}.xml";
        if (!System.IO.File.Exists(xmlFilePath))
        {
            var newCart = new XElement("cart");
            newCart.Save(xmlFilePath);
        }

        var cart = XElement.Load(xmlFilePath);

        var newItem = new XElement("Item",
            new XElement("ItemId", itemId),
            new XElement("ItemName", itemName),
            new XElement("Quantity", quantity),
            new XElement("Price", price)
        );

        cart.Add(newItem);
        cart.Save(xmlFilePath);

        return Json(new { success = true });
    }

    [HttpGet]
    public IActionResult ViewCart()
    {
        int tableNumber = HttpContext.Session.GetObjectFromJson<int>("TableNumber");
        string xmlFilePath = $"wwwroot/cart_{tableNumber}.xml";

        if (!System.IO.File.Exists(xmlFilePath))
        {
            return View(new List<CartItem>());
        }

        var cart = XElement.Load(xmlFilePath);
        var items = cart.Elements("Item").Select(x => new CartItem
        {
            ItemId = int.Parse(x.Element("ItemId")?.Value ?? "0"),
            ItemName = x.Element("ItemName")?.Value,
            Quantity = int.Parse(x.Element("Quantity")?.Value ?? "0"),
            Price = decimal.Parse(x.Element("Price")?.Value ?? "0")
        }).ToList();

        return View(items);
    }

    [HttpPost]
    public IActionResult UpdateQuantity(int updateItemId, int newQuantity)
    {
        int tableNumber = HttpContext.Session.GetObjectFromJson<int>("TableNumber");
        string xmlFilePath = $"wwwroot/cart_{tableNumber}.xml";

        if (!System.IO.File.Exists(xmlFilePath))
        {
            return NotFound(new { success = false, message = "Cart not found." });
        }

        var cart = XElement.Load(xmlFilePath);
        var itemToUpdate = cart.Elements("Item").FirstOrDefault(x => x.Element("ItemId")?.Value == updateItemId.ToString());
        if(itemToUpdate != null)
        {
            itemToUpdate.SetElementValue("Quantity", newQuantity);
            cart.Save(xmlFilePath);
            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = false });
        }
    }

    [HttpPost]
    public IActionResult GetTableNumber(int tableNumber)
    {
        HttpContext.Session.SetObjectAsJson("TableNumber", tableNumber);
        return Json(new {success = true});
    }

    public IActionResult PlaceOrder()
    {

        ViewBag.OrderId = 121243;
        int tableNumber = HttpContext.Session.GetObjectFromJson<int>("TableNumber");
        string UserID = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails").UserId;
        string xmlFilePath = $"wwwroot/cart_{tableNumber}.xml";

        XDocument xmlDoc = XDocument.Load(xmlFilePath);
        var items = xmlDoc.Descendants("Item").Select(item => new
        {
            ItemID = (int)item.Element("ItemId"),
            Quantity = (int)item.Element("Quantity")
        });

        // Insert Order Header Details

        OrderHeaderModel orderHeader = new OrderHeaderModel();
        orderHeader.Opmode = 0;
        orderHeader.OrderDate = DateTime.Now;
        orderHeader.DeliveryMode = 0;
        orderHeader.TableNumber = tableNumber;
        orderHeader.CreatedBy = UserID;
        orderHeader.CreatedOn = DateTime.Now;
        orderHeader.OrderStatus = 0;
        int res = DBOperations<OrderHeaderModel>.DMLOperation(orderHeader, Constant.usp_OrderHeader);

        // Fetch OrderID
        
        OrderHeaderModel orderHeaderDummy = new OrderHeaderModel();
        orderHeaderDummy.Opmode = 1;
        orderHeaderDummy.OrderDate = DateTime.Now;
        orderHeaderDummy.TableNumber = tableNumber;
        orderHeaderDummy.CreatedOn = DateTime.Now;
        long headerID = DBOperations<OrderHeaderModel>.GetSpecific(orderHeaderDummy, Constant.usp_OrderHeader).OrderID;
        ViewBag.OrderID = headerID;

        // Insert Item Details

        foreach (var item in items)
        {
            OrderDetailsModel itemDetails = new OrderDetailsModel();
            itemDetails.OpMode = 0;
            itemDetails.OrderID = headerID;
            itemDetails.MenuID = item.ItemID;
            itemDetails.Quantity = item.Quantity;
            itemDetails.CreatedBy = UserID;
            itemDetails.CreatedOn = DateTime.Now;
            int x = DBOperations<OrderDetailsModel>.DMLOperation(itemDetails, Constant.usp_OrderDetails);
        }

        System.IO.File.Delete(xmlFilePath); //delete the XML after placing order
        return View();
    }

    public IActionResult NewOrder()
    {
        HttpContext.Session.SetObjectAsJson("TableNumber", 0);
        return RedirectToAction("WelcomeWaiter", "Waiter");
    }
}

// CartItem Model
public class CartItem
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

