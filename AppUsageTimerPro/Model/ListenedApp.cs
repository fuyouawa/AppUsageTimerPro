using System;
using Newtonsoft.Json;

namespace AppUsageTimerPro
{
    public class ListenedApp
    {
        public string Name { get; set; }

        public ListenedApp(string name)
        {
            Name = name;
        }
    }

    public class ListenAppConverter : JsonConverter<ListenedApp>
    {
        public override void WriteJson(JsonWriter writer, ListenedApp? value, JsonSerializer serializer)
        {
            if (value == null)
                return;

            writer.WriteValue(value.Name);
        }

        public override ListenedApp? ReadJson(JsonReader reader, Type objectType, ListenedApp? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var s = (string?)reader.Value;
            if (s == null)
                return null;
            return new ListenedApp(s);
        }
    }
}