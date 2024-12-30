namespace restaurantOrderManagement.Models
{
    public class InvoiceGenerateModel
    {
        public int OpMode { get; set; }
        public long OrderID { get; set; }
        public long InvoiceID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<BillItemsModel> BillItems { get; set; }
        public decimal AmountExcludingGST { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal AmountIncludingGST { get; set; }
        public int PaymentMode { get; set; }
    }
}
