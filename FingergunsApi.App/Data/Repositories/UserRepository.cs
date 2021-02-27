using System.Threading.Tasks;
using FingergunsApi.App.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FingergunsApi.App.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsDisplayNameTakenAsync(string displayName)
        {
            return await _dbContext.Users.AnyAsync(u => u.DisplayName == displayName);
        }

        public async Task InsertUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}