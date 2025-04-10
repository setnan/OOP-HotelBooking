using System.Runtime.InteropServices.JavaScript;

namespace HotelBooking.Core.Models;

public enum MealType
{
    Breakfast,
    Lunch,
    Dinner
}
public class Meal
{
    public int MealId { get; set; }
    public int HotelId { get; set; }
    public string Name { get; set; }
    public DateTime Date  { get; set; }
    public MealType MealType { get; set; }
}