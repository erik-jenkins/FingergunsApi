namespace FingergunsApi.App.Data.Models
{
    public class User
    {
        public int UserId { get; init; }
        public string Email { get; init; }
        public string DisplayName { get; init; }
        public string Hash { get; init; }
        public string Salt { get; init; }
    }
}