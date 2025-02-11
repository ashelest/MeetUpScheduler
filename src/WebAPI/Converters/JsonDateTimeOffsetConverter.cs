using System.Text.Json;

namespace WebAPI.Converters;

public class JsonDateTimeOffsetConverter : System.Text.Json.Serialization.JsonConverter<DateTimeOffset>
{
	public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return DateTimeOffset.Parse(reader.GetString());
	}

	public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
	}
}