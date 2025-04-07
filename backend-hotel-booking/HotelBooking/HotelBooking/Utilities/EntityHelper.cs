using Newtonsoft.Json;

namespace HotelBooking.Utilities;

public static class EntityHelper
{
    public static bool ApplyUpdatesFromJson<T>(this T existingEntity, string json)
    {
        bool updated = false;
        var newEntity = JsonConvert.DeserializeObject<T>(json);
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            var newProperty = property.GetValue(newEntity);
            if (newProperty != null && !newProperty.Equals(property.GetValue(existingEntity)))
            {
                property.SetValue(existingEntity, newProperty);
                updated = true;
            }
        }

        return updated;
    }
}