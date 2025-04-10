using System.Runtime.InteropServices.JavaScript;

namespace HotelBooking.Core.Models;

public enum MealEnum
{
    Breakfast,
    Lunch,
    Dinner
}
public class Meal
{
    public int MealId { get; set; }
    public string Name { get; set; }
    public DateTime Date  { get; set; }
    public MealEnum MealType { get; set; }
}