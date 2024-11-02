using System.Xml.Serialization;

namespace ConsoleApp;

[Serializable]
[XmlInclude(typeof(City))]
[XmlInclude(typeof(Deal))]
[XmlInclude(typeof(Resource))]
[XmlInclude(typeof(Imported))]
[XmlInclude(typeof(Exported))]
[XmlInclude(typeof(ManMade))]
[XmlInclude(typeof(Natural))]
[XmlInclude(typeof(PublicVehicle))]
[XmlInclude(typeof(RecreationalSpace))]
[XmlInclude(typeof(Resident))]
[XmlInclude(typeof(Residential))]
[XmlInclude(typeof(Route))]
[XmlInclude(typeof(Schedule))]
[XmlInclude(typeof(Workplace))]
public abstract class SerializableObject;

[Serializable]
public abstract class SerializableObject<T> : SerializableObject where T : SerializableObject<T>
{
    public static List<T> _instances = [];
    private const string Filename = "Instances.xml";

    public static Task<List<T>> GetAllInstances()
    {
        return Task.FromResult(_instances);
    }

    public static async Task SerializeAll()
    {
        if (_instances.Count == 0)
        {
            return;
        }

        var xRoot = new XmlRootAttribute
        {
            ElementName = "message",
            IsNullable = true
        };

        var serializer = new XmlSerializer(typeof(List<SerializableObject>), xRoot); 
        var combines = new List<SerializableObject>();

        if (File.Exists(Filename) && new FileInfo(Filename).Length > 0)
        {
            using var reader = new StreamReader(Filename);
            var existing = (List<SerializableObject>)serializer.Deserialize(reader)!;
            combines.AddRange(existing);
        }

        combines.AddRange(_instances);
        
        await using (var writer = new StreamWriter(Filename))
        {
            serializer.Serialize(writer, combines);
        }

        _instances.Clear();
    }

    public static Task LoadAll()
    {
        if (File.Exists(Filename) && new FileInfo(Filename).Length > 0)
        {
            var xRoot = new XmlRootAttribute
            {
                ElementName = "message",
                IsNullable = true
            };

            var serializer = new XmlSerializer(typeof(List<SerializableObject>), xRoot); 

            try
            {
                using var reader = new StreamReader(Filename);
                var existing = (List<SerializableObject>)serializer.Deserialize(reader)!;
                _instances = existing as List<T> ?? []; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading instances: {ex.Message}");
                _instances = [];
            }
        }
        else
        {
            _instances = [];
        }
        
        return Task.CompletedTask;
    }
}