using System;
using System.Collections.Generic;
using System.IO;

namespace Fast
{
    public class FileUtils
    {
        public static bool CreateAllFilepathDirectories(string filepath)
        {
            FileInfo info = new FileInfo(filepath);

            DirectoryInfo directory_info = Directory.CreateDirectory(info.Directory.FullName);

            return directory_info.Exists;
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
