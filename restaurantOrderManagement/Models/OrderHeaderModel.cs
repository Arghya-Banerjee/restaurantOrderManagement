namespace restaurantOrderManagement.Models
{
    public class OrderHeaderModel
    {
        public int Opmode { get; set; }
        public long OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int DeliveryMode { get; set; }
        public int TableNumber { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int OrderStatus { get; set; }
    }
}
