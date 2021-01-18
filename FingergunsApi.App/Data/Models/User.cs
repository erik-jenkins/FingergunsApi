namespace FingergunsApi.App.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }
    }
}