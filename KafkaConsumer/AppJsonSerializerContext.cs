namespace KafkaConsumer;

using System.Text.Json.Serialization;

[JsonSerializable(typeof(UserContext))]
[JsonSerializable(typeof(DisburseCommand))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
public partial class AppJsonSerializerContext : JsonSerializerContext
{
}
