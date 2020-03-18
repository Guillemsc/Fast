using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Fast.Serializers
{
    class StreamSerializer
    {
        public static async Task SerializeToPathAsync(string filepath, Stream to_serialize)
        {
            bool can_create = FileUtils.CreateAllFilepathDirectories(filepath);

            if(!can_create)
            {
                FastService.MLog.LogError($"Directories could not be created for filepath: {filepath}");

                return;
            }

            FileStream outputFileStream = new FileStream(filepath, FileMode.Create);

            await to_serialize.CopyToAsync(outputFileStream);

            outputFileStream.Dispose();
        }

        public static async Task<Stream> DeSerializeFromPathAsync(string filepath)
        {
            if(!File.Exists(filepath))
            {
                FastService.MLog.LogError($"The file specified does not exist: {filepath}");

                return null;
            }

            FileStream outputFileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);

            MemoryStream stream = new MemoryStream();

            await outputFileStream.CopyToAsync(stream);

            outputFileStream.Dispose();

            return stream;
        }
    }
}
