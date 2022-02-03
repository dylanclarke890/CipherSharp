using System.IO;

namespace CipherSharp.Utility.FileHandling
{
    public class FileHandlingService
    {
        public string GetFile(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            
            var relPath = "../../../data/" + path;
            if (File.Exists(relPath))
            {
                return File.ReadAllText(relPath);
            }

            return string.Empty;
        }
    }
}
