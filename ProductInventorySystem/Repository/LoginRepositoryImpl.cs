using Microsoft.EntityFrameworkCore;
using ProductInventorySystem.Models;

namespace ProductInventorySystem.Repository
{
    public class LoginRepositoryImpl : ILoginRepository
    {
        // Virtual base private variable
        private readonly ProductInventoryDbContext _context;

        // Dependency Injection
        public LoginRepositoryImpl(ProductInventoryDbContext context)
        {
            _context = context;
        }

        // Validate User
        public async Task<User> ValidateUser(string username, string password)
        {
            try
            {
                if(_context !=null)
                {
                    User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);

                    if (user != null)
                    {
                        return user;
                    }

                }
                return null;
            }
            catch(Exception ex) 
            {
                throw;
                
            }
        }

    }
}
