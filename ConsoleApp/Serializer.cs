using System.Text.Json;

public abstract class Serializer<T>
{
    private const string Filename = $"{nameof(T)}.json";
    
    public static async Task SerializeObject(T instance)
    {
        if (!File.Exists(Filename))
        {
            await using var stream = File.Create(Filename);
            await JsonSerializer.SerializeAsync(stream, new List<T> { instance });
        }
        else
        {
            var existingObjects = await DeserializeObjects();
            existingObjects.Add(instance);
            await SerializeObjects(existingObjects);
        }
    }

    
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