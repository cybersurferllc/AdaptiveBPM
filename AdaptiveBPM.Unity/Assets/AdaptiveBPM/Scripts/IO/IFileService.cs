using System.Threading.Tasks;

namespace AdaptiveBpm
{
    public interface IFileService
    {
        public Task SaveFile<T>(string fileName, T data);
        public T GetFile<T>(string fileName);
        public void DeleteFile(string fileName);
        public bool Exists(string fileName);
    }

}
