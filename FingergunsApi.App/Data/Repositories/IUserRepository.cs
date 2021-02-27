using System.Threading.Tasks;
using FingergunsApi.App.Data.Models;

namespace FingergunsApi.App.Data.Repositories
{
    public interface IUserRepository
    {
        public Task<User> GetUserByEmailAsync(string email);

        public Task<bool> IsEmailTakenAsync(string email);

        public Task<bool> IsDisplayNameTakenAsync(string displayName);

        public Task InsertUserAsync(User user);

        public Task Save();
    }
}