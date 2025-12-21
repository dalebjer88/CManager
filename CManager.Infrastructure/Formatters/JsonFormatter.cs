using System.Text.Json;
using CManager.Domain.Interfaces;

namespace CManager.Infrastructure.Formatters;

public class JsonFormatter : IJsonFormatter
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public string Serialize<T>(T obj) => JsonSerializer.Serialize(obj, _options);

    public T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, _options);
}