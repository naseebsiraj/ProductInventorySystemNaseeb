using ProductInventorySystem.Models;

namespace ProductInventorySystem.Repository
{
    public interface ILoginRepository
    {
        // Get user details by Username and Password
        public Task<User> ValidateUser(string username, string passsword);
    }
}
