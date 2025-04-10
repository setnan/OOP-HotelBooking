using Dapper;
using System;
using System.Data;

namespace HotelBooking.Core.Utilities
{
    public class EnumAsStringTypeHandler<T> : SqlMapper.TypeHandler<T> where T : struct, Enum
    {
        public override void SetValue(IDbDataParameter parameter, T value)
        {
            parameter.Value = value.ToString();
        }

        public override T Parse(object value)
        {
            return Enum.Parse<T>(value.ToString() ?? string.Empty);
        }
    }
}