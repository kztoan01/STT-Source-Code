using Microsoft.EntityFrameworkCore;
using Sync.Model;

namespace Sync.Services.iml
{
    public class LoginService : ILoginService
    {
        private readonly EFDataContext _context;

        public LoginService(EFDataContext context)
        {
            _context = context;
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            return user;
        }
    }
}
