namespace restaurantOrderManagement.Models
{
    public class UserSec
    {
        public long Id { get; set; }
        public long MobileNumber { get; set; }
        public string Email { get; set; }
        public int? IsActive { get; set; }
        public string UserId { get; set; }
        public string? PassCode { get; set; }
        public int UserType { get; set; }
    }
}
