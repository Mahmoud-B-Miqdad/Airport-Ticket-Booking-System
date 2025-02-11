﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Domain.Utilities
{
    public static class EnumExtensions
    {
        public static T ParseEnum<T>(this string value, T defaultValue = default) where T : struct, Enum
        {
            return Enum.TryParse(value, true, out T result) && Enum.IsDefined(typeof(T), result) ? result : defaultValue;
        }
    }
}
