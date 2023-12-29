using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace AdaptiveBpm
{
    public class FileService : IFileService
    {
        private readonly string DIRECTORY = $"{Application.persistentDataPath}";

        public async Task SaveFile<T>(string fileName, T data)
        {
            // Create the file, or overwrite if the file exists.
            var path = $"{DIRECTORY}/{fileName}";
            using var streamWriter = new StreamWriter($"{DIRECTORY}/{fileName}");
            await streamWriter.WriteAsync(JsonConvert.SerializeObject(data));
        }

        public T GetFile<T>(string fileName)
        {
            var s = File.ReadAllText($"{DIRECTORY}/{fileName}");
            return JsonConvert.DeserializeObject<T>(s);
        }

        public void DeleteFile(string fileName)
        {
            File.Delete($"{DIRECTORY}/{fileName}");
        }

        public bool Exists(string fileName)
        {
            return File.Exists($"{DIRECTORY}/{fileName}");
        }
    }

}
