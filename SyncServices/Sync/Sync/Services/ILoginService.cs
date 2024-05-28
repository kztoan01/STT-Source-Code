using Sync.Model;

namespace Sync.Services
{
    public interface ILoginService
    {
        Task<User> LoginAsync(string username, string password);

    }
}
