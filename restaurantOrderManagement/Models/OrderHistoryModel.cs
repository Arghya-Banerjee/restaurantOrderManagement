namespace restaurantOrderManagement.Models
{
    public class OrderHistoryModel
    {
        public int OpMode { get; set; }
        public string UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long OrderID { get; set; }
        public long InvoiceID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public decimal AmountExcludingGST {  get; set; }
        public decimal GSTAmount {  get; set; }
        public decimal AmountIncludingGST { get; set; }
        public int PaymentMode { get; set; }
        public int ViewType { get; set; }
    }
}
