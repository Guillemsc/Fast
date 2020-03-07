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

            string file_path = Application.persistentDataPath + "/" + save_name + ".bin";

            File.WriteAllBytes(file_path, to_serialize);

            return ret;
        }

        public static bool DeSerializeFromPersistentPath(string save_name, ref byte[] deserialized_file)
        {
            bool ret = false;

            string file_path = Application.persistentDataPath + "/" + save_name + ".bin";

            if (File.Exists(file_path))
            {
                deserialized_file = File.ReadAllBytes(file_path);

                ret = true;
            }

            return ret;
        }
    }
}
