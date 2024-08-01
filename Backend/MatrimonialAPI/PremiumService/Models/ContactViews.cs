namespace PremiumService.Models
{
    public class ContactViews
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ViewedUserId { get; set; }
        public DateTime ViewedTime { get; set; }
    }
}
