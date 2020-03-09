using System;
using System.IO;
using UnityEngine;

namespace Fast.Serializers
{
    class ByteSerializer
    {
        public static bool SerializeToPersistentPath(string save_name, byte[] to_serialize)
        {
            bool ret = false;

            string filepath = Application.persistentDataPath + "/" + save_name + ".bin";

            FileUtils.CreateAllFilepathDirectories(filepath);

            File.WriteAllBytes(filepath, to_serialize);

            return ret;
        }

        public static bool DeSerializeFromPersistentPath(string save_name, ref byte[] deserialized_file)
        {
            bool ret = false;

            string filepath = Application.persistentDataPath + "/" + save_name + ".bin";

            if (File.Exists(filepath))
            {
                deserialized_file = File.ReadAllBytes(filepath);

                ret = true;
            }

            return ret;
        }
    }
}
