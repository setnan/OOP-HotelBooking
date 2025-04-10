using System;

namespace HotelBooking.AvaloniaApp.Services;

public class RoleService
{
    private static readonly Lazy<RoleService> _instance = new Lazy<RoleService>(() => new RoleService());

    private UserRole _currentRole = UserRole.Receptionist;

    private RoleService() { }  // Private constructor for singleton

    public static RoleService Instance => _instance.Value;

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
