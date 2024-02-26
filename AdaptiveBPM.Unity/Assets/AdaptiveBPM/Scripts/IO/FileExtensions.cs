using System.IO;

namespace AdaptiveBpm
{
    public static class FileExtensions
    {
        public static string unityDirectory = @"..\..\..\..\AdaptiveBpmUnity\Assets\AdaptiveBpm\Model";
        public static string UnityModelPath = Path.GetFullPath(@".\Assets\AdaptiveBpm\Model\models\model.zip");
        public static string UnityDataPath = Path.GetFullPath(@".\Assets\AdaptiveBpm\Model\Data\data.csv");
        public static string UnityTestDataPath = Path.GetFullPath(@".\Assets\AdaptiveBpm\Model\Data\test.csv");
    }
}