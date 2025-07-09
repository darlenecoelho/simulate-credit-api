using SimulateCredit.Domain.ValueObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimulateCredit.Infrastructure.Converters;

public class CurrencyJsonConverter : JsonConverter<Currency>
{
    public override Currency Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return Currency.From(value ?? throw new JsonException("Currency code is null"));
    }

    public override void Write(Utf8JsonWriter writer, Currency value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Code);
    }
}
