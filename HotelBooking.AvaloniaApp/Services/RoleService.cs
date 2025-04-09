using System;

namespace HotelBooking.AvaloniaApp.Services;

public class RoleService
{
    private static RoleService? _instance;
    public static RoleService Instance => _instance ??= new RoleService();

    private UserRole _currentRole = UserRole.Receptionist;

    private RoleService() { }

    public UserRole CurrentRole
    {
        get => _currentRole;
        set
        {
            _currentRole = value;
            RoleChanged?.Invoke(this, value);
        }
    }

    public event EventHandler<UserRole>? RoleChanged;

    public bool IsAdministrator => CurrentRole == UserRole.Administrator;
}

public enum UserRole
{
    Receptionist,
    Administrator
}
