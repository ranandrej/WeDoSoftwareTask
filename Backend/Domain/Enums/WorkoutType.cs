using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum WorkoutType
    {
        Cardio = 1,
        Strength = 2,
        Flexibility = 3
    }
}
