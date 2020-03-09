using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Fast.Serializers
{
    class JSONSerializer
    {
        public static bool SerializeToPath(string filepath, object to_serialize)
        {
            bool ret = false;

            FileUtils.CreateAllFilepathDirectories(filepath);

            string data = Newtonsoft.Json.JsonConvert.SerializeObject(to_serialize, Newtonsoft.Json.Formatting.Indented);

            FileUtils.DeleteFileIfExists(filepath);

            File.WriteAllBytes(filepath, new byte[0]);

            File.WriteAllText(filepath, data);

            ret = true;
            
            return ret;
        }

        public static bool DeSerializeFromPath<T>(string filepath, out T deserialized_object)
        {
            bool ret = false;

            deserialized_object = default;

            if (File.Exists(filepath))
            {
                string text = File.ReadAllText(filepath);

                deserialized_object = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(text);

                if (deserialized_object != null)
                {
                    ret = true;
                }
            }
            
            return ret;
        }

        public static bool SerializeToPersistentPath(string persistent_path, object to_serialize)
        {
            bool ret = false;

            string filepath = Application.persistentDataPath + "/" + persistent_path + ".json";

            ret = SerializeToPath(filepath, to_serialize);

            return ret;
        }

        public static bool DeSerializeFromPersistentPath<T>(string persistent_path, out T deserialized_object)
        {
            bool ret = false;

            string filepath = Application.persistentDataPath + "/" + persistent_path + ".json";

            ret = DeSerializeFromPath(filepath, out deserialized_object);

            return ret;
        }

#if UNITY_EDITOR

        public static bool SerializToAssetsPath(string assets_filepath, object to_serialize, bool refresh_asset_database = true)
        {
            bool ret = false;

            string filepath = Application.dataPath + "/" + assets_filepath;

            FileUtils.CreateAllFilepathDirectories(filepath);

            ret = SerializeToPath(filepath, to_serialize);

            if (refresh_asset_database)
            {
                AssetDatabase.SaveAssets();

                AssetDatabase.Refresh();
            }

            ret = true;

            return ret;
        }

        public static bool DeSerializeFromAssetsPath<T>(string assets_filepath, out T deserialized_object)
        {
            bool ret = false;

            string path = Application.dataPath + "/" + assets_filepath;

            ret = DeSerializeFromPath(path, out deserialized_object);

            return ret;
        }

#endif

    }
}
