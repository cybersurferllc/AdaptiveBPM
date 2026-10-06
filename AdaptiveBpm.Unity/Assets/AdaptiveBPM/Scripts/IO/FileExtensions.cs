using System.IO;
using UnityEngine;

namespace AdaptiveBpm
{
    public static class FileExtensions
    {
        public static string UnityModelDirectory => Path.Combine(Application.dataPath, "AdaptiveBPM", "Model");
        public static string UnityModelPath => Path.Combine(UnityModelDirectory, "Models", "model.zip");
        public static string UnityDataPath => Path.Combine(UnityModelDirectory, "Data", "data.csv");
        public static string UnityTestDataPath => Path.Combine(UnityModelDirectory, "Data", "test.csv");
        
        public static string UnityPeristentDataPath => Application.persistentDataPath;
        public static string UnityPeristentModelPath => Path.Combine(UnityPeristentDataPath, "model.zip");
        public static string UnityPeristentDataPathData => Path.Combine(UnityPeristentDataPath, "data.csv");
        public static string UnityPeristentDataPathTestData => Path.Combine(UnityPeristentDataPath, "test.csv");

        public static void EnsurePersistentModelAssets()
        {
            Directory.CreateDirectory(UnityPeristentDataPath);

            CopyIfMissing(UnityDataPath, UnityPeristentDataPathData);
            CopyIfMissing(UnityTestDataPath, UnityPeristentDataPathTestData);
            CopyIfMissing(UnityModelPath, UnityPeristentModelPath);
        }

        private static void CopyIfMissing(string sourcePath, string destinationPath)
        {
            if (File.Exists(destinationPath) || !File.Exists(sourcePath))
            {
                return;
            }

            var directory = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.Copy(sourcePath, destinationPath, overwrite: false);
        }
    }
}
