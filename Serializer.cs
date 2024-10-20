using System.Text.Json;

namespace ConsoleApp3;

public abstract class Serializer<T>
{
    private const string Filename = $"{nameof(T)}.json";
    
    public static async Task SerializeObjects(IEnumerable<T> objects)
    {
        await using var stream = File.Create(Filename);
        await JsonSerializer.SerializeAsync(stream, objects);
    }
    
    public static async Task<List<T>> DeserializeObjects()
    {
        await using var stream = File.OpenRead(Filename);
        return await JsonSerializer.DeserializeAsync<List<T>>(stream) ?? [];
    }
}