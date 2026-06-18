using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IBIZWebApp.Models
{
    public class FlexibleDecimalConverter : JsonConverter<decimal?>
    {
        public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetDecimal();
            }
            if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                if (decimal.TryParse(str, out var value))
                    return value;
                if (string.IsNullOrWhiteSpace(str))
                    return null;
            }
            return null;
        }
        public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteNumberValue(value.Value);
            else
                writer.WriteNullValue();
        }
    }
}
