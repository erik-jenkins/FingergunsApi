using System.Linq;
using FingergunsApi.App.Models;

namespace FingergunsApi.App.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            var users = new[]
            {
                new User
                {
                    Email="erik@example.com",
                    Username="erik",
                    Hash="test",
                    Salt="test"
                }
            };
            
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}