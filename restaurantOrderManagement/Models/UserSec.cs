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

        public UserRole vUserRole;
    }

    public enum UserRole
    {
        Waiter = 1,
        Admin = 2,
        Cashier = 3,
        Manager = 4

    }


}
