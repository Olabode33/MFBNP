using Newtonsoft.Json;
using System;
using Abp.Extensions;

namespace PMSDemo
{
    public class TimeSpanToJsonStringConverter : JsonConverter<TimeSpan>
    {
        public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return default;
            }

            var str = (string)reader.Value;

            if (str.IsNullOrWhiteSpace())
            {
                return default;
            }

            return TimeSpan.Parse(str);
        }
    }
}
