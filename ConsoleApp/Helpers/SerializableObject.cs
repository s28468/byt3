using System.Xml.Serialization;
using ConsoleApp.Models;

namespace ConsoleApp.Helpers;
 
 [XmlInclude(typeof(Imported))]
 [XmlInclude(typeof(Exported))]
 [XmlInclude(typeof(ManMade))]
 [XmlInclude(typeof(Natural))]
 public abstract class SerializableObject<T>
 {
     private static readonly string Filename = $"{typeof(T).Name}.xml";
     protected static List<T> _instances = [];
        
     public static Task<List<T>> GetAllInstances()
     {
         return Task.FromResult(_instances);
     }

     public static async Task ClearFile()
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
                 
                 var existing = new List<T>();
                 var deserializedList = (List<T>)serializer.Deserialize(reader);
                 
                 if (deserializedList != null)
                 {
                     existing.AddRange(deserializedList); 
                 }
                 
                 if (typeof(T) == typeof(Resource))
                 {
                     Resource.SortSubclasses(existing.Cast<Resource>().ToList());
                 }

                 _instances.AddRange(existing);
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"Error while deserializing {typeof(T).Name}: {ex.Message}");
                 _instances = [];
             }
             
             _ = ClearFile();
         }
         else
         {
             _instances = [];
         }
     }

 }
