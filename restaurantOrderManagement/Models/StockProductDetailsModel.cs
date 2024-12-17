namespace restaurantOrderManagement.Models
{
    public class StockProductDetailsModel
    {
        public int OpMode { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string SupplierName { get; set; }
        public decimal Price { get; set; }
        public string Unit {  get; set; }
    }
}
