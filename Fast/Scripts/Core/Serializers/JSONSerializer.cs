using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Fast.Serializers
{
    class JSONSerializer
    {
        public static bool SerializeObject(string filepath, object to_serialize)
        {
            bool ret = false;

            string data = Newtonsoft.Json.JsonConvert.SerializeObject(to_serialize, Newtonsoft.Json.Formatting.Indented);
            
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }

            File.WriteAllBytes(filepath, new byte[0]);

            File.WriteAllText(filepath, data);

            ret = true;
            
            return ret;
        }

        public static bool DeSerializeObject<T>(string filepath, ref T deserialized_object)
        {
            bool ret = false;

            if (Directory.Exists(Application.persistentDataPath))
            {
                if (File.Exists(filepath))
                {
                    string text = File.ReadAllText(filepath);

                    deserialized_object = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(text);

                    if (deserialized_object != null)
                    {
                        ret = true;
                    }
                }
            }

            return ret;
        }

        public static bool SerializeObjectPersistenPath(string save_name, object to_serialize)
        {
            bool ret = false;

            string file_path = Application.persistentDataPath + "/" + save_name + ".json";

            ret = SerializeObject(file_path, to_serialize);

            return ret;
        }

        public static bool DeSerializeObjectPersistentPath<T>(string save_name, ref T deserialized_object)
        {
            bool ret = false;

            string file_path = Application.persistentDataPath + "/" + save_name + ".json";

            ret = DeSerializeObject(file_path, ref deserialized_object);

            return ret;
        }

#if UNITY_EDITOR

        public static bool SerializeObjectAssetsPath(string assets_filepath, object to_serialize, bool refresh_asset_database = true)
        {
            bool ret = false;

            string path = Application.dataPath + "/" + assets_filepath;

            FileInfo info = new FileInfo(path);

            Directory.CreateDirectory(info.Directory.FullName);

            ret = SerializeObject(path, to_serialize);

            if (refresh_asset_database)
            {
                AssetDatabase.SaveAssets();

                AssetDatabase.Refresh();
            }

            ret = true;

            return ret;
        }

        public static bool DeSerializeObjectAssetsPath<T>(string assets_filepath, ref T deserialized_object)
        {
            bool ret = false;

            string path = Application.dataPath + "/" + assets_filepath;

            ret = DeSerializeObject(path, ref deserialized_object);

            return ret;
        }

#endif

    }
}
