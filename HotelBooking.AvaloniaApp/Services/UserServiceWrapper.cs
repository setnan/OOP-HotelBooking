using System.Threading.Tasks;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.Services;

public class UserServiceWrapper
{
    private readonly UserSession userSession;

    public UserServiceWrapper(UserSession userSession)
    {
        this.userSession = userSession;
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = await Task.Run(() => UserService.Authenticate(username, password));
        if (user != null)
        {
            userSession.CurrentUser = user;
        }
        return user;
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        return await Task.FromResult(userSession.CurrentUser);
    }

    public void Logout()
    {
        userSession.CurrentUser = null;
        UserService.Logout();
    }

    public bool IsAdmin(User user)
    {
        return UserService.IsAdmin(user);
    }

    public async Task SaveCredentialsAsync(string username, string password)
    {
        // For now, we'll just store them in memory since we don't want to save actual credentials
        await Task.CompletedTask;
    }
}
