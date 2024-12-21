namespace restaurantOrderManagement.Models
{
    public class InvoiceModel
    {
        public int OpMode { get; set; }
        public long InvoiceID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int OrderID { get; set; }
        public decimal AmountExcludingGST { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal AmountIncludingGST { get; set; }
        public int PaymentMode { get; set; } // 1 = UPI, 2 = Cash, 3 = Card
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}
