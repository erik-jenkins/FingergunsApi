using System.Linq;
using FingergunsApi.App.Data.Models;

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
                    DisplayName="erik",
                    Hash="vbwuN6uWkaaW34HEsksxRYo80qKGjqcoEQTH05LWyfc=",
                    Salt="test"
                }
            };
            
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}