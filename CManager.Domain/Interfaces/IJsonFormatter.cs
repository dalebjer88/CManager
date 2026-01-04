namespace CManager.Domain.Interfaces;
public interface IJsonFormatter
{
    string Serialize<T>(T obj);
    T? Deserialize<T>(string json);
}
