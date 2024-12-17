using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

public class CartController : Controller
{
    private readonly string xmlFilePath = "wwwroot/cart.xml";

    [HttpPost]
    public IActionResult AddToCart(int itemId, string itemName, decimal price, int quantity)
    {
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
}

// CartItem Model
public class CartItem
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

