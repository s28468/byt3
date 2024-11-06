using System.Xml.Serialization;

 namespace ConsoleApp.Helpers;

 public abstract class SerializableObject<T>
 {
     private static readonly string Filename = $"{typeof(T).Name}.xml";
     protected static List<T> _instances = [];
        
     public static Task<List<T>> GetAllInstances()
     {
         return Task.FromResult(_instances);
     }

     private static async Task ClearFile()
     {
         if (File.Exists(Filename))
         {
             File.Delete(Filename);
         }
         await using var stream = File.Create(Filename); 
     }

     public static async Task SerializeAll()
     {
         if (_instances.Count == 0)
             return;
         
         var xRoot = new XmlRootAttribute
         {
             ElementName = "message",
             IsNullable = true
         };
         var serializer = new XmlSerializer(typeof(List<T>), xRoot);
         
         await using (var stream = File.Create(Filename))
         {
             serializer.Serialize(stream, _instances);
         }
         
         _instances.Clear();
     }

     public static void LoadAll()
     {
         if (File.Exists(Filename) && new FileInfo(Filename).Length > 0)
         {
             var xRoot = new XmlRootAttribute
             {
                 ElementName = "message",
                 IsNullable = true
             };

             var serializer = new XmlSerializer(typeof(List<T>), xRoot);

             try
             {
                 using var reader = new StreamReader(Filename);
                 var existing = (List<T>)serializer.Deserialize(reader);
                 
                 if (existing != null)
                 {
                     _instances.AddRange(existing);
                 }
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"Error loading instances: {ex.Message}");
                 _instances = []; 
             }
             
             ClearFile();
         }
         else
         {
             _instances = []; 
         }
     }
 }
