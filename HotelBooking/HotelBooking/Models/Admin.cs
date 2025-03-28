namespace HotelBooking.Models;

public class Admin: User
{
    public int AdminId { get; set; }
    
    public string EmployeeCode { get; set; }


    public Admin(int id, string name, string email, string password, string employeeCode)
        : base(id, name, email, password, Role.Admin)
    {
        AdminId = id;
        EmployeeCode = employeeCode;
    }
}