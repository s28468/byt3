using System.Text.Json;

namespace ConsoleApp;

[Serializable]
public abstract class SerializableObject<T> where T : SerializableObject<T>
{
    protected static List<T> _instances = [];
    private const string Filename = "Instances.json";
    
    public static Task<List<T>> GetAllInstances()
    {
        return Task.FromResult(_instances);
    } 
    
    public static async Task SerializeAll()
    {
        var jsonString = JsonSerializer.Serialize(_instances, new JsonSerializerOptions
           {
              WriteIndented = true
           });
        await File.WriteAllTextAsync(Filename, jsonString);
        _instances.Clear();
    }
    
    public static async Task LoadAll()
    {
        if (File.Exists(Filename))
        {
            await using var stream = File.OpenRead(Filename);
            _instances = await JsonSerializer.DeserializeAsync<List<T>>(stream) ?? [];
        }
    }
}