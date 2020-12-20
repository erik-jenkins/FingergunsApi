using FingergunsApi.App.Models;
using Microsoft.EntityFrameworkCore;

namespace FingergunsApi.App
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}