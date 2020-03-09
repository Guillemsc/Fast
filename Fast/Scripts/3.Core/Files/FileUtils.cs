using System;
using System.Collections.Generic;
using System.IO;

namespace Fast
{
    public class FileUtils
    {
        public static void CreateAllFilepathDirectories(string filepath)
        {
            FileInfo info = new FileInfo(filepath);
            Directory.CreateDirectory(info.Directory.FullName);
        }

        public static void DeleteFileIfExists(string filepath)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }
    }
}
