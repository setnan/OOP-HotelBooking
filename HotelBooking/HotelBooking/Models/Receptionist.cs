namespace HotelBooking.Models;

public class Receptionist: User
{
    public int ReceptionistId { get; set; }
    public string EmployeeCode { get; set; }

    public Receptionist(int id, string name, string email, string password, string employeeCode)
        : base(id, name, email, password, Role.Receptionist)
    {
        ReceptionistId = id;
        EmployeeCode = employeeCode;
    }
}